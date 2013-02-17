using System;

namespace augen
{
	public class ConsoleRunner : AbstractRunner
	{
		protected override void ServerBegin(string serverName)
		{
			Console.WriteLine("[{0}]", serverName);
		}

		protected override void ReportTest(string description, object outcome, bool success)
		{
			Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
			Console.Write(" ");
			Console.Write(success ? "OK" : "KO");
			Console.ResetColor();

			Console.WriteLine(" {0} -> {1}", description, outcome);
		}

		protected override void ServerEnd(string serverName)
		{
			Console.WriteLine();
		}
	}
}