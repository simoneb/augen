using System;

namespace augen.http
{
	public class Http : NonTerminal<Http>
	{
		private readonly int _port;

		public Http(Project project, int port) : base(project)
		{
			_port = port;
		}

		public Get Get(string path)
		{
			return new Get(Project, _port, path);
		}

		public Get Get(Options path)
		{
			return new Get(Project, _port, path);
		}
	}
}