using Collecte.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollecteBundles.App_Code
{
	public static class BundleExtensions
	{
		public static string GetStatusText(this Bundle bundle)
		{
			switch (bundle.Status)
			{
				case BundleStatus.NoFileCreated:
				return "Aucun fichier créé";
				case BundleStatus.CsvInCreated:
				return "Csv IN créé";
				case BundleStatus.CsvInSentToCanal:
				return "CSV IN Envoyé chez Canal";
				case BundleStatus.CsvOutReceived:
				return "CSV OUT Reçu";
				case BundleStatus.CsvOutParsed:
				return "CSV OUT Scanné";
				case BundleStatus.XmlCreated:
				return "XML créé";
				case BundleStatus.XmlSentToTrade:
				return "XML envoyé";
				default:
				return "";
			}
		}
		public static string GetStatusClass(this Bundle bundle)
		{
			switch (bundle.Status)
			{
				case BundleStatus.NoFileCreated:
				return "badge-info";
				case BundleStatus.CsvInCreated:
				return "badge-warning";
				case BundleStatus.CsvInSentToCanal:
				return "badge-important";
				case BundleStatus.CsvOutReceived:
				return "badge-important";
				case BundleStatus.CsvOutParsed:
				return "badge-important";
				case BundleStatus.XmlCreated:
				return "badge-important";
				case BundleStatus.XmlSentToTrade:
				return "badge-success";
				default:
				return "badge-info";
			}
		}
	}
}