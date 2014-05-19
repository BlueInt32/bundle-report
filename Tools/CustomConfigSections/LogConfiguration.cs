using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace Tools.CustomConfigSections
{
	/// <summary>
	/// How To Create Custom ConfigurationSections and ConfigurationLists
	/// http://www.4guysfromrolla.com/articles/032807-1.aspx
	/// </summary>


	public class LogConfiguration : ConfigurationSection
	{
		public static LogConfiguration Values
		{
			get
			{
				return ConfigurationManager.GetSection("LogConfiguration") as LogConfiguration;
				//return ConfigurationSettings.GetConfig("LogConfiguration") as LogConfiguration;
			}
		}

		[ConfigurationProperty("LogLevel", DefaultValue = "Error", IsRequired = false)]
		private string LogLevel
		{
			get
			{
				return this["LogLevel"] as string;
			}
		}
		[ConfigurationProperty("LogMethods", IsRequired = false, DefaultValue = "Log4Net")]
		private string LogMethods
		{
			get
			{
				return this["LogMethods"] as string;
			}
		}
		
		public LogLevel LogLevelEnum
		{
			get
			{
				return (LogLevel)Enum.Parse(typeof(LogLevel), LogLevel);
			}
		}
		public List<LogMethod> LogMethodsList
		{
			get
			{
				List<LogMethod> methods = LogMethods.SplitToList(',').ConvertAll(m => (LogMethod)Enum.Parse(typeof(LogMethod), m));
				
				return methods;
			}
		}
	}
}
