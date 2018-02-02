#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.QueryProvider.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Izual.Linq {
    /// <summary>
    /// A basic abstract LINQ query provider
    /// </summary>
    public abstract class QueryProvider : IQueryProvider, IQueryText {
        #region IQueryProvider Members

        IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression) {
            return new Query<S>(this, expression);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression) {
            Type elementType = TypeHelper.GetElementType(expression.Type);
            try {
                return (IQueryable)Activator.CreateInstance(typeof(Query<>).MakeGenericType(elementType), new object[] {this, expression});
            }
            catch(TargetInvocationException tie) {
                throw tie.InnerException;
            }
        }

        S IQueryProvider.Execute<S>(Expression expression) {
            return (S)Execute(expression);
        }

        object IQueryProvider.Execute(Expression expression) {
            return Execute(expression);
        }

        #endregion

        #region IQueryText Members

        public abstract string GetQueryText(Expression expression);

        #endregion

        public abstract object Execute(Expression expression);
    }
}