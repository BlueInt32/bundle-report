using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace Tools
{
	public class EventViewerManager
	{
		public static void Log(object value)
		{
			string sSource;
			string sLog;
			string sEvent;

			sSource = "DailymotionVideoService";
			sLog = "Application";
			sEvent = "Log Event: " + value;
			if (!EventLog.SourceExists(sSource))
				EventLog.CreateEventSource(sSource, sLog);
			EventLog.WriteEntry(sSource, sEvent);
			//EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 1);
		}
	}
}
