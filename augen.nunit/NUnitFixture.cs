﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;

namespace augen.nunit
{
	[TestFixture]
	public abstract class NUnitFixture<TProject> where TProject : Project, new()
    {
		private readonly TProject _project;
		private readonly IsTruthyConstraint _isTruthy;

		protected NUnitFixture()
		{
			_project = new TProject();
			_isTruthy = new IsTruthyConstraint(_project.GetTruthies());
		}

		[Test, TestCaseSource("GetTests")]
	    public void AugenTests(Func<object> openConnection, Func<object, object> executeRequest,
	                           Expression<Func<object, object>> checker, Action<object> closeResponse,
	                           Action<object> closeConnection)
		{
			var connection = openConnection();
		    var response = executeRequest(connection);

		    var outcome = checker.Compile()(response);

			try
			{
				Assert.That(outcome, _isTruthy);
			}
			finally
			{
				closeResponse(response);
				closeConnection(connection);	
			}
		}

		protected virtual FilterBehavior FilterBehavior { get { return FilterBehavior.Ignore; } }

		public IEnumerable<ITestCaseData> GetTests()
	    {
			foreach (var serverSet in _project.Servers)
			foreach (var serverName in serverSet.Names)
			foreach (var connection in _project.Connections)
			{
				var ignore = false;

				var connectionOptionsMultiple = new OptionsMultipleLookup(serverSet, connection);
				var connectionOptionsSingle = new OptionsSingleLookup(serverSet, connection);

				if (!connection.IsSatisfiedByFilters(connectionOptionsMultiple))
					switch (FilterBehavior)
					{
						case FilterBehavior.Hide:
							continue;
						case FilterBehavior.Ignore:
							ignore = true;
							break;
						default:
							throw new InvalidOperationException(string.Format("FilterBehavior {0} not recognized", FilterBehavior));
					}

				var localConnection = connection;
				var localServerName = serverName;

				var openConnection = new Func<object>(() => localConnection.OpenInternal(localServerName, connectionOptionsSingle));
				var closeConnection = new Action<object>(conn => localConnection.CloseInternal(conn));

				foreach (var request in connection.Requests)
				{
					var options = new OptionsSingleLookup(serverSet, connection, request);
					var localRequest = request;

					var executeRequest = new Func<object, object>(conn => localRequest.ExecuteInternal(conn, options));
					var closeResponse = new Action<object>(resp => localRequest.CloseInternal(resp));

					foreach (var test in request.Tests)
					{
						var data = new TestCaseData(openConnection, executeRequest, test.Checker, closeResponse, closeConnection)
							.SetName(string.Format("[{0}] {1}", serverName, test.Description));

						if (ignore)
							data.Ignore();

						yield return data;
					}
				}
			}
	    }
    }
}