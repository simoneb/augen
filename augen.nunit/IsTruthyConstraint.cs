using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;

namespace augen.nunit
{
	internal class IsTruthyConstraint : Constraint
	{
		private readonly IEnumerable<Truthy> _truthyValues;

		public IsTruthyConstraint(IEnumerable<Truthy> truthyValues)
		{
			_truthyValues = truthyValues;
		}

		public override bool Matches(object actual)
		{
			this.actual = actual;

			if (actual is bool)
				return (bool)actual;

			if (_truthyValues.Any(t => t.Matches(actual)))
				return true;

			return false;
		}

		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WriteExpectedValue("a truthy value");
		}
	}
}