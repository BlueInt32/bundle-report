#region Usings

using System.Collections.Generic;

#endregion

namespace GeneralTools.Orm
{
	public interface IListDataService<T>
		where T : IPlainObject
	{
		IListFilter Filter { get; set; }

		List<T> ListObjects();
	}
}