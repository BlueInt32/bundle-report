using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace Collecte.DAL
{
	internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
		}

		protected override void Seed(DataContext context)
		{
			//context.ShowTypes.AddOrUpdate(entity => entity.Id, new DTO.ShowType { Id = 1, UrlToken = "sport" });
			//context.ShowTypes.AddOrUpdate(entity => entity.Id, new DTO.ShowType { Id = 2,  UrlToken = "cinema" });
			//context.ShowTypes.AddOrUpdate(entity => entity.Id, new DTO.ShowType { Id = 3,UrlToken = "series" });
			//context.ShowTypes.AddOrUpdate(entity => entity.Id, new DTO.ShowType { Id = 4, UrlToken = "jeunesse" });

			//context.ConnexionTypes.AddOrUpdate(entity => entity.Id, new DTO.ConnexionType { Id = 1,  UrlToken = "tnt" });
			//context.ConnexionTypes.AddOrUpdate(entity => entity.Id, new DTO.ConnexionType { Id = 2, UrlToken = "adsl" });
			//context.ConnexionTypes.AddOrUpdate(entity => entity.Id, new DTO.ConnexionType { Id = 3,  UrlToken = "satellite" });
			//context.ConnexionTypes.AddOrUpdate(entity => entity.Id, new DTO.ConnexionType { Id = 4,  UrlToken = "cable" });



			context.SaveChanges();
		}
	}
}
