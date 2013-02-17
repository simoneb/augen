using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace augen
{
	public static class TextRange
	{
		public static string[] Parse(string input)
		{
			return new Parser(input).Parse();
		}

		private class Parser
		{
			private readonly CharEnumerator _s;
			private char _c = ' ';
			private IEnumerable<StringBuilder> _outputs = new[]{new StringBuilder()};
			private bool _next;

			public Parser(string input)
			{
				_s = input.GetEnumerator();
				_next = _s.MoveNext();
				Next();
			}

			private void Next(char c = '\0')
			{
				if(c != '\0' && c != _c)
					throw new InvalidOperationException(string.Format("Expected char <{0}>", c));

				if (!_next)
				{
					_c = '\0';
					return;
				}

				_c = _s.Current;
				_next = _s.MoveNext();
			}

			public string[] Parse()
			{
				while (_next)
					ParseCurrent();

				ParseCurrent();

				return _outputs.Select(o => o.ToString()).ToArray();
			}

			private void ParseCurrent()
			{
				switch (_c)
				{
					case '[':
						Next();
						var from = ReadNumber();
						Next('-');
						var to = ReadNumber();
						Next(']');
						CreateOutputs(from, to);
						break;
					case '\0':
						break;
					default:
						foreach (var output in _outputs)
							output.Append(_c);

						Next();
						break;
				}
			}

			private void CreateOutputs(string from, string to)// IEnumerable<int> range, int fromLength, int toLength)
			{
				if (from.Length != to.Length)
					throw new InvalidOperationException(
						string.Format("Cannot generate range between {0} and {1}. The strings must be of the same length.", from, to));

				var start = int.Parse(from);
				var end = int.Parse(to);

				_outputs = (from output in _outputs
				            from r in Enumerable.Range(start, end - start + 1)
				            select new StringBuilder(output.ToString()).Append(r.ToString(new string('0', @from.Length)))).ToArray();
			}

			private string ReadNumber()
			{
				var o = "";

				while (char.IsNumber(_c))
				{
					o += _c;
					Next();
				}

				return o;
			}
		}
	}
}