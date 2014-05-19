using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.SEO
{
	/// <summary>
	/// Manages basic operations for creating a sitemap.xml document.
	/// </summary>
	public class SitemapManager
	{
		private readonly string _sitemapBase;

		public SitemapManager(string sitemapBase)
		{
			_sitemapBase = sitemapBase;
			DynamicItems = new List<SitemapItem>();
		}

		/// <summary>
		/// Dynamic SiteMapItems to be given. These will be put right after static urls (already in sitemap.base.xml in final output)
		/// </summary>
		public List<SitemapItem> DynamicItems { get; private set; }

		/// <summary>
		/// Add an item to sitemap's Items collection. 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public OperationResult<SitemapManager> AddItem(SitemapItem item)
		{
			if (!item.Url.IsUrl())
				return OperationResult<SitemapManager>.BadResult(string.Format("Bad Format Url '{0}'", item.Url));
			DynamicItems.Add(item);
			return OperationResult<SitemapManager>.OkResultInstance(this);
		}

		/// <summary>
		/// Add an item to sitemap's Items collection. 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public OperationResult<SitemapManager> AddItem(string url, DateTime? LastModification)
		{
			return AddItem(new SitemapItem { Url = url, LastModification = LastModification });
		}


		/// <summary>
		/// Retrieves static file base sitemap.base.xml content, replaces its '#dynamicData#' tag with dynamic items url tags and returns the output file content.
		/// </summary>
		/// <returns></returns>
		public string Output()
		{
			string dynamicOutput = DynamicItems.Aggregate(string.Empty, (current, item) => string.Concat(current, item.Output()));
			string baseFileContent = FileHelper.ReadFile(_sitemapBase);
			string outputFileContent = baseFileContent.Replace("#dynamicData#", dynamicOutput);
			return outputFileContent;
		}
	}
}
