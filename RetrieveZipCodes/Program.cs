using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace RetrieveZipCodes
{
	class Program
	{
		static void Main(string[] args)
		{	
			StringBuilder sb = new StringBuilder();
			using (canalplus_collecteEntities1 model = new canalplus_collecteEntities1())
			{
				var jointure = model.extractEmptyZips.Join(model.insee, outer => outer.City.ToLower().Replace("'", " "), inner => inner.Commune.ToLower(), (i, o) => new { i.Email, i.Zip });
	
				foreach (var item in jointure)
				{
					sb.AppendLine(string.Format("INSERT INTO (Email, Zip) VALUES ({0}, {1});", item.Email, item.Zip));
				}
			}

			if (File.Exists("E:/DDB/CANAL/Collecte/Utils Scripts/zips.sql"))
				File.Delete("E:/DDB/CANAL/Collecte/Utils Scripts/zips.sql");
			File.WriteAllText("E:/DDB/CANAL/Collecte/Utils Scripts/zips.sql", sb.ToString()); 


		}
	}

}
