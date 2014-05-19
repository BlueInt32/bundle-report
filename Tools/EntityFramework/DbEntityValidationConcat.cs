using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace Tools.EntityFramework
{
	public static class DbEntityValidationConcat
	{
		public static string GetAllErrorsInOneString(this IEnumerable<DbEntityValidationResult> validationErrors)
		{
			StringBuilder sb = new StringBuilder();
			foreach (DbEntityValidationResult dbEntityValidationResult in validationErrors)
			{

				foreach (DbValidationError dbValidationError in dbEntityValidationResult.ValidationErrors)
				{
					sb.AppendLine(dbValidationError.ErrorMessage);
				}
			}
			return sb.ToString();
		}


		public static List<string> FormatValidationErrorList(this IEnumerable<DbEntityValidationResult> validationErrors)
		{
			List<string> result = new List<string>();
			foreach (DbEntityValidationResult dbEntityValidationResult in validationErrors)
			{
				foreach (DbValidationError dbValidationError in dbEntityValidationResult.ValidationErrors)
				{
					result.Add(string.Format("{0} : '{1}'", dbValidationError.PropertyName, dbValidationError.ErrorMessage));
				}
			}
			return result;
		}

	}
}
