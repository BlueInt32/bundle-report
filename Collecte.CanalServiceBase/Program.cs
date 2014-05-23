using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics;
using Tools;

namespace Collecte.CanalServiceBase
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
			log("Start in Real Service host mode.");
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new CanalBaseService()

			};
            ServiceBase.Run(ServicesToRun);
            #else
			log("Start in Debug Configuration.");
			CanalBaseService service = new CanalBaseService();
            service.DebugRun();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            #endif
        }

		public static void log(object value)
		{
			Log.Info("Collecte.ServiceBase", value.ToString());
			const string sSource = "CanalServiceBase";
			const string sLog = "Application";
			string sEvent = string.Format("Log Event: {0}", value);
			if (!EventLog.SourceExists(sSource))
				EventLog.CreateEventSource(sSource, sLog);
			EventLog.WriteEntry(sSource, sEvent);
		}
    }
}
