using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
	public interface IOperationResult<T>
	{
		bool Result { get; set; }
		string Message { get; set; }
		T ReturnObject { get; set; }
	}
}
