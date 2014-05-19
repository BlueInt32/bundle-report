#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

#endregion

namespace Tools.Extensions
{
	public  static class PagerExtension
	{
		public static MvcHtmlString Pager(this HtmlHelper helper, int currentPage, int nbItemsPerPage, int totalRecords,
										  string urlPrefix)
		{
			const int pagesAfficheesDansLepager = 5;
			const int nbPageBeforecentral = pagesAfficheesDansLepager / 2;
			int itemsPerPage = Convert.ToInt32(WebConfigurationManager.AppSettings["VideosItemsPerPage"]);
			StringBuilder sb1 = new StringBuilder();
			//int seed = currentPage % currentPageSize == 0 ? currentPage : currentPage - (currentPage % currentPageSize);
			int seed = currentPage - 1 - nbPageBeforecentral < 0 ? 1 : currentPage - nbPageBeforecentral;

			if (currentPage > 1)
				sb1.AppendFormat("<li class=\"ft-cbold {2}\"><a href=\"{0}{1}\"><</a></li>", urlPrefix, currentPage - 1,
								 currentPage == 1 ? " selected" : string.Empty);

			//if (currentPage - currentPageSize >= 0)
			//    sb1.AppendLine(String.Format("<a href=\"{0}/{1}\">...</a>", urlPrefix, (currentPage - currentPageSize) + 1));

			for (int i = seed; i <= ((totalRecords - 1) / itemsPerPage) + 1 && i < seed + pagesAfficheesDansLepager - 1; i++)
			{
				sb1.AppendFormat("<li class=\"ft-cbold {2}\"><a href=\"{0}{1}\">{1}</a></li>", urlPrefix, i,
								 currentPage == i ? " selected" : string.Empty);
			}

			//if (currentPage + currentPageSize <= (Math.Round((totalRecords / 10) + 0.5) - 1))
			//    sb1.AppendLine(String.Format("<a href=\"{0}/{1}\">...</a>", urlPrefix, (currentPage + currentPageSize) + 1));

			if (currentPage < ((totalRecords - 1) / itemsPerPage) + 1)
				sb1.AppendFormat("<li class=\"ft-cbold {2}\"><a href=\"{0}{1}\">></a></li>", urlPrefix, currentPage + 1,
								 currentPage == (totalRecords / itemsPerPage) + 1 ? " selected" : string.Empty);

			return new MvcHtmlString(sb1.ToString());
		}
	}
}