using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
	public class ListResults<T> : List<StdResult<T>> where T : class
	{
		public StdResult<T> MainResult
		{
			get
			{
				StdResult<T> mainResult = StdResult<T>.OkResult;
				mainResult.Message = "";
				foreach (StdResult<T> operationResult in this)
				{
					mainResult.Result &= operationResult.Result;
					if (operationResult.Message != "Ok" && !string.IsNullOrWhiteSpace(operationResult.Message))
						mainResult.Message = string.Concat(mainResult.Message, " / ", operationResult.Message);
				}
				if (mainResult.Result)
					mainResult.Message = "Ok";
				return mainResult;
			}
		}
	}
}
