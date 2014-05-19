#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

#endregion

namespace Tools.Route
{
	public class RouteHelper
	{
		public static string SerializeRouteDictionary(RouteValueDictionary route)
		{
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<string, object> keyValuePair in route)
			{
				sb.AppendFormat("{0}={1}", keyValuePair.Key, keyValuePair.Value);
				sb.Append("&");
			}
			string result = sb.ToString();
			return result.Substring(0, result.Length - 1);
		}

		public static RouteValueDictionary UnSerializeRouteDictionary(string input)
		{
			RouteValueDictionary rvd = new RouteValueDictionary();
			List<string> StrKeyValuePairs = input.Split('&').ToList();
			foreach (string keyValuePair in StrKeyValuePairs)
			{
				rvd.Add(keyValuePair.Split('=')[0], keyValuePair.Split('=')[1]);
			}
			return rvd;
		}
	}
}