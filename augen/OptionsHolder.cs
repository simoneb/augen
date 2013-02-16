using System;
using System.Linq;
using System.Linq.Expressions;

namespace augen
{
	public abstract class OptionsHolder
	{
		internal ILookup<string, object> Options { get; private set; }

		protected OptionsHolder(params Expression<Func<string, object>>[] options)
		{
			Options = options.Select(ParseOption).ToLookup(_ => _.Item1, _ => _.Item2);
		}

		internal static Tuple<string, object> ParseOption(Expression<Func<string, object>> expr)
		{
			return Tuple.Create(expr.Parameters.Single().Name.TrimStart('_'), expr.Compile()(null));
		}
	}
}