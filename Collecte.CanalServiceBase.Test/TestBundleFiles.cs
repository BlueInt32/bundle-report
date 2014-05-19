using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Collecte.DTO;
using System.Collections.Generic;
using MSTestExtensions;
using System.IO;
using Collecte.Logic;

namespace Collecte.CanalServiceBase.Test
{
	[TestClass]
	public class TestBundleFiles : MSTestExtensionsTestFixture
	{
		BundleLogic BundleLogic;
		DateTime Date;
		Bundle TestBundle;

		[TestInitialize]
		public void Init()
		{
			BundleLogic = new BundleLogic();
			Date = new DateTime(2014, 05, 02);
			var creationResult = BundleLogic.CreateBundle(Date);
			TestBundle = creationResult.ReturnObject;

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

		[TestMethod,
		ExpectedExceptionMessage(typeof(CollecteException), "Le fichier indiqué n'existe pas.")
		]
		public void AttachInexistantFileToBundle()
		{
			BundleLogic.AttachFileToBundle(Date, "inexistantFile.txt", DTO.BundleFileType.CsvIn);
		}

		[TestMethod, 
		ExpectedExceptionMessage(typeof(CollecteException), "Le fichier n'a pas la bonne extension.")]
		public void AttachWrongAttachementFileToBundle()
		{
			BundleLogic.AttachFileToBundle(Date, "../../TestBundleFiles.cs", DTO.BundleFileType.CsvIn);
		}

		[TestMethod]
		public void AttachAndCountList()
		{

			//Assert.AreEqual(TestBundle.BundleFiles.Count, 0);
			Assert.IsNull(TestBundle.BundleFiles);
			BundleLogic.AttachFileToBundle(Date, "../../bidule.csv", DTO.BundleFileType.CsvIn);

			var bResult = BundleLogic.GetBundleByDate(Date);
			Assert.AreEqual(1, bResult.ReturnObject.BundleFiles.Count);

			BundleLogic.AttachFileToBundle(Date, "../../bidule2.xml", DTO.BundleFileType.XmlTrade);
			bResult = BundleLogic.GetBundleByDate(Date);
			Assert.AreEqual(2, bResult.ReturnObject.BundleFiles.Count);
			Assert.AreEqual(1, BundleLogic.ListBundles().Count);

		}
	}
}
