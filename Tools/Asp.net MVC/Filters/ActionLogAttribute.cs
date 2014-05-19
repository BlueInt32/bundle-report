using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Tools;

namespace Safe.Filters
{
	public class ActionLogAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string key in filterContext.HttpContext.Request.Form.AllKeys)
			{
				sb.AppendFormat("{0}='{1}' ", key, filterContext.HttpContext.Request.Params[key]);
			}
			foreach (string key in filterContext.HttpContext.Request.QueryString.AllKeys.Where(key => key != "controller" && key != "action"))
			{
				sb.AppendFormat("{0}='{1}' ", key, filterContext.HttpContext.Request.Params[key]);
			}
			Log.InfoFormat(string.Format("{0}Controller", filterContext.RouteData.Values["controller"]), "Action='{0}' {1}", filterContext.RouteData.Values["action"], sb.ToString());
			base.OnActionExecuting(filterContext);
		}
	}
}