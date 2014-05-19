using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace Tools
{
	public static class DateHelper
	{
		public static DateTime ParseOrSetToMaxDate(this string inputStrDate)
		{
			DateTime result;
			if (string.IsNullOrWhiteSpace(inputStrDate) || !DateTime.TryParse(inputStrDate, out result))
			{
				result = SqlDateTime.MaxValue.Value;
			}
			return result;
		}
		public static DateTime ParseOrSetToMinDate(this string inputStrDate)
		{
			DateTime result;
			if (string.IsNullOrWhiteSpace(inputStrDate) || !DateTime.TryParse(inputStrDate, out result))
			{
				result = SqlDateTime.MinValue.Value;
			}
			return result;
		}
		public static DateTime? ParseNullableDate(this string inputStrDate)
		{
			DateTime result;
			if (string.IsNullOrWhiteSpace(inputStrDate) || !DateTime.TryParse(inputStrDate, out result))
				return null;
			return result;
		}

		public static DateTime ParseYearToDateTime(this string inputString)
		{
			int year;
			if (inputString.Length == 4 && int.TryParse(inputString, out year))
			{
				return new DateTime(year, 1, 1);
			}
			else
				return new DateTime();
		}
	}
}
