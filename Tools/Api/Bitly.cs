#region Usings

using System;
using System.IO;
using System.Net;

#endregion

namespace Tools
{
	public class Bitly
	{
		public static OperationResult<string> ShortenUrl(string inputUrl)
		{
			try
			{
				inputUrl = inputUrl.Replace("localhost", "www.google.com");

				string url =
					string.Format(
						"http://api.bitly.com/v3/shorten?login=devrappfrance&apiKey=R_5237d1cb9413f949b27c3846f58f8384&longUrl={0}&format=txt",
						Uri.EscapeUriString(inputUrl));
				WebRequest request = WebRequest.Create(url);

				Stream stream = request.GetResponse().GetResponseStream();
				StreamReader Reader = new StreamReader(stream);

				string shortUrl = Reader.ReadLine();
				return OperationResult<string>.OkResultInstance(shortUrl);
			}
			catch (Exception e)
			{
				return OperationResult<string>.BadResult("BitLy Shorten exception for "+inputUrl+" : " + e.Message);
			}
		}
	}
}