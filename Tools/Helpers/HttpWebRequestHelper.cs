﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Tools
{
	public class HttpWebRequestHelper
	{
		public static OperationResult<string> MakeRequest(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
				return OperationResult<string>.BadResult("L'url ne peut pas etre vide");

			HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);

			wr.Timeout = 10000;

			WebResponse resp = wr.GetResponse();
			StreamReader streamReader = new StreamReader(resp.GetResponseStream());
			string response = streamReader.ReadToEnd();

			return OperationResult<string>.OkResultInstance(response);
		}
	}
}
