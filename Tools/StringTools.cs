using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Tools
{
	public static class StringTools
	{
		#region HtmlEncode Decode UTF8, ISO 8859-1, ASCII

		public static string HtmlDecode(this string input)
		{
			return HttpUtility.HtmlDecode(input);
		}
		public static string ConvertToUtf8(this string input)
		{
			byte[] byteOuput = Encoding.Convert(Encoding.Default, Encoding.UTF8,
												Encoding.Default.GetBytes(input));
			return Encoding.UTF8.GetString(byteOuput);
		}
		public static string ConvertToIso(this string input)
		{
			byte[] byteOuput = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(28591),
												Encoding.Default.GetBytes(input));
			return Encoding.GetEncoding(28591).GetString(byteOuput);
		}
		public static string ConvertToAscii(this string input)
		{
			Encoding inEncoding = Encoding.UTF8;
			Encoding outEncoding = Encoding.GetEncoding(437);
			//inEncoding.GetBytes(input);
			byte[] byteOuput = Encoding.Convert(inEncoding, outEncoding,
												inEncoding.GetBytes(input));
			return outEncoding.GetString(byteOuput);
		}

		public static string ReplaceSpecialCharacters(this string input)
		{
			if (!string.IsNullOrEmpty(input))
			{
				char[] oldChar = { 'À', 'Á', 'Â', 'Ã', 'Ä', 'Å', 'à', 'á', 'â', 'ã', 'ä', 'å', 'Ò', 'Ó', 'Ô', 'Õ', 'Ö', 'Ø', 'ò', 'ó', 'ô', 'õ', 'ö', 'ø', 'È', 'É', 'Ê', 'Ë', 'è', 'é', 'ê', 'ë', 'Ì', 'Í', 'Î', 'Ï', 'ì', 'í', 'î', 'ï', 'Ù', 'Ú', 'Û', 'Ü', 'ù', 'ú', 'û', 'ü', 'ÿ', 'Ñ', 'ñ', 'Ç', 'ç', '°' };
				char[] newChar = { 'A', 'A', 'A', 'A', 'A', 'A', 'a', 'a', 'a', 'a', 'a', 'a', 'O', 'O', 'O', 'O', 'O', 'O', 'o', 'o', 'o', 'o', 'o', 'o', 'E', 'E', 'E', 'E', 'e', 'e', 'e', 'e', 'I', 'I', 'I', 'I', 'i', 'i', 'i', 'i', 'U', 'U', 'U', 'U', 'u', 'u', 'u', 'u', 'y', 'N', 'n', 'C', 'c', ' ' };
				int i = 0;

				foreach (char monc in oldChar)
				{
					input = input.Replace(monc, newChar[i]);
					i++;
				}
			}
			input = input.Replace("'", string.Empty);
			input = input.Replace(" ", "");
			input = input.Replace("!", string.Empty);
			input = input.Replace("?", string.Empty);
			input = input.ToLower();
			return input;
		}

		static public string EncodeTo64(this string toEncode)
		{
			byte[] toEncodeAsBytes= Encoding.UTF8.GetBytes(toEncode);
			string returnValue = Convert.ToBase64String(toEncodeAsBytes);
			return returnValue;
		}

		static public string DecodeFrom64(this string encodedData)
		{
			byte[] encodedDataAsBytes= System.Convert.FromBase64String(encodedData);
			string returnValue = Encoding.UTF8.GetString(encodedDataAsBytes);
			return returnValue;
		}
		#endregion

		#region String Manipulation

		public static string Shorten(this string input, int sizeCut, bool addSuspensionPoints, bool cutOnlyOnSpaces)
		{
			if (input.Length < 5)
				return input;
			bool hasToBeCut = input.Length > sizeCut;
			string cutText = hasToBeCut ? input.Substring(0, sizeCut) : input;
			if (cutOnlyOnSpaces && hasToBeCut)
			{
				while (cutText[cutText.Length - 1] != ' ')
				{
					cutText = cutText.RightStrip(1);
				}
				cutText = cutText.RightStrip(1);
			}
			return string.Format("{0}{1}", cutText.Replace("[...]", ""), addSuspensionPoints && hasToBeCut ? "..." : string.Empty);
		}
		public static string RightStrip(this string s, int n)
		{
			return s.Substring(0, s.Length - n);
		}
		public static string StripHtml(this string inputString)
		{
			return Regex.Replace(inputString, @"<(.|\n)*?>", string.Empty);
		}
		public static string Capitalize(this string input)
		{
			return string.Format("{0}{1}", input.Substring(0, 1).ToUpper(), input.Substring(1).ToLower());
		}

		public static List<string> SplitToList(this string input, params char[] separator)
		{
			string[] tokens = input.Split(separator);
			return tokens.ToList();
		}

		public static string GetFileExtensionNoDot(this string fullpath)
		{
			FileInfo fi = new FileInfo(fullpath);
			return fi.Extension.Substring(1).ToLower();
		}

		public static MvcHtmlString ToMvcHtmlString(this string input)
		{
			return new MvcHtmlString(input);
		}

		public static string ReplaceDiacritics(this string input)
		{
			string stFormD = input.Normalize(NormalizationForm.FormD);
			StringBuilder sb = new StringBuilder();

			for (int ich = 0; ich < stFormD.Length; ich++)
			{
				UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
				if (uc != UnicodeCategory.NonSpacingMark)
				{
					sb.Append(stFormD[ich]);
				}
			}

			return (sb.ToString().Normalize(NormalizationForm.FormC));
		}

		public static string UrlFormat(this string input)
		{
			// this function has to replace spaces by '-'...



			// remove ', &, punctuations, replace +, etc
			string result = input.Replace("'", "");
			result = result.Replace("+", "plus");
			result = result.Replace("&", "et");
			result = result.Replace("`", "");
			result = result.Replace("~", "");
			result = result.Replace("@", "at");
			result = result.Replace("#", "");
			result = result.Replace("\"", "");
			result = result.Replace("$", "");
			result = result.Replace("€", "");
			result = result.Replace("£", "");
			result = result.Replace("*", "");
			result = result.Replace(",", "");
			result = result.Replace(".", "");
			result = result.Replace(";", "");
			result = result.Replace(":", "");
			result = result.Replace("!", "");
			result = result.Replace("?", "");
			result = result.Replace("(", "");
			result = result.Replace(")", "");
			result = result.Replace("[", "");
			result = result.Replace("]", "");
			result = result.Replace("{", "");
			result = result.Replace("}", "");
			result = result.Replace("/", "");
			result = result.Replace(@"\", "");
			result = Regex.Replace(result, @"\s+", "-");

			// replace diacritics (Ç -> C, É -> E, é -> e, ç -> c, etc)
			result = ReplaceDiacritics(result);
			// lower
			result = result.ToLower();

			// as a last resort, url encode
			result = HttpUtility.UrlEncode(result);

			return result;
		}
		#endregion

		#region Pattern Recognition
		public static bool IsUrl(this string input)
		{
			Regex rgx = new Regex(@"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)");
			return rgx.IsMatch(input);
		}
		public static bool IsMailAddress(this string input)
		{
			Regex rgx = new Regex(@"^([\w\-\._]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$");
			return rgx.IsMatch(input);
		}
		public static bool IsPhoneNumberFrench(this string input)
		{
			Regex rgx = new Regex(@"^(01|02|03|04|05|06|07|09)[0-9]{8}$");
			return rgx.IsMatch(input);
		}

		#endregion
	}
}
