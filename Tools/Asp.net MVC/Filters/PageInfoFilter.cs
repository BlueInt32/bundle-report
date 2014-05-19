using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Xml.XPath;

namespace Tools
{
		public class PageInfoFilter : ActionFilterAttribute
		{
			private readonly string _identification = "home";

			public PageInfoFilter()
			{
			}
			public PageInfoFilter(string identification)
			{
				_identification = identification;
			}

			public override void OnActionExecuting(ActionExecutingContext filterContext)
			{
				XPathHelper xPathHelper = new XPathHelper{FilePath = HttpContext.Current.Server.MapPath("~/PageIndex/PageIndex.xml")};
				Dictionary<string, string> pageInfos = xPathHelper.GetPageData(_identification, new List<string> { "cssBodyClass", "PageMainTitle", "bgImagePath", "cssFileName", "FluidBodyAttribute", "AnalyticsTag" });
				filterContext.Controller.ViewBag.cssBodyClass = pageInfos["cssBodyClass"];
				filterContext.Controller.ViewBag.PageMainTitle = pageInfos["PageMainTitle"];
				filterContext.Controller.ViewBag.BgImagePath = pageInfos["bgImagePath"];
				filterContext.Controller.ViewBag.cssFileName = pageInfos["cssFileName"];
				filterContext.Controller.ViewBag.FluidBodyAttribute = new MvcHtmlString(pageInfos["FluidBodyAttribute"]);
				filterContext.Controller.ViewBag.AnalyticsTag = pageInfos["AnalyticsTag"];
				base.OnActionExecuting(filterContext);
			}

			/// <summary>
			/// Récupère une info dans le fichier Xml se trouvant à l'url du serveur Current "~/PageIndex/PageIndex.xml"
			/// </summary>
			/// <param name="idPage"></param>
			/// <param name="field"></param>
			/// <returns></returns>
			public string GetPageInfo(string idPage, string field)
			{
				XPathDocument doc = new XPathDocument(HttpContext.Current.Server.MapPath("~/PageIndex/PageIndex.xml"));
				XPathNavigator XmlNavigator = doc.CreateNavigator();

				XPathExpression expr = XmlNavigator.Compile(string.Format("//page[@id='{0}']/{1}", idPage, field));

				XPathNodeIterator iterator = XmlNavigator.Select(expr);
				iterator.MoveNext();
				string value = iterator.Current.Value;
				return value;
			}
		}
}
