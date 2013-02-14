using System;
using System.Linq.Expressions;
using System.Net;

namespace augen.http
{
	public class Get : Request<HttpWebRequest, HttpWebResponse>
	{
		public Get(Connection connection, string pathArg) : this(connection, path => pathArg)
		{
		}

		public Get(Connection connection, Expression<Func<object, string>> path) : base(connection, path)
		{
		}

		protected override HttpWebResponse Execute(Options options)
		{
		    var response = (HttpWebResponse) null;// http.GetResponse();
            //response.Close();
			return response;
		}
	}
}