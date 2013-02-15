using System.Collections.Generic;

namespace augen
{
    public abstract class Connection 
    {
        internal Project Project { get; private set; }
        public List<Request> Requests { get; private set; }

        protected Connection(Project project)
        {
            Project = project;
            project.Add(this);
            Requests = new List<Request>();
        }

        internal void Add(Request request)
        {
            Requests.Add(request);
        }

        internal abstract void CloseInternal(object connection);
        internal abstract object OpenInternal(string serverName, Options options);
    }

    public abstract class Connection<TConnection> : Connection
	{
        protected Connection(Project project) : base(project) { }

        protected abstract TConnection Open(string serverName, Options options);
        protected abstract void Close(TConnection connection);

        internal override object OpenInternal(string serverName, Options options)
        {
            return Open(serverName, options);
        }

        internal override void CloseInternal(object connection)
        {
            Close((TConnection)connection);
        }
	}
}