using StreamInsightDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using StreamInsightDemo.WpfApplication.WcfObservableService;

namespace StreamInsightDemo.WpfApplication
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        //Event Generator WCF
        private WCFObservableClient wcfClient = new WCFObservableClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //解析XML
            XDocument xd = XDocument.Load(pathTextBox.Text);
            var queryXML = from o in xd.Descendants(nodeTextBox.Text)
                           select o;

            foreach (XElement o in queryXML)
            {
                bool lowerLimitTemperatureResult = false;
                bool upperLimitTemperatureResult = false;
                bool nowTemperatureResult = false;

                int lowerLimitTemperature;
                int upperLimitTemperature;
                int nowTemperature;

                

                string temperature = o.Element("body").Element("temperature_log").Value;

                //Time
                string timeString = o.Element("head").Element("timestamp").Value; 

                //地點
                string siteString = temperature.Substring(
                    temperature.IndexOf("PrintToDatalog on Site", 0) + 1, 1);

                //溫度
                string lowerLimitTemperatureString = temperature.Substring(
                    temperature.IndexOf("LSL=", 0) + 4, 2);
                string upperLimitTemperatureString = temperature.Substring(
                    temperature.IndexOf("USL=", 0) + 4, 2);
                string nowTemperatureString = temperature.Substring(
                    temperature.IndexOf("RESULT=", 0) + 12, 2);

                lowerLimitTemperatureResult = int.TryParse(lowerLimitTemperatureString, out lowerLimitTemperature);
                upperLimitTemperatureResult = int.TryParse(upperLimitTemperatureString, out upperLimitTemperature);
                nowTemperatureResult = int.TryParse(nowTemperatureString, out nowTemperature);

                if (lowerLimitTemperatureResult && upperLimitTemperatureResult && nowTemperatureResult)
                {
                    InputEvent inputEvnet = new InputEvent();
                    inputEvnet.Tp = nowTemperature;
                    inputEvnet.Usl = upperLimitTemperature;
                    inputEvnet.Lsl = lowerLimitTemperature;
                    inputEvnet.time = timeString;
                    inputEvnet.site = siteString;

                    try
                    {
                        wcfClient.OnNext(inputEvnet);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("An exception was hit when sending the events: {0}", exception.Message);
                        return;
                    }

                    Thread.Sleep(1000);
                }
            }

            try
            {
                wcfClient.OnCompleted();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception encountered: {0}\n.Could not send the 'OnCompleted' message, please check that the client and the server are still online.", exception.Message);
            }
            Console.WriteLine("Press <Enter> to exit.");
            Console.ReadLine();
        }
    }
}
