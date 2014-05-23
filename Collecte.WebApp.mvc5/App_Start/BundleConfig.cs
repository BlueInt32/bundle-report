using System.Web.Optimization;

namespace Collecte.WebApp.App_Start
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(
				new ScriptBundle("~/bundles/jquery")
				.Include("~/Scripts/jquery-{version}.js")
				.Include("~/Scripts/jquery.numeric.js")
				.Include("~/Scripts/autoEmpty.js")
			);

			//bundles.Add(
			//    new ScriptBundle("~/bundles/jwplayer")
			//    .Include("~/Scripts/jwplayer/jwplayer.js")
			//);
			Bundle tagBundle = new Bundle("~/bundles/tags", new JsMinify());
			tagBundle.Include("~/Scripts/Tagging.js");
			bundles.Add(tagBundle);

			Bundle cssBundle = new Bundle("~/Content/cssbundle/");
			cssBundle.Include("~/Content/css/styles.css");
			bundles.Add(cssBundle);
		}
	}
}