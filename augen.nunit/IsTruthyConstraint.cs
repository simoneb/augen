using System.Collections.Generic;
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

			return Truthy.IsTruthy(actual, _truthyValues);
		}

		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WriteExpectedValue("a truthy value");
		}
	}
}