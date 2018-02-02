#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.AggregateChecker.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Linq.Expressions;

namespace Izual.Data.Common {
    /// <summary>
    /// Determines if a SelectExpression contains any aggregate expressions
    /// </summary>
    internal class AggregateChecker : DbExpressionVisitor {
        private bool hasAggregate;
        private AggregateChecker() {}

        internal static bool HasAggregates(SelectExpression expression) {
            var checker = new AggregateChecker();
            checker.Visit(expression);
            return checker.hasAggregate;
        }

        protected override Expression VisitAggregate(AggregateExpression aggregate) {
            hasAggregate = true;
            return aggregate;
        }

        protected override Expression VisitSelect(SelectExpression select) {
            // only consider aggregates in these locations
            Visit(select.Where);
            VisitOrderBy(select.OrderBy);
            VisitColumnDeclarations(select.Columns);
            return select;
        }

        protected override Expression VisitSubquery(SubqueryExpression subquery) {
            // don't count aggregates in subqueries
            return subquery;
        }
    }
}