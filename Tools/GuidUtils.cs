#region Usings

using System;

#endregion

namespace Tools
{
	public class GuidUtils
	{
		public static int GetQuiteUniqueInt32FromGuid(Guid input)
		{
			byte[] gb = input.ToByteArray();
			int i = BitConverter.ToInt32(gb, 0);
			if (i < 0)
				i = -i;
			return i;
		}
	}
}