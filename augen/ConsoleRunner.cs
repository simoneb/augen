using System;

namespace augen
{
	public class ConsoleRunner : AbstractRunner
	{
		private readonly bool _printDiagnostics;
		private readonly bool _printConnection;
		private readonly bool _printRequest;
		
		/// <summary>
		/// Creates a new <see cref="ConsoleRunner"/> with the provided options
		/// </summary>
		/// <param name="printDiagnostics">Whether to print the options for each of servers, connection and request</param>
		/// <param name="printConnection">Whether to print connection details</param>
		/// <param name="printRequest">Whether to print request details</param>
		public ConsoleRunner(bool printDiagnostics = false, bool printConnection = false, bool printRequest = false)
		{
			_printDiagnostics = printDiagnostics;
			_printConnection = printConnection;
			_printRequest = printRequest;
		}

		protected override void ServerBegin(string name, string serverOptions)
		{
			Console.Write("[{0}]", name);

			if (!_printDiagnostics)
			{
				Console.WriteLine();
				return;
			}
			
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(" " + serverOptions);
			Console.ResetColor();
		}

		protected override void ConnectionBegin(Type connectionType, string connectionOptions)
		{
			if (!_printConnection)
				return;

			Console.Write(" " + connectionType.Name);

			if (!_printDiagnostics)
			{
				Console.WriteLine();
				return;
			}
			
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(" " + connectionOptions);
			Console.ResetColor();
		}

		protected override void RequestBegin(Type requestType, string requestOptions)
		{
			if (!_printRequest)
				return;

			Console.Write("  " + requestType.Name);

			if (!_printDiagnostics)
			{
				Console.WriteLine();
				return;
			}
			
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(" " + requestOptions);
			Console.ResetColor();
		}

		protected override void TestError(string description, Exception exception)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("   {0} test error ", description);
			Console.ResetColor();
			Console.WriteLine(exception.Message);
		}

		protected override void RequestError(Type requestType, Exception exception)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("  {0} request error ", requestType.Name);
			Console.ResetColor();
			Console.WriteLine(exception.Message);
		}

		protected override void ConnectionError(Type connectionType, Exception exception)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(" {0} connection error ", connectionType.Name);
			Console.ResetColor();
			Console.WriteLine(exception.Message);
		}

		protected override void TestComplete(string description, object outcome, bool success)
		{
			Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
			Console.Write("   ");
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