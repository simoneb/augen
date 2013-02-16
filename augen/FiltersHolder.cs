using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace augen
{
	public abstract class FiltersHolder : OptionsHolder
	{
		private readonly List<Tuple<string, object>> _filters = new List<Tuple<string, object>>();

		protected FiltersHolder(params Expression<Func<string, object>>[] options) : base(options)
		{
		}

		internal bool IsSatisfiedByFilters(dynamic options)
		{
			return !_filters.Any() || _filters.Any(f => options[f.Item1].Contains(f.Item2));
		}

		protected void AddFilters(IEnumerable<Expression<Func<string, object>>> filters)
		{
			_filters.AddRange(filters.Select(ParseOption));
		}
	}
}