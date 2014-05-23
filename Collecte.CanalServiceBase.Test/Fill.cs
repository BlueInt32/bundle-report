using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Collecte.DTO;
using System.IO;
using Collecte.Logic;

namespace Collecte.CanalServiceBase.Test
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class Fill
	{
		BundleLogic BundleLogic;
		InstantGagnantLogic InstantGagnantLogic;

		DateTime Date;
		
		public Fill()
		{
			BundleLogic = new BundleLogic();
			InstantGagnantLogic = new InstantGagnantLogic();
			Date = new DateTime(2014, 05, 02);
			List<Bundle> list = BundleLogic.ListBundles();

			foreach (Bundle bundle in list)
			{
				BundleLogic.DeleteBundles();
			}
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		[TestMethod]
		public void FillBundles()
		{
			string baseFilesPath = System.Configuration.ConfigurationManager.AppSettings["BaseFilesPath"];
			BundleLogic.CreateBundle(Date);
			BundleLogic.SetBundleStatus(Date, BundleStatus.XmlSentToTrade);
			BundleLogic.AttachFileToBundle(Date, Path.Combine(baseFilesPath, "csvin", "FC_RAP_IN_CIBLE_20140511_1.csv"), BundleFileType.CsvIn);
			BundleLogic.AttachFileToBundle(Date, Path.Combine(baseFilesPath, "csvout", "FC_RAP_OUT_CIBLE_20140511_0.csv"), BundleFileType.CsvOut);
			BundleLogic.AttachFileToBundle(Date, Path.Combine(baseFilesPath, "xml", "pending_1772736_1.xml"), BundleFileType.XmlTrade);
			BundleLogic.SetBundleTotalSubs(Date, 1245);
			BundleLogic.SetBundleNbReturnsCanal(Date, 1245, 549, 696);


			BundleLogic.CreateBundle(Date.AddDays(1));
			BundleLogic.SetBundleStatus(Date.AddDays(1), BundleStatus.CsvInCreated);
			BundleLogic.AttachFileToBundle(Date.AddDays(1), Path.Combine(baseFilesPath, "csvin", "FC_RAP_IN_CIBLE_20140511_1.csv"), BundleFileType.CsvIn);
			
			BundleLogic.CreateBundle(Date.AddDays(4));
			BundleLogic.SetBundleStatus(Date.AddDays(4), BundleStatus.CsvOutReceived);
			BundleLogic.AttachFileToBundle(Date.AddDays(4), Path.Combine(baseFilesPath, "csvin", "FC_RAP_IN_CIBLE_20140511_1.csv"), BundleFileType.CsvIn);
			BundleLogic.AttachFileToBundle(Date.AddDays(4), Path.Combine(baseFilesPath, "csvout", "FC_RAP_OUT_CIBLE_20140511_0.csv"), BundleFileType.CsvOut);
			
			BundleLogic.CreateBundle(Date.AddDays(6));
			
			BundleLogic.SetBundleStatus(Date.AddDays(6), BundleStatus.NoFileCreated);
			
			BundleLogic.CreateBundle(Date.AddDays(12));
			BundleLogic.CreateBundle(Date.AddDays(20));
		}


		[TestMethod]
		public void FillInstantsGagnants()
		{
			InstantGagnantLogic.DeleteInstantsGagnant();
			DateTime Now = DateTime.Now;
			List<InstantGagnant> list = new List<InstantGagnant>
			{
				new InstantGagnant { FrontHtmlId = "blason-carnetcanal", Label= "second_winable", StartDateTime = new DateTime(Now.Year, Now.Month, Now.Day), Won = false},
				new InstantGagnant { FrontHtmlId = "blason-carnetcine", Label= "non_winable", StartDateTime = new DateTime(Now.Year, Now.Month, Now.Day).AddDays(1), Won = false},
				new InstantGagnant { FrontHtmlId = "blason-iphone", Label= "first_winable", StartDateTime = new DateTime(Now.Year, Now.Month, Now.Day).AddDays(-1), Won = false},
			};
			foreach (InstantGagnant ig in list)
			{
				InstantGagnantLogic.AddInstantGagnant(ig);
			}
		}

	}
}
