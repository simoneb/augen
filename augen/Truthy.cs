using System;

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

		internal bool Matches(object actual)
		{
			return _type.IsInstanceOfType(actual) && _condition(actual);
		}
	}
}