using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
	public class ListResults<T> : List<OperationResult<T>> where T : class
	{
		public OperationResult<T> MainResult
		{
			get
			{
				OperationResult<T> mainResult = OperationResult<T>.OkResult;
				mainResult.Message = "";
				foreach (OperationResult<T> operationResult in this)
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
