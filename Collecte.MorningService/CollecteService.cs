using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Configuration;
using System.Text;
using System.Timers;
using Collecte.DAL;
using Collecte.DTO;

namespace Collecte.MorningService
{
	[System.ComponentModel.DesignerCategory("Code")] // this tell vs2010 to open the file in code mode directly
	public partial class CollecteService : System.ServiceProcess.ServiceBase
    {
    	readonly bool _canTriggerSeveralTimesADay = false;

        readonly Timer timer;
        readonly uint retrieveInterval;
        DateTime _lastDayDone;
        
        public CollecteService()
        {
			_canTriggerSeveralTimesADay = ConfigurationManager.AppSettings["CanTriggerSeveralTimesADay"] == "true";

			_lastDayDone = DateTime.Now.AddDays(-1);
			Program.Log(string.Format("Démarrage date de derniere execution : {0}", _lastDayDone));
            retrieveInterval = 1000 * Convert.ToUInt32(ConfigurationManager.AppSettings["secondTimerTick"]);
            InitializeComponent();
			ServiceName = "CollecteService";
            timer = new Timer();
            AutoLog = true;
        }
        public void DebugRun()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
			timer.Interval = retrieveInterval;
            timer.Start();
        }


        protected override void OnStop()
        {
            
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            timer.Stop();
			int h = Convert.ToInt32(ConfigurationManager.AppSettings["heureDuJour"].Split('h')[0]);
			int m = Convert.ToInt32(ConfigurationManager.AppSettings["heureDuJour"].Split('h')[1]);
			//Program.log(string.Format("It's {0}h{1}, not {2}h{3}.", DateTime.Now.Hour, DateTime.Now.Minute, h, m));
            if ((DateTime.Now.Hour == h && DateTime.Now.Minute == m && DateTime.Now.Date > _lastDayDone.Date)
                || ConfigurationManager.AppSettings["debugMode"] == "true"
                )
			{
				Program.Log(string.Format("It's time ! {0} as in config",ConfigurationManager.AppSettings["heureDuJour"] ));
				if(ConfigurationManager.AppSettings["debugMode"] == "true")
					Program.Log("(Service is in Debug mode)");
				ServiceProcess mp = new ServiceProcess();
				List<User> listNewUserDay = mp.RetrieveNewUsers();
				Program.Log("User List : " + listNewUserDay.Count);
				UserDal uDal = new UserDal();

				string path = ConfigurationManager.AppSettings["localCsvFilesDirectory"];
				string csvfilePath = uDal.CreateCsvFileFtpFromList(path, listNewUserDay, "CSat");
				//string csvCPlusfilePath = uDal.CreateCsvFileFtpFromList(path, listCanalPlus, "CPlus");
				//string csvCSatfilePath = uDal.CreateCsvFileFtpFromList(path, listCanalSat, "CSat");
				//Program.log(csvCPlusfilePath);
				//mp.PushFileFTP(csvCPlusfilePath, "cplus");
				Program.Log(csvfilePath);
				mp.MailPerformancePushFileFTP(csvfilePath, "csat");
				if(!_canTriggerSeveralTimesADay)
					_lastDayDone = DateTime.Now;
            } 
            
            if (DateTime.Now.ToString("yyyyMMdd") == ConfigurationManager.AppSettings["finOperation"])
            {
                // l'opé est terminée, on ne restart par le timer et il faudra désinstaller le service.
                //OnStop();
				Program.Log("Stopping service, end Of Operation has been reached : " + ConfigurationManager.AppSettings["finOperation"]);
                return;
            }
			//Program.log("Timer restart.");
            timer.Start();
        }

    }
}

