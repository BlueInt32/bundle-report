using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Muse.Models;
using Tools;
using ZaGameApi;

namespace Muse.Filters
{
	public class PageInfoAttribute : ActionFilterAttribute
	{
		readonly Dictionary<PageInfoType, string> _pageInfos;
		public PageInfoAttribute(PageTypeId pageTypeId)
		{
			_pageInfos = LocalPageInfo.GetPageInfos(pageTypeId);
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			BaseModel model = new BaseModel();
			model.HtmlTitle = _pageInfos[PageInfoType.HtmlTitle];
			model.HtmlMetaDescription = _pageInfos[PageInfoType.HtmlMetaDescription];
			model.TagModel = new TagModel { PageName = _pageInfos[PageInfoType.TagModel]};
			model.CssContainerId = _pageInfos[PageInfoType.CssContainerId];
			model.CssContainerClass = _pageInfos[PageInfoType.CssContainerClass];
			filterContext.Controller.ViewBag.OmnitureUrl = WebConfigurationManager.AppSettings["OmnitureSrc"];
			filterContext.Controller.ViewData.Model = model;

			// Default value for special omniture tags
			filterContext.Controller.ViewBag.OmnitureEvent = "";
			filterContext.Controller.ViewBag.OmnitureVar25 = "";

			base.OnActionExecuting(filterContext);
		}


	}
}