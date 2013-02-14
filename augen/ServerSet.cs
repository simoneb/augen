using System.Linq;

namespace augen
{
	public class ServerSet
	{
		public string[] Names { get; private set; }
		public ILookup<string, object> Options { get; private set; }

		public ServerSet(string[] names, ILookup<string, object> options)
		{
			Names = names;
			Options = options;
		}
	}
}