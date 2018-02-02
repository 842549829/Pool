#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.SkipToNestedOrderByRewriter.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Izual.Linq;

namespace Izual.Data.Common {
    /// <summary>
    /// Rewrites queries with skip & take to use the nested queries with inverted ordering technique
    /// </summary>
    public class SkipToNestedOrderByRewriter : DbExpressionVisitor {
        private readonly QueryLanguage language;

        private SkipToNestedOrderByRewriter(QueryLanguage language) {
            this.language = language;
        }

        public static Expression Rewrite(QueryLanguage language, Expression expression) {
            return new SkipToNestedOrderByRewriter(language).Visit(expression);
        }

        protected override Expression VisitSelect(SelectExpression select) {
            // select * from table order by x skip s take t 
            // =>
            // select * from (select top s * from (select top s + t from table order by x) order by -x) order by x

            select = (SelectExpression)base.VisitSelect(select);

            if(select.Skip != null && select.Take != null && select.OrderBy.Count > 0) {
                Expression skip = select.Skip;
                Expression take = select.Take;
                Expression skipPlusTake = PartialEvaluator.Eval(Expression.Add(skip, take));

                select = select.SetTake(skipPlusTake).SetSkip(null);
                select = select.AddRedundantSelect(language, new TableAlias());
                select = select.SetTake(take);

                // propogate order-bys to new layer
                select = (SelectExpression)OrderByRewriter.Rewrite(language, select);
                IEnumerable<OrderExpression> inverted = select.OrderBy.Select(ob => new OrderExpression(ob.OrderType == OrderType.Ascending ? OrderType.Descending : OrderType.Ascending, ob.Expression));
                select = select.SetOrderBy(inverted);

                select = select.AddRedundantSelect(language, new TableAlias());
                select = select.SetTake(Expression.Constant(0)); // temporary
                select = (SelectExpression)OrderByRewriter.Rewrite(language, select);
                IEnumerable<OrderExpression> reverted = select.OrderBy.Select(ob => new OrderExpression(ob.OrderType == OrderType.Ascending ? OrderType.Descending : OrderType.Ascending, ob.Expression));
                select = select.SetOrderBy(reverted);
                select = select.SetTake(null);
            }

            return select;
        }
    }
}