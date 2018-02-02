#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.CrossJoinIsolator.cs
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
    /// Isolates cross joins from other types of joins using nested sub queries
    /// </summary>
    public class CrossJoinIsolator : DbExpressionVisitor {
        private readonly Dictionary<ColumnExpression, ColumnExpression> map = new Dictionary<ColumnExpression, ColumnExpression>();
        private ILookup<TableAlias, ColumnExpression> columns;
        private JoinType? lastJoin;

        public static Expression Isolate(Expression expression) {
            return new CrossJoinIsolator().Visit(expression);
        }

        protected override Expression VisitSelect(SelectExpression select) {
            ILookup<TableAlias, ColumnExpression> saveColumns = columns;
            columns = ReferencedColumnGatherer.Gather(select).ToLookup(c => c.Alias);
            JoinType? saveLastJoin = lastJoin;
            lastJoin = null;
            Expression result = base.VisitSelect(select);
            columns = saveColumns;
            lastJoin = saveLastJoin;
            return result;
        }

        protected override Expression VisitJoin(JoinExpression join) {
            JoinType? saveLastJoin = lastJoin;
            lastJoin = join.Join;
            join = (JoinExpression)base.VisitJoin(join);
            lastJoin = saveLastJoin;

            if(lastJoin != null && (join.Join == JoinType.CrossJoin) != (lastJoin == JoinType.CrossJoin)) {
                Expression result = MakeSubquery(join);
                return result;
            }
            return join;
        }

        private bool IsCrossJoin(Expression expression) {
            var jex = expression as JoinExpression;
            if(jex != null) {
                return jex.Join == JoinType.CrossJoin;
            }
            return false;
        }

        private Expression MakeSubquery(Expression expression) {
            var newAlias = new TableAlias();
            HashSet<TableAlias> aliases = DeclaredAliasGatherer.Gather(expression);

            var decls = new List<ColumnDeclaration>();
            foreach(TableAlias ta in aliases) {
                foreach(ColumnExpression col in columns[ta]) {
                    string name = decls.GetAvailableColumnName(col.Name);
                    var decl = new ColumnDeclaration(name, col, col.QueryType);
                    decls.Add(decl);
                    var newCol = new ColumnExpression(col.Type, col.QueryType, newAlias, col.Name);
                    map.Add(col, newCol);
                }
            }

            return new SelectExpression(newAlias, decls, expression, null);
        }

        protected override Expression VisitColumn(ColumnExpression column) {
            ColumnExpression mapped;
            if(map.TryGetValue(column, out mapped)) {
                return mapped;
            }
            return column;
        }
    }
}