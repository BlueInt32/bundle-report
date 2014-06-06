using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace Collecte.DAL
{
	class Program
	{
		static void Main(string[] args)
		{
			Log.Info("LoadConfigValues", "Loading global.config");
			var dataDirectory = ConfigurationManager.AppSettings["DataDirectory"];
			var absoluteDataDirectory = Path.GetFullPath(dataDirectory);
			AppDomain.CurrentDomain.SetData("DataDirectory", absoluteDataDirectory);
		}
	}
}