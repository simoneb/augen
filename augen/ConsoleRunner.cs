using System;

namespace augen
{
	public class ConsoleRunner : AbstractRunner
	{
		protected override void ServerEnd(string serverName)
		{
			Console.WriteLine();
		}

		protected override void ServerBegin(string serverName)
		{
			Console.WriteLine("[{0}]", serverName);
		}

		protected override void ReportTest(string description, object outcome)
		{
			Console.WriteLine("{0} -> {1}", description, outcome);
		}
	}
}