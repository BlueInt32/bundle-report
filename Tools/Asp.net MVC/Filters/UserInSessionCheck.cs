using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HttpServiceLibrary;

namespace Tools.Filters
{
	public class UserInSessionCheck : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (HttpSessionService.CurrentUser == null)
			{
				Log.Info(string.Format("UserInSessionCheck Filter ({0})", filterContext.RouteData.Values["action"]), "Session Perdue");
				filterContext.Result = new RedirectResult("http://www.leclubcanal.fr");
			}
			base.OnActionExecuting(filterContext);
		}
	}
}