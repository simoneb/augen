using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace augen
{
	public class ServerSet : OptionsHolder
	{
		internal string[] Names { get; private set; }

		internal ServerSet(string[] names, IEnumerable<Expression<Func<string, object>>> options) : base(options)
		{
			Names = names;
		}
	}
}