using System;
using System.Linq.Expressions;
using System.Net;
using NUnit.Framework;

namespace augen.tests
{
    public class RequestTests
    {
        [Test]
        public void Test()
        {
            Expression<Func<dynamic, dynamic>> option = option1 => option1 + 2;
            var request = new StubRequest(new StubConnection(new StubProject()), option);

            request.
        }
    }

    public class StubProject : Project
    {
        public StubProject() : base("stub", new Servers{{"one", option1 => 2}})
        {
        }
    }

    public class StubConnection : Connection<int>
    {
        public StubConnection(Project project) : base(project)
        {
        }

        protected override int Open(string serverName, Options options)
        {
            return 0;
        }

        protected override void Close(int connection)
        {
        }
    }

    public class StubRequest : Request<int, int>
    {
        public StubRequest(Connection connection, LambdaExpression options) : base(connection, options)
        {
        }

        protected override int Execute(Options options)
        {
            return 0;
        }
    }
}
