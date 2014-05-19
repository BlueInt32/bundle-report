using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using log4net;
using System.Diagnostics;

namespace Tools.Extensions
{
	public class WatchMe : ActionFilterAttribute
	{
		public string LogInfo { get; set; }
		private Stopwatch StopwatchMe { get; set; }

		public WatchMe()
		{
			StopwatchMe = new Stopwatch();
		}

		public WatchMe(string logId = "")
		{
			LogInfo = logId;
			StopwatchMe = new Stopwatch();
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			StopwatchMe.Restart();
			base.OnActionExecuting(filterContext);
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			Log.Debug("WatchMe", string.Format("{1}/{2} {0} took {3}ms", string.IsNullOrWhiteSpace(LogInfo) ? string.Empty : string.Format("[{0}]", LogInfo), filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName, StopwatchMe.ElapsedMilliseconds));
			StopwatchMe.Stop();
			base.OnActionExecuted(filterContext);
		}
	}
}
