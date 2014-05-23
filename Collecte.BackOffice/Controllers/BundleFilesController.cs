using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using Tools;

namespace CollecteBundles.Controllers
{
    public class BundleFilesController : ApiController
    {
        // GET: api/BundleFiles
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/BundleFiles/5
		public string Get(string path)
        {
			// RmlsZXMlMkZjc3ZpbiUyRkZDX1JBUF9JTl9DSUJMRV8yMDE0MDUxMV8xLmNzdg==
			//string decodeB64 = path
			using (StreamReader streamReader = new StreamReader(HostingEnvironment.MapPath("~/" + path + ".csv")))
				{
					string text = streamReader.ReadToEnd().Replace(Environment.NewLine, "<br/>");
					streamReader.Close();
					return text;
				}
        }

        // POST: api/BundleFiles
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/BundleFiles/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/BundleFiles/5
        public void Delete(int id)
        {
        }
    }
}
