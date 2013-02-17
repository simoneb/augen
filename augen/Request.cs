using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace augen
{
	public abstract class Request : OptionsHolder
	{
		internal List<Test> Tests { get; private set; }

		internal Request(Connection connection, IEnumerable<Expression<Func<string, Func<object, object>>>> options) : base(options)
		{
			Tests = new List<Test>();
			connection.Add(this);
		}

		internal Request(Connection connection, IEnumerable<Expression<Func<string, object>>> options) : base(options)
		{
			Tests = new List<Test>();
			connection.Add(this);
		}

		protected Request(Connection connection) 
		{
			Tests = new List<Test>();
			connection.Add(this);
		}

		internal void Add(Test test)
		{
			Tests.Add(test);
		}

		internal abstract object ExecuteInternal(object connection, object options);

		internal abstract void CloseInternal(object response);
	}

	public abstract class Request<TResponse> : Request
	{
		internal Request(Connection connection, IEnumerable<Expression<Func<string, Func<object, object>>>> options) : base(connection, options)
		{
		}

		internal Request(Connection connection, IEnumerable<Expression<Func<string, object>>> options) : base(connection, options)
		{
		}

		protected Request(Connection connection) : base(connection)
		{
		}

		internal abstract TResponse ExecuteInternal2(object connection, object options);

	    internal override object ExecuteInternal(object connection, dynamic options)
        {
            return ExecuteInternal2(connection, options);
        }

		internal override void CloseInternal(object response)
		{
			Close((TResponse) response);
		}

	    protected abstract void Close(TResponse response);
	}

	public abstract class Request<TParent, TSelf, TConnection, TResponse> : Request<TResponse> where TSelf : Request<TParent, TSelf, TConnection, TResponse> where TParent : Connection<TParent, TConnection>
	{
		private readonly TParent _connection;

		protected Request(TParent connection) : base(connection)
		{
			_connection = connection;
		}

		protected Request(TParent connection, params Expression<Func<string, object>>[] options) : base(connection, options)
		{
			_connection = connection;
		}

		protected Request(TParent connection, params Expression<Func<string, Func<object, object>>>[] options) : base(connection, options)
		{
			_connection = connection;
		}

		protected abstract TResponse Execute(TConnection connection, dynamic options);

		public TSelf Test(string description, Expression<Func<TResponse, object>> checker)
		{
			Add(new Test(description, Convert(checker)));
			return (TSelf) this;
		}

		public TParent End()
		{
			return _connection;
		}

		private static Expression<Func<object, object>> Convert(Expression<Func<TResponse, object>> input)
		{
			var parameter = Expression.Parameter(typeof(object));
			return Expression.Lambda<Func<object, object>>(Expression.Invoke(input, Expression.Convert(parameter, typeof(TResponse))), parameter);
		}

		internal override TResponse ExecuteInternal2(object connection, dynamic options)
		{
			return Execute((TConnection) connection, options);
		}
	}
}