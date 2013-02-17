using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace augen
{
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