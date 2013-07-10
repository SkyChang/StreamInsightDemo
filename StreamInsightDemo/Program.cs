using System;
using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Reactive;
using System.Reactive.Linq;
using StreamInsightDemo.InterfaceWCF.Observers;
using StreamInsightDemo.InterfaceWCF.Observables;
using StreamInsightDemo.Model;

namespace StreamInsightDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string port = "8088";
            //輸入與輸出之WCF
            string wcfSourceURL = String.Format(@"http://localhost:{0}/StreamInsight/wcf/Source/", port);
            string wcfSinkURL = String.Format(@"http://localhost:{0}/StreamInsight/wcf/Sink/", port);

            using (var server = Server.Create("StreamInsightInstance1"))
            {
                string AppName = "TestApp";

                if (server.Applications.ContainsKey(AppName))
                {
                    server.Applications[AppName].Delete();
                }
                var app = server.CreateApplication(AppName);

                //WCF Artifacts
                var observableWcfSource = app.DefineObservable(() => new WcfObservable(wcfSourceURL));
                var observableWcfSink = app.DefineObserver(() => new WcfObserver(wcfSinkURL));

                var inputStream = observableWcfSource.ToPointStreamable(
                                i => PointEvent.CreateInsert<InputEvent>(DateTime.Now, i),
                                AdvanceTimeSettings.IncreasingStartTime);

                var query = from x in inputStream
                            where x.Tp > x.Usl || x.Tp < x.Lsl
                            select new OutputEvent { Tp = x.Tp , Usl = x.Usl , 
                                site = x.site , Lsl = x.Lsl , time = x.time };

                Console.WriteLine("StreamInsight application using wcf artifacts (WcfObservable)");
                using (query.Bind(observableWcfSink).Run())
                {
                    Console.WriteLine("Sending events...");
                    Console.WriteLine("Press <Enter> to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
