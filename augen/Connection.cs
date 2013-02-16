using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace augen
{
    public abstract class Connection : FiltersHolder
    {
	    internal List<Request> Requests { get; private set; }

        protected Connection(Project project, params Expression<Func<string, object>>[] options) : base(options)
        {
            project.Add(this);
            Requests = new List<Request>();
        }

        internal void Add(Request request)
        {
            Requests.Add(request);
        }

        internal abstract void CloseInternal(object connection);
        internal abstract object OpenInternal(string serverName, dynamic options);
    }

	public abstract class Connection<TSelf, TConnection> : Connection where TSelf : Connection<TSelf, TConnection>
	{
        protected Connection(Project project, params Expression<Func<string, object>>[] options) : base(project, options) { }

        protected abstract TConnection Open(string serverName, dynamic options);
        protected abstract void Close(TConnection connection);

        internal override object OpenInternal(string serverName, dynamic options)
        {
            return Open(serverName, options);
        }

        internal override void CloseInternal(object connection)
        {
            Close((TConnection)connection);
        }

		public TSelf Roles(string role, params string[] roles)
		{
			AddFilters(new[]{role}.Concat(roles).Select(r => MakeExpression(_role => r)));
			return (TSelf) this;
		}

		private static Expression<Func<string, object>> MakeExpression(Expression<Func<string, object>> expr)
		{
			return expr;
		}
	}
}