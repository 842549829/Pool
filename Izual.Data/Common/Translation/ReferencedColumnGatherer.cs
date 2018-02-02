#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.ReferencedColumnGatherer.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Collections.Generic;
using System.Linq.Expressions;

namespace Izual.Data.Common {
    /// <summary>
    /// Gathers all columns referenced by the given expression
    /// </summary>
    public class ReferencedColumnGatherer : DbExpressionVisitor {
        private readonly HashSet<ColumnExpression> columns = new HashSet<ColumnExpression>();
        private bool first = true;

        public static HashSet<ColumnExpression> Gather(Expression expression) {
            var visitor = new ReferencedColumnGatherer();
            visitor.Visit(expression);
            return visitor.columns;
        }

        protected override Expression VisitColumn(ColumnExpression column) {
            columns.Add(column);
            return column;
        }

        protected override Expression VisitSelect(SelectExpression select) {
            if(first) {
                first = false;
                return base.VisitSelect(select);
            }
            return select;
        }
    }
}