#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.DeclaredAliasGatherer.cs
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
    public class DeclaredAliasGatherer : DbExpressionVisitor {
        private readonly HashSet<TableAlias> aliases;

        private DeclaredAliasGatherer() {
            aliases = new HashSet<TableAlias>();
        }

        public static HashSet<TableAlias> Gather(Expression source) {
            var gatherer = new DeclaredAliasGatherer();
            gatherer.Visit(source);
            return gatherer.aliases;
        }

        protected override Expression VisitSelect(SelectExpression select) {
            aliases.Add(select.Alias);
            return select;
        }

        protected override Expression VisitTable(TableExpression table) {
            aliases.Add(table.Alias);
            return table;
        }
    }
}