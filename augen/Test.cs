using System;
using System.Linq.Expressions;

namespace augen
{
	public class Test
	{
		public string Description { get; private set; }
		public Expression<Func<object, object>> Checker { get; private set; }

		public Test(string description, Expression<Func<object, object>> checker)
		{
			Description = description;
			Checker = checker;
		}
	}
}