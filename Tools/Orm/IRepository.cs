using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Orm
{
	public interface Crud<T, in TIdType> where T : class
	{
		StdResult<T> Create(T inputObject);

		StdResult<T> Get(TIdType id);

		StdResult<T> Update(T inputObject);

		StdResult<T> Delete(TIdType id);
	}
}
