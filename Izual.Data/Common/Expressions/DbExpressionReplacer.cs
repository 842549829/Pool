#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.DbExpressionReplacer.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Linq.Expressions;

namespace Izual.Data.Common {
    /// <summary>
    /// Replaces references to one specific instance of an expression node with another node. Supports DbExpression nodes
    /// </summary>
    public class DbExpressionReplacer : DbExpressionVisitor {
        private readonly Expression replaceWith;
        private readonly Expression searchFor;

        private DbExpressionReplacer(Expression searchFor, Expression replaceWith) {
            this.searchFor = searchFor;
            this.replaceWith = replaceWith;
        }

        public static Expression Replace(Expression expression, Expression searchFor, Expression replaceWith) {
            return new DbExpressionReplacer(searchFor, replaceWith).Visit(expression);
        }

        public static Expression ReplaceAll(Expression expression, Expression[] searchFor, Expression[] replaceWith) {
            for(int i = 0, n = searchFor.Length; i < n; i++) {
                expression = Replace(expression, searchFor[i], replaceWith[i]);
            }
            return expression;
        }

        protected override Expression Visit(Expression exp) {
            if(exp == searchFor) {
                return replaceWith;
            }
            return base.Visit(exp);
        }
    }
}