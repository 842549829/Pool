#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.AggregateRewriter.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Izual.Data.Common {
    /// <summary>
    /// Rewrite aggregate expressions, moving them into same select expression that has the group-by clause
    /// </summary>
    public class AggregateRewriter : DbExpressionVisitor {
        private readonly QueryLanguage language;
        private readonly ILookup<TableAlias, AggregateSubqueryExpression> lookup;
        private readonly Dictionary<AggregateSubqueryExpression, Expression> map;

        private AggregateRewriter(QueryLanguage language, Expression expr) {
            this.language = language;
            map = new Dictionary<AggregateSubqueryExpression, Expression>();
            lookup = AggregateGatherer.Gather(expr).ToLookup(a => a.GroupByAlias);
        }

        public static Expression Rewrite(QueryLanguage language, Expression expr) {
            return new AggregateRewriter(language, expr).Visit(expr);
        }

        protected override Expression VisitSelect(SelectExpression select) {
            select = (SelectExpression)base.VisitSelect(select);
            if(lookup.Contains(select.Alias)) {
                var aggColumns = new List<ColumnDeclaration>(select.Columns);
                foreach(AggregateSubqueryExpression ae in lookup[select.Alias]) {
                    string name = "agg" + aggColumns.Count;
                    QueryType colType = language.TypeSystem.GetColumnType(ae.Type);
                    var cd = new ColumnDeclaration(name, ae.AggregateInGroupSelect, colType);
                    map.Add(ae, new ColumnExpression(ae.Type, colType, ae.GroupByAlias, name));
                    aggColumns.Add(cd);
                }
                return new SelectExpression(select.Alias, aggColumns, select.From, select.Where, select.OrderBy, select.GroupBy, select.IsDistinct, select.Skip, select.Take, select.IsReverse);
            }
            return select;
        }

        protected override Expression VisitAggregateSubquery(AggregateSubqueryExpression aggregate) {
            Expression mapped;
            if(map.TryGetValue(aggregate, out mapped)) {
                return mapped;
            }
            return Visit(aggregate.AggregateAsSubquery);
        }

        #region Nested type: AggregateGatherer

        private class AggregateGatherer : DbExpressionVisitor {
            private readonly List<AggregateSubqueryExpression> aggregates = new List<AggregateSubqueryExpression>();
            private AggregateGatherer() {}

            internal static List<AggregateSubqueryExpression> Gather(Expression expression) {
                var gatherer = new AggregateGatherer();
                gatherer.Visit(expression);
                return gatherer.aggregates;
            }

            protected override Expression VisitAggregateSubquery(AggregateSubqueryExpression aggregate) {
                aggregates.Add(aggregate);
                return base.VisitAggregateSubquery(aggregate);
            }
        }

        #endregion
    }
}