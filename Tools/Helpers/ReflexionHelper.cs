using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Tools
{
	public static class ReflexionHelper
	{
		public static string MemberName<T, R>(this T obj, Expression<Func<T, R>> expr)
		{
			var node = expr.Body as MemberExpression;
			if (ReferenceEquals(null, node))
				throw new InvalidOperationException("Expression must be of member access");
			return node.Member.Name;
		}
	}
}
