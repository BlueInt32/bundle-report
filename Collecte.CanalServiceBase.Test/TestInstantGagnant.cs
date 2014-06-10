using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Collecte.Logic;
using Collecte.DTO;
using System.Collections.Generic;
using Collecte.DAL;

namespace Collecte.CanalServiceBase.Test
{
	[TestClass]
	public class TestInstantGagnant
	{

		InstantGagnantLogic IgLogic;
		DateTime Now;
		User MainUser;


		[TestInitialize]
		public void Init()
		{
			Now = DateTime.Now;
			IgLogic = new InstantGagnantLogic();
			CleanUp();
			UserDataService uDal = new UserDataService();
			MainUser = uDal.GetUserByEmail("simon.budin@gmail.com");

			List<InstantGagnant> list = new List<InstantGagnant>
			{
				new InstantGagnant { FrontHtmlId = "blason-carnetcanal", Label= "second_winable", StartDateTime = new DateTime(Now.Year, Now.Month, Now.Day), Won = false},
				new InstantGagnant { FrontHtmlId = "blason-carnetcine", Label= "non_winable", StartDateTime = new DateTime(Now.Year, Now.Month, Now.Day).AddDays(1), Won = false},
				new InstantGagnant { FrontHtmlId = "blason-iphone", Label= "first_winable", StartDateTime = new DateTime(Now.Year, Now.Month, Now.Day).AddDays(-1), Won = false},
			};
			foreach (InstantGagnant ig in list)
			{
				IgLogic.AddInstantGagnant(ig);
			}
		}

		[TestCleanup]
		public void CleanUp()
		{
			IgLogic.DeleteInstantsGagnant();
		}

		[TestMethod]
		public void TestIgAvailable()
		{
			var igResult = IgLogic.GetCurrentInstantGagnant();
			Assert.AreEqual(true, igResult.Result);
			Assert.AreEqual("first_winable", igResult.ReturnObject.Label);
		}

		[TestMethod]
		public void TestAttachToUser()
		{
			var attachResult = IgLogic.PlayInstantGagnant(MainUser);
			Assert.IsNotNull(MainUser.InstantsGagnantWon);
			Assert.AreEqual(1, MainUser.InstantsGagnantWon.Count);
			Assert.AreEqual(true, attachResult.Result);
			Assert.AreEqual("first_winable", attachResult.ReturnObject.Label);
			Assert.AreEqual("first_winable", MainUser.InstantsGagnantWon[0].Label);
			Assert.IsTrue(Math.Abs(DateTime.Now.Subtract(attachResult.ReturnObject.WonDate.Value).Seconds) < 10);
		}

		[TestMethod]
		public void TestMultipleAttach()
		{

			var attachResult = IgLogic.PlayInstantGagnant(MainUser);
			var attachResult2 = IgLogic.PlayInstantGagnant(MainUser);
			Assert.IsNotNull(MainUser.InstantsGagnantWon);
			Assert.AreEqual(2, MainUser.InstantsGagnantWon.Count);
			Assert.AreEqual(true, attachResult2.Result);
			Assert.AreEqual("second_winable", attachResult2.ReturnObject.Label);
			Assert.AreEqual("second_winable", MainUser.InstantsGagnantWon[1].Label);

		}
	}
}
