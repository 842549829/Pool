#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.SelectGatherer.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Izual.Data.Common {
    /// <summary>
    /// returns the list of SelectExpressions accessible from the source expression
    /// </summary>
    public class SelectGatherer : DbExpressionVisitor {
        private readonly List<SelectExpression> selects = new List<SelectExpression>();

        public static ReadOnlyCollection<SelectExpression> Gather(Expression expression) {
            var gatherer = new SelectGatherer();
            gatherer.Visit(expression);
            return gatherer.selects.AsReadOnly();
        }

        protected override Expression VisitSelect(SelectExpression select) {
            selects.Add(select);
            return select; // don't visit sub-queries
        }
    }
}