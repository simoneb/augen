﻿using System;

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
					var connectionOptionsMultiple = new OptionsMultipleLookup(serverSet, connection);
					var connectionOptionsSingle = new OptionsSingleLookup(serverSet, connection);

					if (!connection.IsSatisfiedByFilters(connectionOptionsMultiple))
						continue;

					ConnectionBegin(connection.GetType());

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

					    RequestBegin(request.GetType());

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
							    ReportTest(test.Description, outcome, Truthy.IsTruthy(outcome, project.GetTruthies()));
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

		protected abstract void ServerBegin(string serverName);

		protected virtual void ConnectionBegin(Type connectionType)
		{
		}

		protected virtual void RequestBegin(Type requestType)
		{
		}

		protected abstract void ReportTest(string description, object outcome, bool success);

		protected virtual void RequestEnd(Type requestType)
		{
		}

		protected virtual void ConnectionEnd(Type connectionType)
		{
		}

		protected abstract void ServerEnd(string serverName);
	}
}