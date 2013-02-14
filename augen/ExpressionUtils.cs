using System;
using System.Linq;
using System.Linq.Expressions;

namespace augen
{
	internal static class ExpressionUtils
	{
		public static Tuple<string, object> ParseOption(Expression<Func<string, object>> expr)
		{
			return Tuple.Create(expr.Parameters.Single().Name, expr.Compile()(null));
		}

	    internal static ILookup<string, object> ParseOptions(params Expression<Func<string, object>>[] options)
	    {
	        return options.Select(ParseOption).ToLookup(_ => _.Item1, _ => _.Item2);
	    }
	}
}