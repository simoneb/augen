using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace augen
{
	internal class OptionsMultipleLookup : DynamicObject
	{
		private readonly OptionsHolder[] _holders;

		public OptionsMultipleLookup(params OptionsHolder[] holders)
		{
			_holders = holders;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return TryGetOption(binder.Name, out result);
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			return TryGetOption(indexes[0].ToString(), out result);
		}

		private bool TryGetOption(string name, out object result)
		{
			var found = _holders.FirstOrDefault(l => l.Options.Contains(name));

			result = found != null ? found.Options[name].Select(o => o(this)).ToList() : new List<object>();

			return true;
		}
	}
}