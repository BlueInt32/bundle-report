using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Tools;

namespace Tools.Filters
{
	public class LogExceptionFilter : IExceptionFilter
	{
		public void OnException(ExceptionContext filterContext)
		{
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<string, object> keyValuePair in filterContext.Controller.ControllerContext.RouteData.Values)
			{
				sb.Append(string.Format("{0}:{1}/", keyValuePair.Key, keyValuePair.Value));
			}

			Log.Error(sb.ToString(), filterContext.Exception.Message + " --- " + filterContext.Exception.StackTrace);
		}
	}
}
