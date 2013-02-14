using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace augen
{
	public class Servers : IEnumerable<ServerSet>
	{
		private readonly Collection<ServerSet> _dict = new Collection<ServerSet>();

		public void Add(string name, params Expression<Func<string, object>>[] options)
		{
			_dict.Add(new ServerSet(ParseName(name), ParseOptions(options)));
		}

		private static ILookup<string, object> ParseOptions(IEnumerable<Expression<Func<string, object>>> options)
		{
			return options.Select(ExpressionUtils.GetNameAndValue).ToLookup(arg => arg.Item1, arg => arg.Item2);
		}

		private static string[] ParseName(string name)
		{
			return new[] {name};
		}

		public IEnumerator<ServerSet> GetEnumerator()
		{
			return _dict.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}