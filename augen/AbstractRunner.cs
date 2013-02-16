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

					var connectionInstance = connection.OpenInternal(serverName, connectionOptionsSingle);

				    foreach (var request in connection.Requests)
				    {
					    var options = new OptionsSingleLookup(serverSet, connection, request);

					    var response = request.ExecuteInternal(connectionInstance, options);

					    foreach (var test in request.Tests)
						    ReportTest(test.Description, test.Checker.Compile()(response));

					    request.CloseInternal(response);
				    }

					connection.CloseInternal(connectionInstance);
			    }

		        ServerEnd(serverName);
	        }
		}

		protected abstract void ServerEnd(string serverName);

		protected abstract void ServerBegin(string serverName);

		protected abstract void ReportTest(string description, object outcome);
	}
}