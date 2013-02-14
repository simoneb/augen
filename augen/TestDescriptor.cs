using System;
using System.Linq.Expressions;

namespace augen
{
	public class TestDescriptor
	{
		private readonly Terminal _terminal;

		public string Description { get; private set; }
		public Expression<Func<object, object>> Checker { get; private set; }

		public TestDescriptor(Terminal terminal, string description, Expression<Func<object, object>> checker)
		{
			_terminal = terminal;
			Description = description;
			Checker = checker;
		}

		public object Execute(string serverName, Options options)
		{
			return _terminal.ExecuteInternal(serverName, options);
		}
	}
}