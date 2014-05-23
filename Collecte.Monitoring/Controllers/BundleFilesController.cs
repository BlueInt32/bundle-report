﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Collecte.Monitoring.App_Code;

namespace CollecteBundles.Controllers
{
	public class BundleFilesController : ApiController
	{
		public string Get(string path)
		{
			string fileContent = "";
			
			if(path.EndsWith(".csv"))
			{
				fileContent = path.ReadFile().PrepareCsv();
			}
			else if(path.EndsWith(".xml"))
			{
				fileContent = path.ReadFile().PrepareXml();
			}
			return fileContent;
		}
	}
}