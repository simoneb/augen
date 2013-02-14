﻿using System;
using System.Collections.Generic;

namespace augen
{
	public class ConsoleRunner : AbstractRunner
	{
		protected override void ExecuteTests(string serverName, Options options, IEnumerable<TestDescriptor> tests)
		{
			Console.WriteLine("[{0}]", serverName);

			foreach (var test in tests)
				Console.WriteLine("{0} -> {1}", test.Description, test.Checker.Compile()(test.Execute(serverName, options)));

			Console.WriteLine();
		}
	}
}