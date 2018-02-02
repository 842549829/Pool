#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.Query.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Izual.Linq {
    /// <summary>
    /// Optional interface for IQueryProvider to implement Query <T>'s QueryText property.
    /// </summary>
    public interface IQueryText {
        string GetQueryText(Expression expression);
    }

    /// <summary>
    /// A default implementation of IQueryable for use with QueryProvider
    /// </summary>
    public class Query<T> : IQueryable<T>, IQueryable, IEnumerable<T>, IEnumerable, IOrderedQueryable<T>, IOrderedQueryable {
        private readonly Expression expression;
        private readonly IQueryProvider provider;

        public Query(IQueryProvider provider) : this(provider, null) {}

        public Query(IQueryProvider provider, Type staticType) {
            if(provider == null) {
                throw new ArgumentNullException("Provider");
            }
            this.provider = provider;
            expression = staticType != null ? Expression.Constant(this, staticType) : Expression.Constant(this);
        }

        public Query(QueryProvider provider, Expression expression) {
            if(provider == null) {
                throw new ArgumentNullException("Provider");
            }
            if(expression == null) {
                throw new ArgumentNullException("expression");
            }
            if(!typeof(IQueryable<T>).IsAssignableFrom(expression.Type)) {
                throw new ArgumentOutOfRangeException("expression");
            }
            this.provider = provider;
            this.expression = expression;
        }

        public string QueryText {
            get {
                var iqt = provider as IQueryText;
                if(iqt != null) {
                    return iqt.GetQueryText(expression);
                }
                return "";
            }
        }

        #region IQueryable<T> Members

        public Expression Expression {
            get { return expression; }
        }

        public Type ElementType {
            get { return typeof(T); }
        }

        public IQueryProvider Provider {
            get { return provider; }
        }

        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>)provider.Execute(expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)provider.Execute(expression)).GetEnumerator();
        }

        #endregion

        public override string ToString() {
            if(expression.NodeType == ExpressionType.Constant && ((ConstantExpression)expression).Value == this) {
                return "Query(" + typeof(T) + ")";
            }
            else {
                return expression.ToString();
            }
        }
    }
}