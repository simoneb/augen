using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace augen
{
	public abstract class AbstractRunner
	{
		public void Run(Project project)
		{
			project.AcceptRunner(this);
		}

		internal void RunImpl(string projectName, IEnumerable<ServerSet> servers, TestDescriptor[] tests)
		{
			foreach (var serverSet in servers)
			foreach (var serverName in serverSet.Names)
			{
				var set = serverSet;
				ExecuteTests(serverName, expr => CreateOptions(expr, set.Options), tests);
			}
		}

		private static dynamic CreateOptions(Expression<Func<string, object>> expr, ILookup<string, object> options)
		{
			var nameAndValue = ExpressionUtils.GetNameAndValue(expr);

			return options[nameAndValue.Item1].FirstOrDefault() ?? nameAndValue.Item2;
		}

		protected abstract void ExecuteTests(string serverName, Options options, IEnumerable<TestDescriptor> tests);
	}
}