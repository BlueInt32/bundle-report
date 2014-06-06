using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

namespace Collecte.Monitoring.Filters
{
	public class MonitoringAuthenticationFilter : BasicAuthenticationFilter
	{
		protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
				return false;
			if (username != ConfigurationManager.AppSettings["login"] && password != ConfigurationManager.AppSettings["pass"])
				return false;
			return true;
		}
	}
}