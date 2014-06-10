﻿using Collecte.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace Collecte.DAL
{
	internal sealed class Configuration : DbMigrationsConfiguration<CollecteContext>
	{
		string baseFilesPath = System.Configuration.ConfigurationManager.AppSettings["BaseFilesPath"];
		DateTime Date = new DateTime(2014, 05, 02);

		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(CollecteContext context)
		{
			Date = new DateTime(2014, 05, 02);

			Bundle bundle = new Bundle { Date = Date, Status = BundleStatus.XmlSentToTrade, NbInscriptions = 1245, NbKo = 549, NbOk = 696, NbRetoursCanal = 1245 };

			context.Bundles.Add(bundle);

			BundleFile bf1 = CreateBundleFile(BundleFileType.CsvIn, "FC_RAP_IN_CIBLE_20140511_1.csv");
			BundleFile bf2 = CreateBundleFile(BundleFileType.CsvOut, "FC_RAP_OUT_CIBLE_20140511_0.csv");
			BundleFile bf3 = CreateBundleFile(BundleFileType.XmlTrade, "pending_1772736_1.xml");

			context.BundleFiles.Add(bf1);
			context.BundleFiles.Add(bf2);
			context.BundleFiles.Add(bf3);
			bundle.BundleFiles = new List<BundleFile>();
			bundle.BundleFiles.Add(bf1);
			bundle.BundleFiles.Add(bf2);
			bundle.BundleFiles.Add(bf3);

			context.SaveChanges();
		}
		private BundleFile CreateBundleFile(BundleFileType type, string fileName)
		{
			return new BundleFile { CreationDate = DateTime.Now, Type = type, FileName = fileName };
		}
	}
}