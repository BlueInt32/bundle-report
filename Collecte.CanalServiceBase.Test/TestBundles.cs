using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Collecte.DTO;
using System.Collections.Generic;
using Collecte.Logic;

namespace Collecte.CanalServiceBase.Test
{
	[TestClass]
	public class TestBundles
	{
		BundleLogic BundleLogic;
		DateTime Date;

		[TestInitialize]
		public void Init()
		{
			BundleLogic = new BundleLogic();
			CleanUp();
			Date = new DateTime(2014, 05, 02);
		}

		[TestCleanup]
		public void CleanUp()
		{
			List<Bundle> list = BundleLogic.ListBundles();

			foreach (Bundle bundle in list)
			{
				BundleLogic.DeleteBundles();
			}
		}

		[TestMethod]
		[ExpectedException(typeof(CollecteException))]
		public void BadCreationDate()
		{
			BundleLogic.CreateBundle(DateTime.Now);
		}

		[TestMethod]
		public void ClassicCreation()
		{
			var createResult = BundleLogic.CreateBundle(Date);

			Assert.IsNotNull(createResult);
			Assert.IsNotNull(createResult.ReturnObject);
			Assert.IsTrue(createResult.Result);
			Assert.AreEqual(createResult.ReturnObject.Status, BundleStatus.NoFileCreated);
			Assert.AreEqual(createResult.ReturnObject.Date, Date); 
		}

		[TestMethod]
		[ExpectedException(typeof(CollecteException))]
		public void DoubleCreation()
		{

			var createResult = BundleLogic.CreateBundle(Date);
			var createResultDouble = BundleLogic.CreateBundle(Date);
		}

		[TestMethod]
		public void SetStatus()
		{
			var createResult = BundleLogic.CreateBundle(Date);
			var editResult = BundleLogic.SetBundleStatus(Date, BundleStatus.XmlCreated);

			Assert.AreEqual<BundleStatus>(editResult.ReturnObject.Status, BundleStatus.XmlCreated);
		}

		[TestMethod]
		public void SetTotalSubs()
		{
			var createResult = BundleLogic.CreateBundle(Date);
			var editResult = BundleLogic.SetBundleTotalSubs(Date, 1234);

			Assert.AreEqual<int>(editResult.ReturnObject.NbInscriptions.Value, 1234);
		}
		
		[TestMethod]
		public void SetSubs()
		{
			var createResult = BundleLogic.CreateBundle(Date);

			var editResult = BundleLogic.SetBundleNbReturnsCanal(Date, 25, 10, 15);
			//var editResult = BundleLogic.SetBundleSubsStatus(Date, 10, 15);

			Assert.AreEqual<int>(editResult.ReturnObject.NbRetoursCanal.Value, 25);
			Assert.AreEqual<int>(editResult.ReturnObject.NbOk.Value, 10);
			Assert.AreEqual<int>(editResult.ReturnObject.NbKo.Value, 15);

		}

		[TestMethod]
		public void TestCreationForService()
		{
			BundleLogic bundleLogic = new BundleLogic();
			DateTime mockNowDate = new DateTime(2014, 05, 02);
			int DailyExecutionHour = 0;
			int DailyExecutionMinute = 0;
			var bundleGet = bundleLogic.GetBundleByDate(mockNowDate.AddDays(-1));
			bool goCsvCreate = false;
			if (mockNowDate.Hour == DailyExecutionHour
				&& mockNowDate.Minute == DailyExecutionMinute
				&&
					(
						!bundleGet.Result
						||
						(
							bundleGet.Result
							&& bundleGet.ReturnObject.Status == BundleStatus.NoFileCreated
						)
					)
				)
				goCsvCreate = true;

			Assert.AreEqual<bool>(true, goCsvCreate);
		}
	}
}
