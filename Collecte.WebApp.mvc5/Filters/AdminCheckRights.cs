using System.Web.Mvc;

namespace Collecte.WebApp.Filters
{
	public class AdminCheckRights : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.HttpContext.Session["logged"] == null)
				filterContext.Result = new RedirectResult("~/Admin");
			base.OnActionExecuting(filterContext);
		}
	}
}