using System;
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
	[System.ComponentModel.DesignerCategory("Code")] // this tells visual studio to open the file in code mode directly (System.ComponentModel seems to be necessary for VS)
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
			bool hasTriggeredTodayYet = LastBundleModifiedDate == BundleLogic.GetYesterday();
			if (hasTriggeredTodayYet)
				return;

			BundleLogic bundleLogic = new BundleLogic();
			DateTime YesterdayMidnight = BundleLogic.GetYesterday();
			var bundleGet = bundleLogic.GetBundleByDate(YesterdayMidnight);

			// predicates
			bool yesterdayCsvNotYetCreated = !bundleGet.Result || (bundleGet.Result && bundleGet.ReturnObject.Status == BundleStatus.NoFileCreated);
			bool rightTime = (DateTime.Now.Hour == DailyExecutionHour && DateTime.Now.Minute == DailyExecutionMinute) || WebConfig.Get.forceRightTime == "true";
			bool featureCsvCreationActivated = WebConfig.Get.createCsv == "true";

			if (rightTime && yesterdayCsvNotYetCreated && featureCsvCreationActivated)
			{
				Program.log(string.Format("It's time ! ({0}h{1}) {2}", DateTime.Now.Hour, DateTime.Now.Minute, WebConfig.Get.forceRightTime == "true" ? "(Forced)" : ""));

				var bResult = bundleLogic.CreateBundle(YesterdayMidnight); Program.log("Bundle créé : " + YesterdayMidnight);

				ServiceProcess service = new ServiceProcess(); Program.log("Retrieving users of " + BundleLogic.GetYesterday().ToString("dd/MM/yyyy"));
				List<User> listNewUserDay = service.RetrieveNewUsersSince(YesterdayMidnight); Program.log("User List : " + listNewUserDay.Count);
				bundleLogic.SetBundleTotalSubs(YesterdayMidnight, listNewUserDay.Count);
				UserDal uDal = new UserDal();
				string csvInPath = ConfigurationManager.AppSettings["localCsvFilesDirectory"];
				string localfilePath = uDal.CreateCsvContentForCanal(csvInPath, listNewUserDay, 1);
				if (File.Exists(localfilePath))
				{
					Program.log("File CSVIN créé : " + localfilePath);

					var bfResult = bundleLogic.AttachFileToBundle(YesterdayMidnight, localfilePath, BundleFileType.CsvIn);
					if (bfResult.Result)
						bundleLogic.SetBundleStatus(YesterdayMidnight, BundleStatus.CsvInCreated);

					if (ConfigurationManager.AppSettings["sendCsvToCanal"] == "true")
					{
						var ftpResult = service.CanalPushFileFTP(localfilePath);
						if (ftpResult.Result)
							bundleLogic.SetBundleStatus(YesterdayMidnight, BundleStatus.CsvInSentToCanal);
					}
				}

				if (!_canTriggerSeveralTimesADay)
				{
					LastBundleModifiedDate = YesterdayMidnight;
				}
			}
			else if (!_canTriggerSeveralTimesADay)
			{
				LastBundleModifiedDate = YesterdayMidnight;
			}
			timer.Start();
		}
	}
}

