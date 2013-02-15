using System.Net;
using augen.http;

namespace augen.console
{
	public class MyProject : Project
	{
		public MyProject() : base("some project",
		    new Servers { { "173.194.67.102", port => 80, path => "/calendar" } })
		{
			this.Http(123456)
                .Header("host", "google.com")
				.Get("/")
				.Test("status code", response => response.StatusCode == HttpStatusCode.OK)
				.Test("contents", response => response.ContentLength > 100);

			this.Http(12345)
				.Header("host", "google.com")
				.Get("/")
				.Test("status code", response => response.StatusCode == HttpStatusCode.OK)
				.Test("contents", response => response.ContentLength > 100);
		}
	}
}