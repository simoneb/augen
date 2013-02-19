using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace augen
{
	public abstract class OptionsHolder
	{
		private readonly List<Expression<Func<string, Func<object, object>>>> _options = new List<Expression<Func<string, Func<object, object>>>>();
		internal ILookup<string, Func<object, object>> Options { get { return _options.Select(ParseOption).ToLookup(_ => _.Item1, _ => _.Item2); } }

		protected OptionsHolder()
		{
		}

		protected OptionsHolder(IEnumerable<Expression<Func<string, object>>> options) : this(ConvertOptions(options))
		{
		}

		protected OptionsHolder(IEnumerable<Expression<Func<string, Func<object, object>>>> options)
		{
			_options = options.ToList();
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

		protected void AddOption(string optionName, Func<object, object> value)
		{
			var p = Expression.Parameter(typeof (string), optionName);
			_options.Add(Expression.Lambda<Func<string, Func<object, object>>>(Expression.Constant(value), p));
		}
	}
}