using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Collecte.Monitoring.App_Code
{
	public static class FilePreprocess
	{
		public static string ReadFile(this string relativePath)
		{
			using (StreamReader streamReader = new StreamReader(HostingEnvironment.MapPath("~/" + relativePath)))
			{
				string text = HttpUtility.HtmlEncode(streamReader.ReadToEnd());
				streamReader.Close();
				return text;
			}
		}

		public static string PrepareCsv(this string csvContent)
		{
			return HttpUtility.HtmlEncode(csvContent.Replace(Environment.NewLine, "<br/>").Replace("\n", "<br/>"));
		}
		public static string PrepareXml(this string xmlContent)
		{
			return xmlContent;
		}
	}
}