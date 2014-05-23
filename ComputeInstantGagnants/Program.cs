using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputeInstantGagnants
{
	public class Program
	{
		/// <summary>
		/// Mains the specified arguments.
		/// </summary>
		/// <param name="args">The arguments.</param>
		static void Main(string[] args)
		{
			StringBuilder sb = new StringBuilder();
			Random random = new Random();
			foreach (IGDescriptor descriptor in IGDescriptor.LotList)
			{
				sb.AppendLine(string.Format("-- Lots {0}", descriptor.LabelLot));

				// génération d'un tableau random des lots IG
				List<int> minutesRandom = new List<int>();
				int totalMinutes = IGDescriptor.NbTotalHours * 60;
				for (int i = 0; i < descriptor.NbLots; i++)
				{
					minutesRandom.Add(random.Next(1, totalMinutes - 1));
				}
				minutesRandom = minutesRandom.OrderBy(i => i).ToList();

				//int hoursBetweenLots = Convert.ToInt32(Math.Floor((double)IGDescriptor.NbTotalHours / (double)descriptor.NbLots));
				
				int hoursOffset = 0;
				
				for (int i = 0; i < descriptor.NbLots; i++)
				{
					string lineSql = "INSERT INTO dbo.InstantGagnants ([LotId], [StartDateTime],[Won],[Label],[FrontHtmlId],[UserId],[WonDate]) VALUES (";
					//hoursOffset += hoursBetweenLots;
					DateTime startDate = IGDescriptor.StartDate.AddMinutes(minutesRandom[i]);
					lineSql = string.Concat(lineSql, string.Format("{0},convert(datetime , '{1}' , 21),0 ,'{2}','{3}',NULL,NULL); -- {4}h{5} de plus", 
						descriptor.Id,
						startDate.ToString("yyyy-MM-dd HH:mm:ss:sss"), 
						descriptor.LabelLot,
						descriptor.IdDiv,
						minutesRandom[i] / 60,
						minutesRandom[i]%60));
					sb.AppendLine(lineSql);
				}

				sb.AppendLine("");
				sb.AppendLine("");

			}
			if (File.Exists("E:/DDB/CANAL/Collecte/Utils Scripts/insertIg.sql"))
				File.Delete("E:/DDB/CANAL/Collecte/Utils Scripts/insertIg.sql");
			File.WriteAllText("E:/DDB/CANAL/Collecte/Utils Scripts/insertIg.sql",sb.ToString()); 
		}
	}

	public class IGDescriptor
	{
		public int Id { get; set; }
		public string LabelLot { get; set; }
		public string IdDiv { get; set; }
		public int NbLots { get; set; }
		public static DateTime StartDate
		{
			get
			{
				return new DateTime(2014, 5, 27).AddHours(18);
				//return new DateTime(2014, 5, 01).AddHours(18);
			}
		}
		public static int NbTotalHours { get { return 1080; } }
		public static List<IGDescriptor> LotList
		{
			get
			{

				return new List<IGDescriptor>
				{
					new IGDescriptor { Id = 1, IdDiv = "blason-LGJ", LabelLot = "UN CAHIER LE GRAND JOURNAL", NbLots =50},
					new IGDescriptor { Id = 2, IdDiv = "blason-creationoriginale", LabelLot = "UN CAHIER CANAL+ SERIES", NbLots =50},
					new IGDescriptor { Id = 3, IdDiv = "blason-iphone", LabelLot = "UNE COQUE IPHONE VERSION CANAL+ POUR IPHONE 4", NbLots =50},
					new IGDescriptor { Id = 4, IdDiv = "blason-lpj4", LabelLot = "UNE COQUE IPHONE VERSION LE PETIT JOURNAL  POUR IPHONE 4", NbLots =25},
					new IGDescriptor { Id = 5, IdDiv = "blason-lpj5", LabelLot = "UNE COQUE IPHONE VERSION LE PETIT JOURNAL  POUR IPHONE 5", NbLots =25},
					new IGDescriptor { Id = 6, IdDiv = "blason-parapluieViolet", LabelLot = "UN PARAPLUIE PLIANT DE COULEUR VIOLET", NbLots =17},
					new IGDescriptor { Id = 7, IdDiv = "blason-parapluieOrange", LabelLot = "UN PARAPLUIE PLIANT DE COULEUR ORANGE", NbLots =16},
					new IGDescriptor { Id = 8, IdDiv = "blason-parapluieNoir", LabelLot = "UN PARAPLUIE PLIANT DE COULEUR  NOIR", NbLots =17},
					new IGDescriptor { Id = 9, IdDiv = "blason-muggroland", LabelLot = "UN MUG GROLAND", NbLots =25},
					new IGDescriptor { Id = 10, IdDiv = "blason-mugGJ", LabelLot = "UN MUG LE GRAND JOURNAL", NbLots =25},
					new IGDescriptor { Id = 11, IdDiv = "blason-carnetcine", LabelLot = "UN CARNET CINEMA", NbLots =12},
					new IGDescriptor { Id = 12, IdDiv = "blason-sport", LabelLot = "UN CARNET SPORT", NbLots =12},
					new IGDescriptor { Id = 13, IdDiv = "blason-carnetcanal", LabelLot = "UN CARNET CANAL+", NbLots =12},
					new IGDescriptor { Id = 14, IdDiv = "blason-carnetLPJ", LabelLot = "UN CARNET LE PETIT JOURNAL", NbLots =14}
				};
			}
		}
	}
}
