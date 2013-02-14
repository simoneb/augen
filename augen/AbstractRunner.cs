using System;
using System.Linq;
using System.Linq.Expressions;

namespace augen
{
	public abstract class AbstractRunner
	{
		public void Run(Project project)
		{
            foreach (var serverSet in project.Servers)
            foreach (var serverName in serverSet.Names)
            foreach (var connection in project.Connections)
            {
                foreach (var request in connection.Requests)
                {
                    var conn = connection.OpenInternal(serverName, expr => GetOption(expr, serverSet.Options, request.Options));


                    //var response = request.Execute()
                }

                //connection.CloseInternal(conn);
            }
		}

		private static dynamic GetOption(Expression<Func<string, object>> accessor, ILookup<string, object> serverOptions, Tuple<string[], Delegate>[] requestOptions)
		{
			var accessorData = ExpressionUtils.ParseOption(accessor);
            var parsedRequestOptions = 

			return serverOptions[accessorData.Item1].FirstOrDefault() ?? accessorData.Item2;
		}
	}
}