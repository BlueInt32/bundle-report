using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Tools.CustomConfigSections
{
	public class TagConfiguration : ConfigurationSection
	{
		public static TagConfiguration Values
		{
			get
			{
				return ConfigurationManager.GetSection("TagConfiguration") as TagConfiguration;
			}
		}

		[ConfigurationProperty("GoogleAnalyticsCode", DefaultValue = "", IsRequired = false)]
		public string GoogleAnalyticsCode
		{
			get
			{
				return this["GoogleAnalyticsCode"] as string;
			}
		}
		[ConfigurationProperty("OmnitureUrl", IsRequired = false, DefaultValue = "")]
		public string OmnitureUrl
		{
			get
			{
				return this["OmnitureUrl"] as string;
			}
		}
	}
}
