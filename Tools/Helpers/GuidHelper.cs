using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
	public static class GuidHelper
	{
		public static Guid? ParseToNullableGuid(this string inputString)
		{
			Guid result;
			if (Guid.TryParse(inputString, out result))
				return result;
			return null;
		}

		public static Guid ParseGuidOrEmptyGuid(this string inputString)
		{
			Guid result;
			if (Guid.TryParse(inputString, out result))
				return result;
			return Guid.Empty;
		}

		public static int GetQuiteUniqueInt32(this Guid input)
		{
			byte[] gb = input.ToByteArray();
			int i = BitConverter.ToInt32(gb, 0);
			if (i < 0)
				i = -i;
			return i;
		}

		public static List<Guid> ParseSerializedGuidList(this string input, char sep)
		{
			List<string> l = input.Split(sep).ToList();
			List<Guid> result = new List<Guid>();
			foreach (string s in l)
			{
				Guid g;
				if(Guid.TryParse(s, out g))
					result.Add(g);
			}
			return result;
		}
	}
}
