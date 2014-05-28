using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Collecte.WebApp.Models
{
	public class OmnitureViewModel
	{
		public OmnitureViewModel(string rawPageName)
		{
			RawPageName = rawPageName;
		}
		public string OpeName { get { return "Grand Jeu Les Heros"; } }
		public string Channel { get { return "MD Collecte"; } }
		public string RawPageName { get; set; }
		public string PageName { get { return string.Concat(OpeName, " - ", RawPageName); } }


		public string Prop4 { get { return OpeName; } }
		public string Prop5 { get { return string.Concat(OpeName, " - ", RawPageName); } }
	}
}