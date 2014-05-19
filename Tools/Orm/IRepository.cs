using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Orm
{
	public interface Crud<T, in TIdType> where T : class
	{
		OperationResult<T> Create(T inputObject);

		OperationResult<T> Retrieve(TIdType id);

		OperationResult<T> Update(T inputObject);

		OperationResult<T> Delete(TIdType id);
	}
}
