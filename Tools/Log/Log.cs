using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using log4net;
using Tools.CustomConfigSections;

namespace Tools
{
	public static class Log
	{
		public static LogLevel ConfLevel { get { return LogConfiguration.Values.LogLevelEnum; } }
		public static List<LogMethod> LogMethods { get { return LogConfiguration.Values.LogMethodsList; } }

		public static void Debug(string source, string text)
		{
			switch (ConfLevel)
			{
				case LogLevel.Debug:
				EffectiveLog(LogLevel.Debug, source, text);
				break;
				case LogLevel.Info:
				// no log info if conflevel is Info or more
				case LogLevel.Warn:
				case LogLevel.Error:
				case LogLevel.Fatal:
				break;
				default:
				throw new ArgumentOutOfRangeException();
			}
		}
		public static void Info(string source, string text)
		{
			switch (ConfLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Info:
				EffectiveLog(LogLevel.Info, source, text);
				break;
				case LogLevel.Warn:
				// no log info if conflevel is Warn or more
				case LogLevel.Error:
				case LogLevel.Fatal:
				return;
				default:
				throw new ArgumentOutOfRangeException();
			}
		}
		public static void Warn(string source, string text)
		{
			switch (ConfLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Info:
				case LogLevel.Warn:
				EffectiveLog(LogLevel.Warn, source, text);
				break;
				case LogLevel.Error:
				// no log info if conflevel is error or more
				case LogLevel.Fatal:
				return;
				default:
				throw new ArgumentOutOfRangeException();
			}
		}
		public static void Error(string source, string text)
		{
			switch (ConfLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Info:
				case LogLevel.Warn:
				case LogLevel.Error:
				EffectiveLog(LogLevel.Error, source, text);
				break;
				case LogLevel.Fatal:
				// no log info if conflevel is Fatal
				return;
				default:
				throw new ArgumentOutOfRangeException();
			}
		}
		public static void Fatal(string source, string text)
		{
			switch (ConfLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Info:
				case LogLevel.Warn:
				case LogLevel.Error:
				case LogLevel.Fatal:
				EffectiveLog(LogLevel.Fatal, source, text);
				break;
				default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static void DebugFormat(string source, string formatText, params Object[] args)
		{
			switch (ConfLevel)
			{
				case LogLevel.Debug:
				EffectiveLog(LogLevel.Debug, source, string.Format(formatText, args));
				break;
				case LogLevel.Info:
				// no log info if conflevel is Info or more
				case LogLevel.Warn:
				case LogLevel.Error:
				case LogLevel.Fatal:
				break;
				default:
				throw new ArgumentOutOfRangeException();
			}
		}
		public static void InfoFormat(string source, string formatText, params Object[] args)
		{
			switch (ConfLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Info:
				EffectiveLog(LogLevel.Info, source, string.Format(formatText, args));
				break;
				case LogLevel.Warn:
				// no log info if conflevel is Warn or more
				case LogLevel.Error:
				case LogLevel.Fatal:
				return;
				default:
				throw new ArgumentOutOfRangeException();
			}
		}
		public static void WarnFormat(string source, string formatText, params Object[] args)
		{
			switch (ConfLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Info:
				case LogLevel.Warn:
				EffectiveLog(LogLevel.Warn, source, string.Format(formatText, args));
				break;
				case LogLevel.Error:
				// no log info if conflevel is error or more
				case LogLevel.Fatal:
				return;
				default:
				throw new ArgumentOutOfRangeException();
			}
		}
		public static void ErrorFormat(string source, string formatText, params Object[] args)
		{
			switch (ConfLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Info:
				case LogLevel.Warn:
				case LogLevel.Error:
				EffectiveLog(LogLevel.Error, source, string.Format(formatText, args));
				break;
				case LogLevel.Fatal:
				// no log info if conflevel is Fatal
				return;
				default:
				throw new ArgumentOutOfRangeException();
			}
		}
		public static void FatalFormat(string source, string formatText, params Object[] args)
		{
			switch (ConfLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Info:
				case LogLevel.Warn:
				case LogLevel.Error:
				case LogLevel.Fatal:
				EffectiveLog(LogLevel.Fatal, source, string.Format(formatText, args));
				break;
				default:
				throw new ArgumentOutOfRangeException();
			}
		}

		#region Privates

		private static void EffectiveLog(LogLevel level, string source, string text)
		{
			string processedText = text.Replace(Environment.NewLine, " ");
			foreach (LogMethod logMethod in LogMethods)
			{
				switch (logMethod)
				{
					case LogMethod.DebugWriteLine:
					System.Diagnostics.Debug.WriteLine(string.Format(" -- {0} -- {1}", DateTime.UtcNow, processedText));
					break;
					case LogMethod.FileAppend:
					FileHelper.TextAppendToFile("Tools.Log", ConfigurationManager.AppSettings["logFilePath"], string.Format("{0} - {1}\n\n", DateTime.Now, processedText));
					return;
					case LogMethod.EventViewer:
					try
					{
						string sSource = source;
						string sLog = "Application";
						string sEvent = string.Format("Log Event: {0} - {1}", source, processedText);
						if (!EventLog.SourceExists(sSource))
							EventLog.CreateEventSource(sSource, sLog);
						EventLog.WriteEntry(sSource, sEvent);
					}
					catch
					{
					}
					break;
					case LogMethod.Log4Net:
					ILog log = LogManager.GetLogger(source);
					switch (level)
					{
						case LogLevel.Debug:
						log.Debug(processedText);
						break;
						case LogLevel.Info:
						log.Info(processedText);
						break;
						case LogLevel.Warn:
						log.Warn(processedText);
						break;
						case LogLevel.Error:
						log.Error(processedText);
						break;
						case LogLevel.Fatal:
						log.Fatal(processedText);
						break;
						default:
						throw new ArgumentOutOfRangeException();
					}
					break;
					default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}
		#endregion
	}
}
