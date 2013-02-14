using System;
using System.Linq.Expressions;

namespace augen
{
	public class Test<TResponse> : Test
	{
		public string Description { get; private set; }
		public Expression<Func<TResponse, object>> Checker { get; private set; }

		public Test(string description, Expression<Func<TResponse, object>> checker)
		{
			Description = description;
			Checker = checker;
		}
	}

    public class Test {}
}