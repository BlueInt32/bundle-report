using System.Collections.Generic;
using System.Xml.XPath;

namespace Tools
{
	public class XPathHelper
	{

		public string FilePath {get;set;}

		public string GetSinglePageInfo(string idPage, string field)
		{
			XPathDocument doc = new XPathDocument(FilePath);
			XPathNavigator XmlNavigator = doc.CreateNavigator();

			XPathExpression expr = XmlNavigator.Compile(string.Format("//page[@id='{0}']/{1}", idPage, field));

			XPathNodeIterator iterator = XmlNavigator.Select(expr);
			iterator.MoveNext();
			string value = iterator.Current.Value;
			return value;
		}

		/// <summary>
		/// Remplit un dictionnaire avec les datas d'une page dont idPage est renseigné
		/// </summary>
		/// <param name="idPage"></param>
		/// <param name="keys">Liste des clefs désirées</param>
		/// <returns></returns>
		public Dictionary<string, string> GetPageData(string idPage, List<string> keys)
		{
			Dictionary<string, string> result = new Dictionary<string, string>();
			XPathDocument doc = new XPathDocument(FilePath);
			
			XPathNavigator XmlNavigator = doc.CreateNavigator();

			foreach (string key in keys)
			{
				XPathExpression expr = XmlNavigator.Compile(string.Format("//page[@id='{0}']/{1}", idPage, key));

				XPathNodeIterator iterator = XmlNavigator.Select(expr);
				iterator.MoveNext();
				result[key] = iterator.Count > 0? iterator.Current.Value: string.Empty;
			}
			return result;
		}
	}
}
