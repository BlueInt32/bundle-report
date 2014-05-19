
using Collecte.DTO;
using Collecte.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace CollecteBundles.Controllers
{
    public class BundlesController : ApiController
    {
		BundleLogic BundleLogic = new BundleLogic();

		public IEnumerable<KeyValuePair<int, List<Bundle>>> GetAllBundles()
		{
			
			var result = BundleLogic.GroupBundles();
			var arrayEnumerable = from entry in result select entry;
			return arrayEnumerable;
		}
    }
}
