using System.Web.Mvc;
using Tools;

namespace Collecte.WebApp.Filters
{
	public class CheckSession : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.HttpContext.Session["UserId"] == null)
			{
				Log.Warn("CheckSessionFilter", "Session lost -> redirect to Home");
				filterContext.Result = new RedirectResult("~/");
			}
			base.OnActionExecuting(filterContext);
		}
	}
}