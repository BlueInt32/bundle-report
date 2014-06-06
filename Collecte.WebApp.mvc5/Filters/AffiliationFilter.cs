using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Collecte.WebApp.Filters
{
	public class AffiliationFilter : ActionFilterAttribute
	{
		readonly List<string> _utmAllowed = new List<string>
		{
			"participation+filleul",
			"canalsat",
			"facebook+recommander",
			"facebook+canalsat",
			"facebook+canalplus",
			"twitter+canalplus",
            "canalplus",
			"tradedoubler",
			"acxiom",
			"twitter+canalsat",
			"1000mercis+conquete",
			"1000mercis+infinity",
			"tdemail",
			"tdreward",
			"tddisplay",
			"tdemailincentive"
		};


		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if(_utmAllowed.Contains(HttpUtility.UrlEncode(filterContext.HttpContext.Request.QueryString["utm_campaign"])))
				filterContext.HttpContext.Session["utm_campaign"] = HttpUtility.UrlEncode(filterContext.HttpContext.Request.QueryString["utm_campaign"]);

			base.OnActionExecuting(filterContext);
		}
	}
}