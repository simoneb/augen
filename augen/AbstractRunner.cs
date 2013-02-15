using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace augen
{
	public abstract class AbstractRunner
	{
		public void Run(Project project)
		{
            foreach (var serverSet in project.Servers)
	        foreach (var serverName in serverSet.Names)
	        {
				ServerBegin(serverName);

		        foreach (var connection in project.Connections)
		        {
			        object conn = null;

			        foreach (var request in connection.Requests)
			        {
				        Options options = expr => GetOption(expr, serverSet.Options, request.Options);

				        conn = conn ?? connection.OpenInternal(serverName, options);

				        var response = request.ExecuteInternal(conn, options);

				        foreach (var test in request.Tests)
				        {
					        ReportTest(test.Description, test.Checker.Compile()(response));
				        }

				        request.CloseInternal(response);
			        }

			        if (conn != null)
				        connection.CloseInternal(conn);
		        }

		        ServerEnd(serverName);
	        }
		}

		protected abstract void ServerEnd(string serverName);

		protected abstract void ServerBegin(string serverName);

		protected abstract void ReportTest(string description, object outcome);

		private static dynamic GetOption(Expression<Func<string, object>> accessor, ILookup<string, object> serverOptions, IReadOnlyDictionary<string, object> requestOptions)
		{
			var accessorData = ExpressionUtils.ParseOption(accessor);

			object requestOption;
		
			return serverOptions[accessorData.Item1].LastOrDefault() ??
				(requestOptions.TryGetValue(accessorData.Item1, out requestOption)
				       ? requestOption
				       :  accessorData.Item2);
		}
	}

	internal class DynamicLastLookup : DynamicObject
	{
		private readonly ILookup<string, object> _lookup;

		public DynamicLastLookup(ILookup<string, object> lookup)
		{
			_lookup = lookup;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return (result = _lookup[binder.Name].LastOrDefault()) != null;
		}
	}
}