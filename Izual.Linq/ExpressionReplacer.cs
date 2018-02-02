#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.ExpressionReplacer.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Linq.Expressions;

namespace Izual.Linq {
    /// <summary>
    /// Replaces references to one specific instance of an expression node with another node
    /// </summary>
    public class ExpressionReplacer : ExpressionVisitor {
        private readonly Expression replaceWith;
        private readonly Expression searchFor;

        private ExpressionReplacer(Expression searchFor, Expression replaceWith) {
            this.searchFor = searchFor;
            this.replaceWith = replaceWith;
        }

        public static Expression Replace(Expression expression, Expression searchFor, Expression replaceWith) {
            return new ExpressionReplacer(searchFor, replaceWith).Visit(expression);
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