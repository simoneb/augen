using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Linq;

namespace augen
{
	public class Servers : IEnumerable<ServerSet>
	{
		private readonly Collection<ServerSet> _servers = new Collection<ServerSet>();

		public void Add(string name, params Expression<Func<string, object>>[] options)
		{
			_servers.Add(new ServerSet(ParseName(name), options));
		}

		public void Add(IEnumerable<string> names, params Expression<Func<string, object>>[] options)
		{
			_servers.Add(new ServerSet(names.SelectMany(ParseName).ToArray(), options));
		}

	    private static string[] ParseName(string name)
	    {
		    return TextRange.Parse(name);
	    }

		public IEnumerator<ServerSet> GetEnumerator()
		{
			return _servers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}