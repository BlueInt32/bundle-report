using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics;
using Tools;

namespace Collecte.MorningService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
			log4net.Config.XmlConfigurator.Configure();
            #if (!DEBUG)
			Log("Start in Real Service host mode.");
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new CollecteService()
			};
            ServiceBase.Run(ServicesToRun);
            #else
			Log("Start in Debug Configuration.");
			CollecteService service = new CollecteService();
            service.DebugRun();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            #endif
        }

        public static void Log(object value)
        {
			Tools.Log.Info("Collecte.MorningService", value.ToString());
        	//if (ConfigurationManager.AppSettings["debugMode"] != "true") return;
        	const string sSource = "CollecteService";
        	const string sLog = "Application";
        	string sEvent = string.Format("Log Event: {0}", value);
        	if (!EventLog.SourceExists(sSource))
        		EventLog.CreateEventSource(sSource, sLog);
        	EventLog.WriteEntry(sSource, sEvent);
        }


    }
}
