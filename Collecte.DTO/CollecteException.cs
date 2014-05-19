using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collecte.DTO
{
	public class CollecteException : Exception
	{
		public CollecteException()
		{
		}
		public CollecteException(string message)
			: base(message)
		{
		}
		public CollecteException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
