using System;
using System.Linq.Expressions;

namespace augen
{
	public class ServerSet : OptionsHolder
	{
		public string[] Names { get; private set; }

		public ServerSet(string[] names, Expression<Func<string, object>>[] options) : base(options)
		{
			Names = names;
		}
	}
}