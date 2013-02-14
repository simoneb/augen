using System.Net;
using augen.http;

namespace augen.console
{
	public class MyProject : Project
	{
		public MyProject() : base("some project",
								  new Servers { { "173.194.67.102", port => 80 } })
		{
			this.Http(123456)
				.Get(path => "/")
				.Header("host", "google.com")
				.Test("status code", response => response.StatusCode == HttpStatusCode.OK)
				.Test("contents", response => response.ContentLength > 100);

			this.Http(12345)
				.Get("/")
				.Header("host", "google.com")
				.Test("status code", response => response.StatusCode == HttpStatusCode.OK)
				.Test("contents", response => response.ContentLength > 100);
		}
	}
}