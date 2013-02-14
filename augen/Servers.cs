﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace augen
{
	public class Servers : IEnumerable<ServerSet>
	{
		private readonly Collection<ServerSet> servers = new Collection<ServerSet>();

		public void Add(string name, params Expression<Func<string, object>>[] options)
		{
			servers.Add(new ServerSet(ParseName(name), ExpressionUtils.ParseOptions(options)));
		}

	    private static string[] ParseName(string name)
		{
			return new[] {name};
		}

		public IEnumerator<ServerSet> GetEnumerator()
		{
			return servers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}