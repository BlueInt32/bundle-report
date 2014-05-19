using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
	public static class IntHelper
	{
		public static int? ParseNullableInt(this string inputInt)
		{
			int result;
			if (string.IsNullOrWhiteSpace(inputInt) || !int.TryParse(inputInt, out result))
				return null;
			return result;
		}
	}
}
