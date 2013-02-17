using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace augen
{
	public abstract class OptionsHolder
	{
		internal ILookup<string, Func<object, object>> Options { get; private set; }

		protected OptionsHolder()
		{
			Options = Enumerable.Empty<Tuple<string, Func<object, object>>>().ToLookup(o => o.Item1, o => o.Item2);
		}

		protected OptionsHolder(IEnumerable<Expression<Func<string, object>>> options) : this(ConvertOptions(options))
		{
		}

		protected OptionsHolder(IEnumerable<Expression<Func<string, Func<object, object>>>> dynamicOptions)
		{
			Options = dynamicOptions.Select(ParseOption).ToLookup(_ => _.Item1, _ => _.Item2);
		}

		private static IEnumerable<Expression<Func<string, Func<object, object>>>> ConvertOptions(IEnumerable<Expression<Func<string, object>>> options)
		{
			var par = Expression.Parameter(typeof(object), "options_ignored");

			return options.Select(o => Expression.Lambda<Func<string, Func<object, object>>>(Expression.Lambda<Func<object, object>>(o.Body, par), o.Parameters));
		}

		internal static Tuple<string, T> ParseOption<T>(Expression<Func<string, T>> expr)
		{
			return Tuple.Create(expr.Parameters.Single().Name.TrimStart('_'), expr.Compile()(null));
		}
	}
}