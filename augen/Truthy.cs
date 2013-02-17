using System;
using System.Collections.Generic;
using System.Linq;

namespace augen
{
	public class Truthy
	{
		private readonly Type _type;
		private readonly Func<object, bool> _condition;

		internal Truthy(Type type, Func<object, bool> condition)
		{
			_type = type;
			_condition = condition;
		}

		private TruthyMatch TryMatch(object actual)
		{
			return new TruthyMatch(_type.IsInstanceOfType(actual), () => _condition(actual));
		}

		public static bool IsTruthy(object value, IEnumerable<Truthy> truthies)
		{
			if (value is bool)
				return (bool)value;

			var truthy = truthies.Select(t => t.TryMatch(value)).FirstOrDefault(tm => tm);

			return truthy != null && truthy.Value;
		}
	}

	internal class TruthyMatch
	{
		private readonly bool _matches;
		private readonly Func<bool> _getValue;

		public TruthyMatch(bool matches, Func<bool> getValue)
		{
			_matches = matches;
			_getValue = getValue;
		}

		public static implicit operator bool(TruthyMatch m)
		{
			return m._matches;
		}

		public bool Value { get { return _getValue(); } }
	}
}