using System;
using System.Collections.Generic;
using System.Net;

namespace augen.http
{
	public class Get : Terminal<HttpWebResponse>
	{
		private readonly int _port;
		private readonly Options _path;
		private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

		public Get(Project project, int port, string path) : this(project, port, _ => path)
		{
		}

		public Get(Project project, int port, Options path) : base(project)
		{
			_port = port;
			_path = path;
		}

		public Get Header(string name, string value)
		{
			_headers[name] = value;
			return this;
		}

		protected override HttpWebResponse Execute(string serverName, Options options)
		{
			var pathFromLocalOptions = _path(path => _path(_ => null));

			var http = WebRequest.CreateHttp(new UriBuilder("http", serverName, options(port => _port), options(path => pathFromLocalOptions)).ToString());
			http.Timeout = 1000;

			foreach (var header in _headers)
			{
				if (header.Key.Equals("host", StringComparison.OrdinalIgnoreCase))
					http.Host = header.Value;
				else
					http.Headers[header.Key] = header.Value;
			}

			var response = (HttpWebResponse) http.GetResponse();
			response.Close();
			return response;
		}
	}
}