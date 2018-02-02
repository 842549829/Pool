#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.RelationshipBinder.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Izual.Data.Common.Translation {
    /// <summary>
    /// Translates accesses to relationship members into projections or joins
    /// </summary>
    public class RelationshipBinder : DbExpressionVisitor {
        private readonly QueryLanguage language;
        private readonly QueryMapper mapper;
        private readonly QueryMapping mapping;
        private Expression currentFrom;

        private RelationshipBinder(QueryMapper mapper) {
            this.mapper = mapper;
            mapping = mapper.Mapping;
            language = mapper.Translator.Linguist.Language;
        }

        public static Expression Bind(QueryMapper mapper, Expression expression) {
            return new RelationshipBinder(mapper).Visit(expression);
        }

        protected override Expression VisitSelect(SelectExpression select) {
            Expression saveCurrentFrom = currentFrom;
            currentFrom = VisitSource(select.From);
            try {
                Expression where = Visit(select.Where);
                ReadOnlyCollection<OrderExpression> orderBy = VisitOrderBy(select.OrderBy);
                ReadOnlyCollection<Expression> groupBy = VisitExpressionList(select.GroupBy);
                Expression skip = Visit(select.Skip);
                Expression take = Visit(select.Take);
                ReadOnlyCollection<ColumnDeclaration> columns = VisitColumnDeclarations(select.Columns);
                if(currentFrom != select.From || where != select.Where || orderBy != select.OrderBy || groupBy != select.GroupBy || take != select.Take || skip != select.Skip ||
                   columns != select.Columns) {
                    return new SelectExpression(select.Alias, columns, currentFrom, where, orderBy, groupBy, select.IsDistinct, skip, take, select.IsReverse);
                }
                return select;
            }
            finally {
                currentFrom = saveCurrentFrom;
            }
        }

        protected override Expression VisitMemberAccess(MemberExpression m) {
            Expression source = Visit(m.Expression);
            var ex = source as EntryExpression;

            if(ex != null && mapping.IsRelationship(ex.Entry, m.Member)) {
                var projection = (ProjectionExpression)Visit(mapper.GetMemberExpression(source, ex.Entry, m.Member));
                if(currentFrom != null && mapping.IsSingletonRelationship(ex.Entry, m.Member)) {
                    // convert singleton associations directly to OUTER APPLY
                    projection = language.AddOuterJoinTest(projection);
                    Expression newFrom = new JoinExpression(JoinType.OuterApply, currentFrom, projection.Select, null);
                    currentFrom = newFrom;
                    return projection.Projector;
                }
                return projection;
            }
            else {
                Expression result = QueryBinder.BindMember(source, m.Member);
                var mex = result as MemberExpression;
                if(mex != null && mex.Member == m.Member && mex.Expression == m.Expression) {
                    return m;
                }
                return result;
            }
        }
    }
}