using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace Collecte.Monitoring.App_Code
{
	public static class FilePreprocess
	{
		public static string ReadFile(this string relativePath)
		{
			using (StreamReader streamReader = new StreamReader(HostingEnvironment.MapPath("~/" + relativePath), Encoding.GetEncoding(28591)))
			{
				string text = streamReader.ReadToEnd();
				streamReader.Close();
				return text;
			}
		}

		public static string PrepareCsv(this string csvContent)
		{
			return csvContent.Replace(Environment.NewLine, "<br/>").Replace("\n", "<br/>");
		}
		public static string PrepareXml(this string xmlContent)
		{
			//return HttpUtility.HtmlEncode(xmlContent).Replace(Environment.NewLine, "<br/>").Replace("\n", "<br/>"));
			return HttpUtility.HtmlEncode(xmlContent).Replace("&gt;", "&gt;<br/>").Replace("\r\n", " "); ;
		}
	}
}