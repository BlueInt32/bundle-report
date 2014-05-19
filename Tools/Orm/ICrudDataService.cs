#region Usings

using System;

#endregion

namespace GeneralTools.Orm
{
	public interface ICrudDataService<in T> where T : IPlainObject
	{
		OperationResult<T> NewObject(T pocoObject);

		OperationResult<T> DeleteObject(Guid idObject);

		IPlainObject GetObject(Guid pocoId);

		OperationResult<T> UpdateObject(T pocoObject);
	}
}