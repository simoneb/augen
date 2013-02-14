using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace augen
{
	public delegate dynamic Options(Expression<Func<string, dynamic>> expr);

    public abstract class Request<TRequest, TResponse> : Request<TResponse>
	{
        protected Request(Connection connection, params LambdaExpression[] options) : base(connection, options)
        {
        }

        public Request<TRequest, TResponse> Test(string description, Expression<Func<TResponse, object>> checker)
		{
			Add(new Test<TResponse>(description, checker));
            return this;
		}

        private static Expression<Func<object, object>> Convert(Expression<Func<TResponse, object>> input)
		{
			var parameter = Expression.Parameter(typeof(object));
			return Expression.Lambda<Func<object, object>>(Expression.Invoke(input, Expression.Convert(parameter, typeof(TResponse))), parameter);
		}
	}

    public abstract class Request<TResponse> : Request
	{
	    protected Request(Connection connection, IEnumerable<LambdaExpression> options) : base(connection, options)
	    {
	    }

	    protected abstract TResponse Execute(Options options);

        protected override object ExecuteInternal(Options options)
        {
            return Execute(options);
        }
	}

    public abstract class Request
    {
        internal List<Test> Tests { get; set; }
        internal Tuple<string[], Delegate>[] Options { get; private set; }

        protected Request(Connection connection, IEnumerable<LambdaExpression> options)
        {
            Options = ParseOptions(options);
            Tests = new List<Test>();

            connection.Add(this);
        }

        private static Tuple<string[], Delegate>[] ParseOptions(IEnumerable<LambdaExpression> options)
        {
            return options.Select(o => 
                Tuple.Create(o.Parameters.Select(p => p.Name).ToArray(), 
                             Expression.Lambda(
                                Expression.GetFuncType(o.Parameters.Select(p => p.Type).Concat(new[]{o.ReturnType}).ToArray()),
                                o.Body, o.Parameters).Compile())).ToArray();
        }

        internal void Add(Test test)
        {
            Tests.Add(test);
        }

        protected abstract object ExecuteInternal(Options options);
    }
}