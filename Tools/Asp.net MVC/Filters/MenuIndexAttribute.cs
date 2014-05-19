using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpeningAct.WebApp.Models;

namespace Tools.Filters
{
	/// <summary>
	/// Use in conjonction with BaseViewModel's ActiveMenuIndexZeroBased property 
	/// and function ProcessCssClass(int) function to retrieve easily a active/nonactive menu css class.
	/// </summary>
	public class MenuIndexAttribute : ActionFilterAttribute
	{
		public int MenuIndex { get; set; }

		public MenuIndexAttribute(int menuIndex)
		{
			MenuIndex = menuIndex;
		}

		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			if (filterContext.Controller.ViewData.Model != null)
				((BaseViewModel)filterContext.Controller.ViewData.Model).ActiveMenuIndexZeroBased = MenuIndex;
			else
				filterContext.Controller.ViewData.Model = new BaseViewModel
				{
					ActiveMenuIndexZeroBased = MenuIndex
				};
			base.OnResultExecuting(filterContext);
		}
	}
}