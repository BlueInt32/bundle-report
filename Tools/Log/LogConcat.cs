using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tools.LogTools
{
	public class LogLine
	{
		public DateTime DateTime { get; set; }
		public string Text { get; set; }
		public string Status { get; set; }
		public int GroupingIndex { get; set; }
	}

	public class LogLineComparer : IComparer<LogLine>
	{
		public int Compare(LogLine x, LogLine y)
		{
			return DateTime.Compare(x.DateTime, y.DateTime);
		}
	}

	public class LogConcat
	{
		public const string Log4NetBaseDatePattern = @"201[0-9]{1}-[0-1][0-9]-[0-3][0-9]\s[0-2][0-9]:[0-5][0-9]:[0-5][0-9],[0-9]{3}";
		public const string DatePatternFormat = "yyyy-MM-dd HH:mm:ss,fff";
		public const string UrlRegexPattern = @"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)";

		public int NbFilesAdded { get; set; }

		public LogConcat()
		{
			LogFilesContents = new List<string>();
			FilterPredicates = new List<Func<LogLine, bool>>();
			NbFilesAdded = 0;
		}


		public string LogLinePattern { get; set; }


		/// <summary>
		/// You add log file content to this list.
		/// </summary>
		private List<string> LogFilesContents { get; set; }

		/// <summary>
		/// List of predicates that are tested to filter log entries.
		/// </summary>
		public List<Func<LogLine, bool>> FilterPredicates { get; set; }

		/// <summary>
		/// This dictionary will contain the final concatenated logs from all sources (log contents in LogFilesContents), keys are dates.
		/// </summary>
		public List<LogLine> LogLinesFull { get; set; }


		/// <summary>
		/// This dictionary will contain the final concatenated logs from all sources (log contents in LogFilesContents), keys are dates.
		/// </summary>
		public List<LogLine> LogLinesProcessed { get; set; }

		/// <summary>
		/// Add a log file content to LogFilesContents. This also fills LogLines without any particular sorting.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public OperationResult<LogConcat> AddLogFileContent(string input)
		{
			LogFilesContents.Add(input);
			ProcessFile(input, NbFilesAdded);
			NbFilesAdded++;
			return OperationResult<LogConcat>.OkResult;
		}

		public OperationResult<LogConcat> ProcessFiltering()
		{
			if (FilterPredicates.Count == 0)
			{
				LogLinesProcessed = LogLinesFull;
				return OperationResult<LogConcat>.BadResult("No Filter exists");
			}
			if (LogLinesFull == null || LogLinesFull.Count == 0)
				return OperationResult<LogConcat>.OkResult;
			List<LogLine> ResultList = (from logLine in LogLinesFull
			                            let i = FilterPredicates.Count(predicate => predicate(logLine))
			                            where i == FilterPredicates.Count
			                            select logLine).ToList();
			LogLinesProcessed = ResultList;
			return OperationResult<LogConcat>.OkResult;
		}

		public string Output()
		{
			// Just Before Outputing, we have to order Lines by Date Desc
			LogLinesProcessed.Sort(new LogLineComparer());


			StringBuilder output = new StringBuilder();
			output.AppendLine("<ul>");
			foreach (LogLine logLine in LogLinesProcessed)
			{
				output.AppendLine(LineHtmlFormat(logLine));
			}
			output.AppendLine("</ul>");

			return output.ToString();
		}

		private void ProcessFile(string inputFileContent, int nbFilesAdded)
		{
			if (LogLinesFull == null)
				LogLinesFull = new List<LogLine>();

			Regex tagRegex = new Regex(string.Format(@"((?<date>{0})\s(?<status>[A-Z]+)\s(?<logline>.*))\n", Log4NetBaseDatePattern));
			MatchCollection matches = tagRegex.Matches(inputFileContent);
			foreach (Match match in matches)
			{
				string date = match.Groups["date"].ToString().ToLower();
				string status = match.Groups["status"].ToString();
				string logline = match.Groups["logline"].ToString();
				DateTime parsedDate = DateTime.ParseExact(date, DatePatternFormat, null); //2012-11-06 18:10:14,152
				LogLinesFull.Add(new LogLine { DateTime = parsedDate, Text = logline, GroupingIndex = nbFilesAdded, Status = status });
			}
		}

		

		private string LineHtmlFormat(LogLine logLine)
		{
			string dateHtml = string.Format("<span class='{1}'>{0}</span>", logLine.DateTime.ToString(DatePatternFormat), "date");
			string statusHtml = string.Format("<span class='status {1}'>{0}</span>", logLine.Status, logLine.Status.ToLower());
			
			string textHtml = Regex.Replace(logLine.Text, UrlRegexPattern, delegate(Match match)
			{
				string v = match.ToString();
				return string.Format("<a href='{0}' target='_blank'>{0}</a>", v);
			});
			textHtml = string.Format("<span class='{1}'>{0}</span>", textHtml, "log");

			string output = string.Format("<li class='g{3} searchable'>{0} {1} {2}</li>", dateHtml, statusHtml, textHtml, logLine.GroupingIndex);


			return output;
		}
	}
}
