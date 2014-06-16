using Collecte.DTO;
using Collecte.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace CollecteBundles.Controllers
{
    public class BundlesController : ApiController
    {
		public IEnumerable<KeyValuePair<int, List<Bundle>>> GetAllBundles()
		{
			var result = (new BundleLogic()).GroupBundlesByWeeks();
			return result;
		}
    }
}
