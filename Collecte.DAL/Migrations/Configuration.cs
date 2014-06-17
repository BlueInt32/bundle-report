namespace Collecte.DAL.Migrations
{
	using Collecte.DTO;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<Collecte.DAL.CollectContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
		}

		protected override void Seed(CollectContext context)
		{

			Bundle bundle = new Bundle { Date = new DateTime(2014, 05, 02), Status = BundleStatus.XmlSentToTrade, NbInscriptions = 1245, NbKo = 549, NbOk = 696, NbRetoursCanal = 1245 };
			Bundle bundle2 = new Bundle { Date = new DateTime(2014, 05, 02), Status = BundleStatus.CsvInSentToCanal, NbInscriptions = 3211 };
			context.Bundles.AddRange(new List<Bundle> { bundle, bundle2});

			BundleFile bf1 = CreateBundleFile(BundleFileType.CsvIn, "FC_RAP_IN_CIBLE_20140511_1.csv");
			BundleFile bf2 = CreateBundleFile(BundleFileType.CsvOut, "FC_RAP_OUT_CIBLE_20140511_0.csv");
			BundleFile bf3 = CreateBundleFile(BundleFileType.XmlTrade, "pending_1772736_1.xml");

			BundleFile bf4 = CreateBundleFile(BundleFileType.CsvIn, "FC_RAP_IN_CIBLE_20140511_1.csv");
			BundleFile bf5 = CreateBundleFile(BundleFileType.CsvOut, "FC_RAP_OUT_CIBLE_20140511_0.csv");


			bundle.BundleFiles = new List<BundleFile>();
			bundle.BundleFiles.Add(bf1);
			bundle.BundleFiles.Add(bf2);
			bundle.BundleFiles.Add(bf3);
			bundle2.BundleFiles = new List<BundleFile>();
			bundle2.BundleFiles.Add(bf4);
			bundle2.BundleFiles.Add(bf5);

			context.SaveChanges();
		}
		private BundleFile CreateBundleFile(BundleFileType type, string fileName)
		{
			return new BundleFile { CreationDate = DateTime.Now, Type = type, FileName = fileName };
		}
	}
}
