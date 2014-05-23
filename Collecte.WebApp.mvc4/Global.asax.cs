using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Collecte.WebApp.App_Code;
using Tools;
using System.Web.Optimization;
using Collecte.WebApp.App_Start;
using Collecte.WebApp.mvc4;

namespace Collecte.WebApp
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			log4net.Config.XmlConfigurator.Configure();

			Log.InfoFormat("Global.asax", "Application_Start");
			AreaRegistration.RegisterAllAreas();

			//WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			InitQualifData.Go();
		}

		protected void Application_BeginRequest()
		{
			if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["redirectedUrl"]) && Request.Url.AbsoluteUri.Contains(ConfigurationManager.AppSettings["redirectedUrl"]))
			{
				Log.Info("Global.asax", "Redirection depuis l'url " + ConfigurationManager.AppSettings["redirectedUrl"]);
				Response.RedirectPermanent(ConfigurationManager.AppSettings["shareUrl"]);
			}
		}
		protected void Application_Error()
		{
			Exception exception = Server.GetLastError();
			if (exception != null)
			{
				Log.Error("HttpException", exception.Message);
			}
		}
	}
}