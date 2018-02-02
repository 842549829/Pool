#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.ColumnMapper.cs
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
    /// Rewrite all column references to one or more aliases to a new single alias
    /// </summary>
    public class ColumnMapper : DbExpressionVisitor {
        private readonly TableAlias newAlias;
        private readonly HashSet<TableAlias> oldAliases;

        private ColumnMapper(IEnumerable<TableAlias> oldAliases, TableAlias newAlias) {
            this.oldAliases = new HashSet<TableAlias>(oldAliases);
            this.newAlias = newAlias;
        }

        public static Expression Map(Expression expression, TableAlias newAlias, IEnumerable<TableAlias> oldAliases) {
            return new ColumnMapper(oldAliases, newAlias).Visit(expression);
        }

        public static Expression Map(Expression expression, TableAlias newAlias, params TableAlias[] oldAliases) {
            return Map(expression, newAlias, (IEnumerable<TableAlias>)oldAliases);
        }

        protected override Expression VisitColumn(ColumnExpression column) {
            if(oldAliases.Contains(column.Alias)) {
                return new ColumnExpression(column.Type, column.QueryType, newAlias, column.Name);
            }
            return column;
        }
    }
}