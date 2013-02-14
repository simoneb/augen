using System;
using System.Linq;
using System.Linq.Expressions;

namespace augen
{
	internal static class ExpressionUtils
	{
		public static Tuple<string, object> GetNameAndValue(Expression<Func<string, object>> expr)
		{
			return Tuple.Create(expr.Parameters.Single().Name, expr.Compile()(null));
		}
	}
}