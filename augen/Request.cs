using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace augen
{
	public delegate dynamic Options(Expression<Func<string, dynamic>> expr);

    public abstract class Request<TRequest, TResponse> : Request<TResponse>
	{
        protected Request(Connection connection, params Tuple<string, object>[] options) : base(connection, options)
        {
        }

        public Request<TRequest, TResponse> Test(string description, Expression<Func<TResponse, object>> checker)
		{
			Add(new Test(description, Convert(checker)));
            return this;
		}

        private static Expression<Func<object, object>> Convert(Expression<Func<TResponse, object>> input)
		{
			var parameter = Expression.Parameter(typeof(object));
			return Expression.Lambda<Func<object, object>>(Expression.Invoke(input, Expression.Convert(parameter, typeof(TResponse))), parameter);
		}

	    internal override TResponse ExecuteInternal2(object connection, Options options)
		{
			return Execute((TRequest) connection, options);
		}

	    protected abstract TResponse Execute(TRequest request, Options options);
	}

    public abstract class Request<TResponse> : Request
	{
	    protected Request(Connection connection, IEnumerable<Tuple<string, object>> options) : base(connection, options)
	    {
	    }

	    internal abstract TResponse ExecuteInternal2(object connection, Options options);

	    internal override object ExecuteInternal(object connection, Options options)
        {
            return ExecuteInternal2(connection, options);
        }

		internal override void CloseInternal(object response)
		{
			Close((TResponse) response);
		}

	    protected abstract void Close(TResponse response);
	}

    public abstract class Request
    {
        internal List<Test> Tests { get; private set; }
        internal IReadOnlyDictionary<string, object> Options { get; private set; }

        protected Request(Connection connection, IEnumerable<Tuple<string, object>> options)
        {
            Options = options.ToDictionary(t => t.Item1, t => t.Item2);
            Tests = new List<Test>();

            connection.Add(this);
        }

        internal void Add(Test test)
        {
            Tests.Add(test);
        }

        internal abstract object ExecuteInternal(object connection, Options options);

	    internal abstract void CloseInternal(object response);
    }
}