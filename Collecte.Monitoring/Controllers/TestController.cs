using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Collecte.Monitoring.Controllers
{
    public class TestController : ApiController
    {


		public string Get()
		{
			throw new Exception("Erreur test");
			return "yo";
		}
    }
}
