using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Collecte.DTO
{
	/// <summary>
	/// Les bundles servent à encapsuler un ensemble de metadonnées liées aux fichiers échangés entre les différentes entités
	/// du Service Base. Un bundle est censé contenir 3 bundleFiles référençant les fichiers suivants : 
	/// - le csv généré par le service (envoyé à Canal)
	/// - le csv reçu de la part de canal avec les status
	/// - le xml envoyé à TradeDoubler
	/// </summary>
	public class Bundle
	{
		public int BundleId { get; set; }

		//[Key]
		public DateTime Date { get; set; }

		public BundleStatus Status { get; set; }

		public int? NbInscriptions { get; set; }
		public int? NbRetoursCanal { get; set; }
		public int? NbOk { get; set; }
		public int? NbKo { get; set; }

		public virtual List<BundleFile> BundleFiles { get; set; }
	}
}
