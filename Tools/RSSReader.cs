using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace Tools
{
	public class RssReader
	{
		public static List<RssArticle> GetArticlesFromFeed(string feedUrl)
		{
			return GetArticles(Read(feedUrl));
		}

		private static XmlDocument Read(string rssFeedUrl)
		{
			string xmlsrc = rssFeedUrl;

			if (xmlsrc == "") return null;
			// make remote request
			HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(xmlsrc);

			// set the HTTP properties
			wr.Timeout = 10000;
			// 10 seconds

			// read the response
			WebResponse resp = wr.GetResponse();
			Stream stream = resp.GetResponseStream();

			// load XML document
			XmlTextReader reader = new XmlTextReader(stream);
			reader.XmlResolver = null;
			XmlDocument doc = new XmlDocument();
			doc.Load(reader);

			return doc;
		}

		private static List<RssArticle> GetArticles(XmlDocument doc)
		{
			List<RssArticle> resultList = new List<RssArticle>();

			XmlNodeList items = doc.SelectNodes("//item");
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
			nsmgr.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");


			foreach (XmlNode item in items)
			{
				string title = item.SelectSingleNode("title").InnerText;
				string content = item.SelectSingleNode("content:encoded", nsmgr).InnerText;
				DateTime dateCreation  =DateTime.Parse(item.SelectSingleNode("pubDate").InnerText);

				RssArticle rssArticle = new RssArticle
				{
					Title = item.SelectSingleNode("title").InnerText,
					Content = item.SelectSingleNode("content:encoded", nsmgr).InnerText,
					DateCreation = DateTime.Parse(item.SelectSingleNode("pubDate").InnerText),
					Description = item.SelectSingleNode("description").InnerText,
					Url = item.SelectSingleNode("link").InnerText,
					ImageUrl = item.SelectSingleNode("enclosure ").Attributes["url"].Value
				};
				resultList.Add(rssArticle);
			}
			return resultList;
		}



		public class RssArticle
		{
			public string Title { get; set; }
			public string Content { get; set; }
			public DateTime DateCreation { get; set; }
			public string Description { get; set; }
			public string Url { get; set; }
			public string ImageUrl { get; set; }
		}
	}
}
