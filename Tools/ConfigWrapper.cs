using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Tools.Utils
{
	public class WebConfig : DynamicObject
	{
		private static volatile WebConfig instance;
		private static object syncRoot = new Object();
		private NameValueCollection _items;

		private WebConfig()
		{
			_items = ConfigurationManager.AppSettings;
		}
		public static dynamic Get
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						instance = new WebConfig();
					}
				}
				return instance;
			}
		}


		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = _items[binder.Name];
			return result != null;
		}
	}
}
