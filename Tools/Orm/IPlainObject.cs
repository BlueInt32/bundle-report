#region Usings

using System;

#endregion

namespace GeneralTools.Orm
{
	public interface IPlainObject
	{
		Guid Id { get; set; }
		string Nom { get; set; }
		DateTime DateCreation { get; set; }
		DateTime? DateUpdate { get; set; }
	}
}