#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.SkipToRowNumberRewriter.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Linq;
using System.Linq.Expressions;
using Izual.Linq;

namespace Izual.Data.Common {
    /// <summary>
    /// Rewrites take & skip expressions into uses of TSQL row_number function
    /// </summary>
    public class SkipToRowNumberRewriter : DbExpressionVisitor {
        private readonly QueryLanguage language;

        private SkipToRowNumberRewriter(QueryLanguage language) {
            this.language = language;
        }

        public static Expression Rewrite(QueryLanguage language, Expression expression) {
            return new SkipToRowNumberRewriter(language).Visit(expression);
        }

        protected override Expression VisitSelect(SelectExpression select) {
            select = (SelectExpression)base.VisitSelect(select);
            if(select.Skip != null) {
                SelectExpression newSelect = select.SetSkip(null).SetTake(null);
                bool canAddColumn = !select.IsDistinct && (select.GroupBy == null || select.GroupBy.Count == 0);
                if(!canAddColumn) {
                    newSelect = newSelect.AddRedundantSelect(language, new TableAlias());
                }
                QueryType colType = language.TypeSystem.GetColumnType(typeof(int));
                newSelect = newSelect.AddColumn(new ColumnDeclaration("_rownum", new RowNumberExpression(select.OrderBy), colType));

                // add layer for WHERE clause that references new rownum column
                newSelect = newSelect.AddRedundantSelect(language, new TableAlias());
                newSelect = newSelect.RemoveColumn(newSelect.Columns.Single(c => c.Name == "_rownum"));

                TableAlias newAlias = ((SelectExpression)newSelect.From).Alias;
                var rnCol = new ColumnExpression(typeof(int), colType, newAlias, "_rownum");
                Expression where;
                if(select.Take != null) {
                    where = new BetweenExpression(rnCol, Expression.Add(select.Skip, Expression.Constant(1)), Expression.Add(select.Skip, select.Take));
                }
                else {
                    where = rnCol.GreaterThan(select.Skip);
                }
                if(newSelect.Where != null) {
                    where = newSelect.Where.And(where);
                }
                newSelect = newSelect.SetWhere(where);

                select = newSelect;
            }
            return select;
        }
    }
}