using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
//using System.Data.Objects;
using System.Linq;
using System.Text;
using Collecte.DTO;
using System.IO;
using System.Configuration;
using Tools;
using System.Diagnostics;
using System.Reflection;

namespace Collecte.DAL
{
	public class CollecteContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public DbSet<AnswerChoice> AnswerChoices { get; set; }
		public DbSet<TradeOccurence> TradeDoublerIndex { get; set; }
		public DbSet<Bundle> Bundles { get; set; }
		public DbSet<BundleFile> BundleFiles { get; set; }
		public DbSet<InstantGagnant> InstantsGagnants { get; set; }

		public CollecteContext()
		{
			Database.SetInitializer<CollecteContext>(null);
			Configuration.ProxyCreationEnabled = false;
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//base.Configuration.ProxyCreationEnabled = false;
		}
	}
}

