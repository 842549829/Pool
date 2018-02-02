#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.TypedSubtreeFinder.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Linq.Expressions;

namespace Izual.Linq {
    /// <summary>
    /// Finds the first sub-expression that is of a specified type
    /// </summary>
    public class TypedSubtreeFinder : ExpressionVisitor {
        private readonly Type type;
        private Expression root;

        private TypedSubtreeFinder(Type type) {
            this.type = type;
        }

        public static Expression Find(Expression expression, Type type) {
            var finder = new TypedSubtreeFinder(type);
            finder.Visit(expression);
            return finder.root;
        }

        protected override Expression Visit(Expression exp) {
            Expression result = base.Visit(exp);

            // remember the first sub-expression that produces an IQueryable
            if(root == null && result != null) {
                if(type.IsAssignableFrom(result.Type))
                    root = result;
            }

            return result;
        }
    }
}