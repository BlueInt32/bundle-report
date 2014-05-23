using System.Web.Mvc;
using System.Web.Routing;
using Collecte.WebApp.App_Code;

namespace Collecte.WebApp.App_Start
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			//routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			//routes.MapRoute(
			//    name: "GenericError",
			//    url: "GenericError",
			//    defaults: new { controller = "Error", action = "GenericError", id = UrlParameter.Optional }
			//);w
			routes.MapRoute(
                "Maintenance",
				"Maintenance",
				new { controller = "Site", action = "Maintenance", id = UrlParameter.Optional }
			);
			routes.MapRoute(
                "404", 
                "Page404", 
                new { controller = "Error", action = "Page404", id = UrlParameter.Optional }
			);
			routes.MapRoute(
				"QuestionChoice", 
                "qc/{questionNum}/{urltoken}",
				new { controller = "Site", action = "QuestionChoice", questionNum = 1, urltoken = "" },
				new { urltoken = new QuestionRouteConstraint() }
			);

			//routes.MapRoute(
			//    name: "Episode4Choice",
			//    url: "Home/Episode4Choice/{urltoken}",
			//    defaults: new { controller = "Home", action = "Episode4Choice", urltoken = UrlParameter.Optional },
			//    constraints: new { urltoken = new ConnexionTypeRouteConstraint() }
			//);
			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
                new { controller = "Site", action = "Index", id = UrlParameter.Optional },
                new[] { "admin.Controllers" }
			);
		}
	}
}