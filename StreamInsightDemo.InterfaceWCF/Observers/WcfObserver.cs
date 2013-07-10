using StreamInsightDemo.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace StreamInsightDemo.InterfaceWCF.Observers
{
    // Define a service contract.
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.WcfObserver",
        CallbackContract = typeof(IWcfObserverCallback))]
    public interface IWCFObserver
    {
        [OperationContract]
        void Subscribe();

        [OperationContract]
        void Unsubscribe();
    }

    public interface IWcfObserverCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnCompleted();
        [OperationContract(IsOneWay = true)]
        void OnError(Exception error);
        [OperationContract(IsOneWay = true)]
        void OnNext(OutputEvent value);
    }

    [ServiceBehavior(IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single)]
    class WcfObserverService : IWCFObserver
    {
        List<IWcfObserverCallback> subscribersList = new List<IWcfObserverCallback>();

        public void Subscribe()
        {
            var wcfClientCallback = OperationContext.Current.GetCallbackChannel<IWcfObserverCallback>();
            if (!subscribersList.Contains(wcfClientCallback))
            {
                subscribersList.Add(wcfClientCallback);
            }
        }

        public void Unsubscribe()
        {
            var wcfClientCallback = OperationContext.Current.GetCallbackChannel<IWcfObserverCallback>();
            if (subscribersList.Contains(wcfClientCallback))
            {
                subscribersList.Remove(wcfClientCallback);
            }
        }

        public void OnCompleted()
        {
            Action(s => s.OnCompleted());
        }

        public void OnError(Exception error)
        {
            Action(s => s.OnError(error));
        }

        public void OnNext(OutputEvent value)
        {
            Action(s => s.OnNext(value));
        }

        void Action(Action<IWcfObserverCallback> call)
        {
            var disconnected = subscribersList.Where(s =>
            {
                try
                {
                    call(s);
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Connection already disposed, disconnecting client's channel");
                    return true;
                }
            }
            );
            disconnected.ToList().ForEach(s => subscribersList.Remove(s));
        }
    }

    public class WcfObserver : IObserver<OutputEvent>
    {
        string connectionString = @"Data Source=(localdb)\V11.0;Initial Catalog=streaminsight;Integrated Security=True;MultipleActiveResultSets=True";
        string queryString =
            "INSERT INTO AlertTable (Time,Site,Lsl,Usl,Tp) VALUES (@time,@site,@lsl,@usl,@tp)";


        //ServiceHost _selfHost;
        //WcfObserverService _observerService;

        //public WcfObserver(string baseAddress)
        //{
        //    _observerService = new WcfObserverService();
        //    _selfHost = new ServiceHost(_observerService, new Uri(baseAddress));

        //    _selfHost.AddServiceEndpoint(
        //             typeof(IWCFObserver),
        //             new WSDualHttpBinding(),
        //             "WcfObserverService");
        //    ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
        //    smb.HttpGetEnabled = true;
        //    _selfHost.Description.Behaviors.Add(smb);
        //    _selfHost.Open();
        //}
        public WcfObserver(string baseAddress)
        {

        }


        public void OnCompleted()
        {
            Debug.WriteLine("Wcf Observer OnCompleted");
            //_observerService.OnCompleted();
        }

        public void OnError(Exception error)
        {
            Debug.WriteLine("Wcf Observer OnError: " + error.Message);
            //_observerService.OnError(error);
        }

        public void OnNext(OutputEvent value)
        {
            Debug.WriteLine("Wcf Observer OnNext: " + value.ToString());
            //_observerService.OnNext(value);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@time", value.time);
                command.Parameters.AddWithValue("@site", value.site);
                command.Parameters.AddWithValue("@lsl", value.Lsl);
                command.Parameters.AddWithValue("@usl", value.Usl);
                command.Parameters.AddWithValue("@tp", value.Tp);

                // Open the connection in a try/catch block. 
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
    }
}