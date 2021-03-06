﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Timers;
using Collecte.DAL;
using Collecte.DTO;
using Collecte.Logic;
using Timer = System.Timers.Timer;
using Tools.Utils;
using System.IO;

namespace Collecte.CanalServiceBase
{
	[/*System.ComponentModel.*/DesignerCategory("")] // this tells visual studio to open the file in code mode directly (System.ComponentModel seems to be necessary for VS)
	public partial class CanalBaseService : ServiceBase
	{
		readonly bool _canTriggerSeveralTimesADay = false;

		readonly Timer timer;
		readonly uint retrieveInterval;

		int DailyExecutionHour = Convert.ToInt32(WebConfig.Get.dailyExecutionTime.Split('h')[0]);
		int DailyExecutionMinute = Convert.ToInt32(WebConfig.Get.dailyExecutionTime.Split('h')[1]);

		DateTime LastBundleModifiedDate;

		public DateTime? LastExecutionExactTime = null;
		
		public CanalBaseService()
		{
			CultureInfo culture = new CultureInfo(WebConfig.Get.DefaultCulture);

			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
			_canTriggerSeveralTimesADay = WebConfig.Get.CanTriggerSeveralTimesADay == "true";

			Program.log(string.Format("Démarrage date de derniere execution : {0}", LastExecutionExactTime.HasValue ? LastExecutionExactTime.Value.ToString() : "--" ));
			retrieveInterval = 1000 * Convert.ToUInt32(WebConfig.Get.secondTimerTick);
			InitializeComponent();
			ServiceName = "CanalBaseService";
			timer = new Timer();
			AutoLog = true;

			CsvFileGrabber csvFileGrabber = new CsvFileGrabber();

			Mailer mailer = new Mailer();
			mailer.LogDelegate = Program.log;
			string emailConf = ConfigurationManager.AppSettings["NotificationEmail"];
			mailer.SendMail(emailConf, "[Moulinette Canal Collecte] Démarrage du service", "<a href='http://monitoring.collecte.canalplus.clients.rappfrance.com'>Monitoring</a>", null, ConfigurationManager.AppSettings["NotificationEmail_CC"]);

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
			if (DateTime.Now.DayOfWeek.Equals(DayOfWeek.Saturday) || DateTime.Now.DayOfWeek.Equals(DayOfWeek.Sunday))
				return;
			timer.Stop();
			bool hasTriggeredTodayYet = LastBundleModifiedDate == BundleLogic.GetToday();
			if (hasTriggeredTodayYet)
			{
				timer.Start();
				return;
			}


			bool rightTime = (DateTime.Now.Hour == DailyExecutionHour && DateTime.Now.Minute == DailyExecutionMinute) || WebConfig.Get.forceRightTime == "true";
			if (!rightTime)
			{
				timer.Start();
				return;
			}
			Program.log("It's business time !");
			BundleLogic bundleLogic = new BundleLogic();
			DateTime Today0h = BundleLogic.GetToday();
			var bundleGet = bundleLogic.GetBundleByDate(Today0h);
			if (!bundleGet.Result && bundleGet.Message == "Probleme de connexion à la base.")
			{
				Program.log(bundleGet.Message);
				Stop();
			}

			Program.log("Bundle déjà créé ? " + bundleGet.Result +"("+ bundleGet.Message+")");

			// predicates
			bool csvNotYetCreated = !bundleGet.Result || (bundleGet.Result && bundleGet.ReturnObject.Status == BundleStatus.NoFileCreated);
			bool featureCsvCreationActivated = WebConfig.Get.createCsv == "true";
			Program.log("yesterdayCsvNotYetCreated : " + csvNotYetCreated);
			Program.log("rightTime : " + rightTime);
			Program.log("featureCsvCreationActivated : " + featureCsvCreationActivated);


			if (csvNotYetCreated && featureCsvCreationActivated)
			{

				Program.log(string.Format("It's time ! ({0}h{1}) {2}", DateTime.Now.Hour, DateTime.Now.Minute, WebConfig.Get.forceRightTime == "true" ? "(Forced)" : ""));

				var bResult = bundleLogic.CreateBundle(Today0h); Program.log("Bundle créé : " + Today0h);

				ServiceProcess service = new ServiceProcess(); Program.log("Retrieving users since " + BundleLogic.GetPreviousDayOrSo0h().ToString("dd/MM/yyyy"));
				List<User> listNewUserDay = service.RetrieveNewUsersSince(BundleLogic.GetPreviousDayOrSo0h()); Program.log("User List : " + listNewUserDay.Count);
				bundleLogic.SetBundleTotalSubs(Today0h, listNewUserDay.Count);
				UserDataService uDal = new UserDataService();
				string csvInPath = ConfigurationManager.AppSettings["localCsvFilesDirectory"];
				string localfilePath = uDal.CreateCsvContentForCanal(csvInPath, listNewUserDay, 1);
				if (File.Exists(localfilePath))
				{
					Program.log("File CSVIN créé : " + localfilePath);

					var bfResult = bundleLogic.AttachFileToBundle(Today0h, localfilePath, BundleFileType.CsvIn);
					if (bfResult.Result)
						bundleLogic.SetBundleStatus(Today0h, BundleStatus.CsvInCreated);

					if (ConfigurationManager.AppSettings["sendCsvToCanal"] == "true")
					{
						var ftpResult = service.CanalPushFileFTP(localfilePath);
						if (ftpResult.Result)
							bundleLogic.SetBundleStatus(Today0h, BundleStatus.CsvInSentToCanal);
					}
				}

				if (!_canTriggerSeveralTimesADay)
				{
					LastBundleModifiedDate = Today0h;
				}
			}
			else if (!_canTriggerSeveralTimesADay)
			{
				LastBundleModifiedDate = Today0h;
			}
			timer.Start();
		}
	}
}

