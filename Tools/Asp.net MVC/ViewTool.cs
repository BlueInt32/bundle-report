using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Tools
{
	public class ViewTool
	{
		public static string RenderPartialViewToString(ControllerContext context, ViewResultBase partialViewResult)
		{
			using (var sw = new StringWriter())
			{
				if (string.IsNullOrEmpty(partialViewResult.ViewName))
				{
					partialViewResult.ViewName = context.RouteData.GetRequiredString("action");
				}
				ViewEngineResult result = null;
				if (partialViewResult.View == null)
				{
					result = partialViewResult.ViewEngineCollection.FindPartialView(context, partialViewResult.ViewName);
					if (result.View == null)
						throw new InvalidOperationException(
									   "Unable to find view. Searched in: " +
									   string.Join(",", result.SearchedLocations));
					partialViewResult.View = result.View;
				}

				var view = partialViewResult.View;
				var viewContext = new ViewContext(context, view, partialViewResult.ViewData,
												  partialViewResult.TempData, sw);
				view.Render(viewContext, sw);
				if (result != null)
				{
					result.ViewEngine.ReleaseView(context, view);
				}
				return sw.ToString();
			}
		}
	}
}
