#region Usings

using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

#endregion

namespace Tools
{
	public static class UrlHelperSb
	{
		public static string GetApplicationBaseUrl(bool endSlash)
		{
			string result = string.Format("{0}://{1}{2}", 
				HttpContext.Current.Request.Url.Scheme,
				HttpContext.Current.Request.Url.Authority,
				HttpContext.Current.Request.ApplicationPath);
			if (!result.EndsWith("/") && endSlash)
				result = string.Concat(result, "/");
			else if (result.EndsWith("/") && !endSlash)
				result = result.Substring(0, result.Length - 1);

			return result;
		}

		public static string ApplicationServerPath()
		{
			// il faudra peut-etre ajouter à la section de config system.serviceModel <serviceHostingEnvironment aspNetCompatibilityEnabled="true" />
			string directory = HostingEnvironment.ApplicationPhysicalPath;
			return directory;
		}

        public static string SetValidString(string text)
        {
            text = HttpUtility.HtmlDecode(text);
            return HttpContext.Current.Server.UrlEncode(RemoveDiacritics(text).Replace("'", "-").Replace(" / ", "-").Replace(' ', '-').Replace('\'', '-').Replace('/', '-').Replace('@', '-').Replace('&', '-').Replace('#', '-'));
        }

        public static string RemoveDiacritics(string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }


		public static string ToContentUrl(this string input)
		{
			return UrlHelper.GenerateContentUrl(input, new HttpContextWrapper(HttpContext.Current));
		}

		public static string ContentAbsolute(this string path)
		{
			if (path.StartsWith("~"))
			{
				path = path.Substring(1);
			}
			return string.Format("{0}{1}", GetApplicationBaseUrl(false), path);
		}
		public static string ContentAbsolute(this UrlHelper helper, string path)
		{
			if (path.StartsWith("~"))
			{
				path = path.Substring(1);
			}
			return string.Format("{0}{1}", GetApplicationBaseUrl(false), path);
		}
	}
}