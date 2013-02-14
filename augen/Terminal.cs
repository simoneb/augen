using System;
using System.Linq.Expressions;
using System.Linq;

namespace augen
{
	public delegate dynamic Options(Expression<Func<string, dynamic>> expr);

	public static class Ext
	{
		public static dynamic Throw(this Options options)
		{
			throw new InvalidOperationException(string.Format("Option value not available for {0}", options.Target));
		}
	}

	public abstract class Terminal<T> : Terminal
	{
		private readonly Project _project;

		protected Terminal(Project project)
		{
			_project = project;
		}

		public Terminal<T> Test(string description, Expression<Func<T, object>> checker)
		{
			_project.Add(new TestDescriptor(this, description, Convert(checker)));
			return this;
		}

		private static Expression<Func<object, object>> Convert(Expression<Func<T, object>> input)
		{
			var parameter = Expression.Parameter(typeof(object));
			return Expression.Lambda<Func<object, object>>(Expression.Invoke(input, Expression.Convert(parameter, typeof(T))), parameter);
		}

		protected abstract T Execute(string serverName, Options options);

		public override object ExecuteInternal(string serverName, Options options)
		{
			return Execute(serverName, options);
		}
	}

	public abstract class Terminal
	{
		public abstract object ExecuteInternal(string serverName, Options options);
	}
}