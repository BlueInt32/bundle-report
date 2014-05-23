using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Collecte.WebApp.Filters
{
	public class MaintenanceFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (WebConfigurationManager.AppSettings["maintenance"] == "true" && !filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri.Contains("aintenance"))
			{
				filterContext.Result = new RedirectResult("~/Maintenance");
			}
			base.OnActionExecuting(filterContext);
		}
	}
}