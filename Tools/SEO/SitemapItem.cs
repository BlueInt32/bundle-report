using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.SEO
{
	/// <summary>
	/// Represents an url item in a sitemap.xml document. 
	/// </summary>
	public class SitemapItem
	{
		public string Url { get; set; }
		public DateTime? LastModification { get; set; }

		/// <summary>
		/// Supposed to output something like 
		/// <url>\n<loc>http://www.example.com/</loc>\n<lastmod>2005-01-01</lastmod>\n</url>
		/// </summary>
		/// <returns></returns>
		public string Output()
		{
			string lastModificationTag = LastModification.HasValue ? string.Format("<lastmod>{0}</lastmod>", LastModification.Value.ToShortDateString()) : string.Empty;
			return string.Format("<url>\n<loc>{0}</loc>\n{1}\n</url>", Url, lastModificationTag);

		}

	}
}
