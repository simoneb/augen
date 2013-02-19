using System;

namespace augen
{
	public abstract class AbstractRunner
	{
		public void Run(Project project)
		{
            foreach (var serverSet in project.Servers)
	        foreach (var serverName in serverSet.Names)
	        {
		        var serverOptions = serverSet.DescribeOptions(serverSet.Options);

				ServerBegin(serverName, serverOptions);

		        foreach (var connection in project.Connections)
		        {
					var connectionOptionsMultiple = new OptionsMultipleLookup(serverSet, connection);
					var connectionOptionsSingle = new OptionsSingleLookup(serverSet, connection);

					if (!connection.IsSatisfiedByFilters(connectionOptionsMultiple))
						continue;

					ConnectionBegin(connection.GetType(), connection.DescribeOptions(connectionOptionsSingle));

			        object connectionInstance;

			        try
			        {
				        connectionInstance = connection.OpenInternal(serverName, connectionOptionsSingle);
			        }
			        catch (Exception e)
			        {
				        ConnectionError(connection.GetType(), e);
				        continue;
			        }

			        foreach (var request in connection.Requests)
				    {
					    var options = new OptionsSingleLookup(serverSet, connection, request);

					    RequestBegin(request.GetType(), request.DescribeOptions(options));

					    object response;

					    try
					    {
						    response = request.ExecuteInternal(connectionInstance, options);
					    }
					    catch (Exception e)
					    {
						    RequestError(request.GetType(), e);
						    continue;
					    }

					    foreach (var test in request.Tests)
					    {
						    try
						    {
							    var outcome = test.Checker.Compile()(response);
							    TestComplete(test.Description, outcome, Truthy.IsTruthy(outcome, project.GetTruthies()));
						    }
						    catch (Exception e)
						    {
							    TestError(test.Description, e);
						    }
					    }

					    RequestEnd(request.GetType());

					    request.CloseInternal(response);
				    }

			        ConnectionEnd(connection.GetType());

					connection.CloseInternal(connectionInstance);
			    }

		        ServerEnd(serverName);
	        }
		}

		protected abstract void TestError(string description, Exception exception);

		protected abstract void RequestError(Type requestType, Exception exception);

		protected abstract void ConnectionError(Type connectionType, Exception exception);

		protected abstract void ServerBegin(string name, string serverOptions);

		protected virtual void ConnectionBegin(Type connectionType, string connectionOptions)
		{
		}

		protected virtual void RequestBegin(Type requestType, string requestOptions)
		{
		}

		protected abstract void TestComplete(string description, object outcome, bool success);

		protected virtual void RequestEnd(Type requestType)
		{
		}

		protected virtual void ConnectionEnd(Type connectionType)
		{
		}

		protected abstract void ServerEnd(string serverName);
	}
}