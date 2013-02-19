using System;
using System.Collections.Generic;
using System.Net.Http;

namespace augen.Http
{
	public class Http : Connection<Http, HttpClient>
	{
		private readonly IDictionary<string, string> _headers = new Dictionary<string, string>();

		public Http(Project project, string scheme, int port) : base(project, _scheme => scheme, _port => port)
		{
		}

		public Get Get(string path)
		{
			return new Get(this, path);
		}

		public Get Get(Func<dynamic, string> path)
		{
			return new Get(this, path);
		}

		public Http Header(string name, string value)
		{
			return Header(name, _ => value);
		}

		public Http Header(string name, Func<dynamic, string> value)
		{
			var optionName = "http_header:" + name;

			_headers[name] = optionName;
			AddOption(optionName, value);
			
			return this;
		}

		protected override HttpClient Open(string serverName, dynamic options)
		{
			var client = new HttpClient {BaseAddress = new UriBuilder(options.scheme, serverName).Uri};

			foreach (var header in _headers)
				client.DefaultRequestHeaders.Add(header.Key, (string)options[header.Value]);

			return client;
		}

		protected override void Close(HttpClient connection)
		{
			connection.Dispose();
		}
	}
}