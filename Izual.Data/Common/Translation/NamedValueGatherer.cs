#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.NamedValueGatherer.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Izual.Data.Common {
    public class NamedValueGatherer : DbExpressionVisitor {
        private readonly HashSet<NamedValueExpression> namedValues = new HashSet<NamedValueExpression>(new NamedValueComparer());

        private NamedValueGatherer() {}

        public static ReadOnlyCollection<NamedValueExpression> Gather(Expression expr) {
            var gatherer = new NamedValueGatherer();
            gatherer.Visit(expr);
            return gatherer.namedValues.ToList().AsReadOnly();
        }

        protected override Expression VisitNamedValue(NamedValueExpression value) {
            namedValues.Add(value);
            return value;
        }

        #region Nested type: NamedValueComparer

        private class NamedValueComparer : IEqualityComparer<NamedValueExpression> {
            #region IEqualityComparer<NamedValueExpression> Members

            public bool Equals(NamedValueExpression x, NamedValueExpression y) {
                return x.Name == y.Name;
            }

            public int GetHashCode(NamedValueExpression obj) {
                return obj.Name.GetHashCode();
            }

            #endregion
        }

        #endregion
    }
}