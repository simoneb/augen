using System;
using System.Collections.Generic;
using System.Net;

namespace augen.http
{
	public class Http : Connection<HttpWebRequest>
	{
		private readonly int _port;
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

		public Http(Project project, int port) : base(project)
		{
			_port = port;
		}

        public Http Header(string name, string value)
        {
            _headers[name] = value;
            return this;
        }

		public Get Get(string path)
		{
			return new Get(this, path);
		}

	    protected override HttpWebRequest Open(string serverName, Options options)
	    {
            var http = WebRequest.CreateHttp(new UriBuilder("http", serverName, options(port => _port), options(path => null)).ToString());
            http.Timeout = 1000;

            foreach (var header in _headers)
            {
                if (header.Key.Equals("host", StringComparison.OrdinalIgnoreCase))
                    http.Host = header.Value;
                else
                    http.Headers[header.Key] = header.Value;
            }

	        return http;
	    }

	    protected override void Close(HttpWebRequest connection)
	    {
	    }
	}
}