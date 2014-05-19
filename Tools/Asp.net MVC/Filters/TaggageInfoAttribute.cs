using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpeningAct.WebApp.Models;

namespace Tools.Filters
{
	public class TaggageInfoAttribute : ActionFilterAttribute
	{
		TaggageViewModel TaggageViewModel { get; set; }
		public TaggageInfoAttribute(string pageName)
		{
			TaggageViewModel = new TaggageViewModel
			{
				PageName = pageName,
				ExceptionTagEvent = new ExceptionTagEvent { Var25 = string.Empty, OmnitureEventName = string.Empty },
				OmnitureCanalUrl = ConfigurationManager.AppSettings["OmnitureTagUrl"]
			};
		}

		public TaggageInfoAttribute(string pageName, string exceptionEventName, string var25)
		{
			TaggageViewModel = new TaggageViewModel 
			{ 
				PageName = pageName, 
				ExceptionTagEvent = new ExceptionTagEvent { Var25 = var25, OmnitureEventName = exceptionEventName },
				OmnitureCanalUrl = ConfigurationManager.AppSettings["OmnitureTagUrl"]
			};
		}

		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			if (filterContext.Controller.ViewData.Model != null)
				((BaseViewModel)filterContext.Controller.ViewData.Model).TaggageViewModel = TaggageViewModel;
			else
				filterContext.Controller.ViewData.Model = new BaseViewModel
				{
					TaggageViewModel = TaggageViewModel
				};
			base.OnResultExecuting(filterContext);
		}
	}
}