using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace Safe_Web.Models
{
	public class BaseViewModel
	{
		public TaggageViewModel TaggageViewModel { get; set; }

		/** In some pages, we don't want any omniture tagging going on */
		public bool HideOmniture { get; set; }

		public string GoogleAnalyticsCode
		{
			get
			{
				return ConfigurationManager.AppSettings["GoogleAnalyticsCode"];
			}

		}


		public int ActiveMenuIndexZeroBased { get; set; }
		public string ProcessCssClass(int index)
		{
			if (index == ActiveMenuIndexZeroBased)
			{
				return "focus_nav";
			}
			return null;
		}

		public string ServerRootUrl { get { return ConfigurationManager.AppSettings["ServerAppRoot"]; } }


		public BaseViewModel()
		{
			ActiveMenuIndexZeroBased = -1;
		}
	}
}
