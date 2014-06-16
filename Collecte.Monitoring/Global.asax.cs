using Collecte.Monitoring.Filters;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Collecte.Monitoring
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
			GlobalConfiguration.Configuration.Filters.Add(new MonitoringAuthenticationFilter());
        }
		static public string AssemblyDirectory
		{
			get
			{
				string codeBase = Path.Combine(Assembly.GetExecutingAssembly().CodeBase, "../../../Collecte.DAL");
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return path;
			}
		}
    }
}
