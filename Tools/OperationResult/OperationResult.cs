#region Usings

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;

#endregion

namespace Tools
{
	public class StdResult<T> : IStdResult<T> where T : class
	{
		public StdResult()
		{
		}
		public T ReturnObject { get; set; }


		public bool Result { get; set; }
		public string Message { get; set; }

		public List<string> ErrorList { get; set; }

		#region Statics
		public static StdResult<T> OkResult
		{
			get { return new StdResult<T> { Result = true, Message = "Ok" }; }
		}
		public static StdResult<T> OkResultInstance(T instance)
		{
			return new StdResult<T> { Result = true, Message = "Ok", ReturnObject = instance };
		}

		public static StdResult<T> BadResult(string message)
		{
			
			return new StdResult<T> { Result = false, Message = message };
		}

		public static StdResult<T> BadResultWithList(string message, List<string> errorList)
		{
			return new StdResult<T> { Result = false, Message = message, ErrorList = errorList };
		}

		public static StdResult<T> BadResult(string message, T badInstance)
		{
			return new StdResult<T> { Result = false, Message = message, ReturnObject = badInstance };
		}

		public static StdResult<T> BadResultFormat(string message, params object[] args)
		{
			return new StdResult<T> { Result = false, Message = string.Format(message, args) };
		}
		#endregion

		public StdResult<T> LogicalAnd(bool result, string message)
		{
			StdResult<T> newResult = new StdResult<T>
			{
				Result = Result & result,
			};

			string.Concat(newResult.Message, " / ", message);
			return newResult;
		}
	}
}