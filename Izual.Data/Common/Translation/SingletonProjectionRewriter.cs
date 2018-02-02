#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.SingletonProjectionRewriter.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Linq.Expressions;

namespace Izual.Data.Common {
    /// <summary>
    /// Rewrites nested singleton projection into server-side joins
    /// </summary>
    public class SingletonProjectionRewriter : DbExpressionVisitor {
        private readonly QueryLanguage language;
        private SelectExpression currentSelect;
        private bool isTopLevel = true;

        private SingletonProjectionRewriter(QueryLanguage language) {
            this.language = language;
        }

        public static Expression Rewrite(QueryLanguage language, Expression expression) {
            return new SingletonProjectionRewriter(language).Visit(expression);
        }

        protected override Expression VisitClientJoin(ClientJoinExpression join) {
            // treat client joins as new top level
            bool saveTop = isTopLevel;
            SelectExpression saveSelect = currentSelect;
            isTopLevel = true;
            currentSelect = null;
            Expression result = base.VisitClientJoin(join);
            isTopLevel = saveTop;
            currentSelect = saveSelect;
            return result;
        }

        protected override Expression VisitProjection(ProjectionExpression proj) {
            if(isTopLevel) {
                isTopLevel = false;
                currentSelect = proj.Select;
                Expression projector = Visit(proj.Projector);
                if(projector != proj.Projector || currentSelect != proj.Select) {
                    return new ProjectionExpression(currentSelect, projector, proj.Aggregator);
                }
                return proj;
            }

            if(proj.IsSingleton && CanJoinOnServer(currentSelect)) {
                var newAlias = new TableAlias();
                currentSelect = currentSelect.AddRedundantSelect(language, newAlias);

                // remap any references to the outer select to the new alias;
                var source = (SelectExpression)ColumnMapper.Map(proj.Select, newAlias, currentSelect.Alias);

                // add outer-join test
                ProjectionExpression pex = language.AddOuterJoinTest(new ProjectionExpression(source, proj.Projector));

                ProjectedColumns pc = ColumnProjector.ProjectColumns(language, pex.Projector, currentSelect.Columns, currentSelect.Alias, newAlias, proj.Select.Alias);

                var join = new JoinExpression(JoinType.OuterApply, currentSelect.From, pex.Select, null);

                currentSelect = new SelectExpression(currentSelect.Alias, pc.Columns, join, null);
                return Visit(pc.Projector);
            }

            bool saveTop = isTopLevel;
            SelectExpression saveSelect = currentSelect;
            isTopLevel = true;
            currentSelect = null;
            Expression result = base.VisitProjection(proj);
            isTopLevel = saveTop;
            currentSelect = saveSelect;
            return result;
        }

        private bool CanJoinOnServer(SelectExpression select) {
            // can add singleton (1:0,1) join if no grouping/aggregates or distinct
            return !select.IsDistinct && (select.GroupBy == null || select.GroupBy.Count == 0) && !AggregateChecker.HasAggregates(select);
        }

        protected override Expression VisitSubquery(SubqueryExpression subquery) {
            return subquery;
        }

        protected override Expression VisitCommand(CommandExpression command) {
            isTopLevel = true;
            return base.VisitCommand(command);
        }
    }
}