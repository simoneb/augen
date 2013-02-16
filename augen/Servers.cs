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

		public void Add(Names names, params Expression<Func<string, object>>[] options)
		{
			_servers.Add(new ServerSet(names.SelectMany(ParseName).ToArray(), options));
		}

	    private static string[] ParseName(string name)
		{
			return new[] {name};
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

	public class Names : IEnumerable<string>
	{
		private readonly Collection<string> _names = new Collection<string>();

		public void Add(string name)
		{
			_names.Add(name);
		}

		public IEnumerator<string> GetEnumerator()
		{
			return _names.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}