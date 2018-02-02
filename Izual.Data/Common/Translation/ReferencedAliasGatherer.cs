#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.ReferencedAliasGatherer.cs
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
    /// returns the set of all aliases produced by a query source
    /// </summary>
    public class ReferencedAliasGatherer : DbExpressionVisitor {
        private readonly HashSet<TableAlias> aliases;

        private ReferencedAliasGatherer() {
            aliases = new HashSet<TableAlias>();
        }

        public static HashSet<TableAlias> Gather(Expression source) {
            var gatherer = new ReferencedAliasGatherer();
            gatherer.Visit(source);
            return gatherer.aliases;
        }

        protected override Expression VisitColumn(ColumnExpression column) {
            aliases.Add(column.Alias);
            return column;
        }
    }
}