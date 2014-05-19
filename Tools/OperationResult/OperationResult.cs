#region Usings

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;

#endregion

namespace Tools
{
	public class OperationResult<T> : IOperationResult<T> where T : class
	{
		public OperationResult()
		{
		}
		public T ReturnObject { get; set; }


		public bool Result { get; set; }
		public string Message { get; set; }

		public List<string> ErrorList { get; set; }

		#region Statics
		public static OperationResult<T> OkResult
		{
			get { return new OperationResult<T> { Result = true, Message = "Ok" }; }
		}
		public static OperationResult<T> OkResultInstance(T instance)
		{
			return new OperationResult<T> { Result = true, Message = "Ok", ReturnObject = instance };
		}

		public static OperationResult<T> BadResult(string message)
		{
			
			return new OperationResult<T> { Result = false, Message = message };
		}

		public static OperationResult<T> BadResultWithList(string message, List<string> errorList)
		{
			return new OperationResult<T> { Result = false, Message = message, ErrorList = errorList };
		}

		public static OperationResult<T> BadResult(string message, T badInstance)
		{
			return new OperationResult<T> { Result = false, Message = message, ReturnObject = badInstance };
		}

		public static OperationResult<T> BadResultFormat(string message, params object[] args)
		{
			return new OperationResult<T> { Result = false, Message = string.Format(message, args) };
		}
		#endregion

		public OperationResult<T> LogicalAnd(bool result, string message)
		{
			OperationResult<T> newResult = new OperationResult<T>
			{
				Result = Result & result,
			};

			string.Concat(newResult.Message, " / ", message);
			return newResult;
		}
	}
}