using System.Web.Mvc;
using Collecte.WebApp.Filters;

namespace Collecte.WebApp.App_Start
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new LogExceptionFilter());
			filters.Add(new HandleErrorAttribute());
			filters.Add(new MaintenanceFilter());
		}
	}
}