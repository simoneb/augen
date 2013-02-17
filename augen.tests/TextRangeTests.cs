using System.Collections.Generic;
using NUnit.Framework;

namespace augen.tests
{
    public class TextRangeTests
    {
		[Test, TestCaseSource(typeof(TextRangeTests), "GetEnumerator")]
		public IEnumerable<string> Tests(string input)
		{
			return TextRange.Parse(input);
		}

	    private static IEnumerable<ITestCaseData> GetEnumerator()
	    {
			yield return new TestCaseData("abc").SameName().Returns(new[] { "abc" });
			yield return new TestCaseData("[1-2]b").SameName().Returns(new[] { "1b", "2b" });
			yield return new TestCaseData("a[1-2]b").SameName().Returns(new[] { "a1b", "a2b" });
			yield return new TestCaseData("a[1-2]").SameName().Returns(new[] { "a1", "a2" });
			yield return new TestCaseData("a[001-002]b").SameName().Returns(new[] { "a001b", "a002b" });
			yield return new TestCaseData("a[1-2]b[3-4]c").SameName().Returns(new[] { "a1b3c", "a1b4c", "a2b3c", "a2b4c" });
	    }
    }

	public static class Ext
	{
		public static TestCaseData SameName(this TestCaseData input)
		{
			input.SetName(string.Join(",", input.Arguments));
			return input;
		}
	}
}
