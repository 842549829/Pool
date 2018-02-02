#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.ColumnProjector.cs
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
    /// <summary>
    /// Result from calling ColumnProjector.ProjectColumns
    /// </summary>
    public sealed class ProjectedColumns {
        private readonly ReadOnlyCollection<ColumnDeclaration> columns;
        private readonly Expression projector;

        public ProjectedColumns(Expression projector, ReadOnlyCollection<ColumnDeclaration> columns) {
            this.projector = projector;
            this.columns = columns;
        }

        public Expression Projector {
            get { return projector; }
        }

        public ReadOnlyCollection<ColumnDeclaration> Columns {
            get { return columns; }
        }
    }

    /// <summary>
    /// Splits an expression into two parts 1) a list of column declarations for sub-expressions that must be evaluated on the server 2) a expression that describes how to combine/project the columns back together into the correct result
    /// </summary>
    public class ColumnProjector : DbExpressionVisitor {
        private readonly HashSet<Expression> candidates;
        private readonly HashSet<string> columnNames;
        private readonly List<ColumnDeclaration> columns;
        private readonly HashSet<TableAlias> existingAliases;
        private readonly QueryLanguage language;
        private readonly Dictionary<ColumnExpression, ColumnExpression> map;
        private readonly TableAlias newAlias;
        private int iColumn;

        private ColumnProjector(QueryLanguage language, Expression expression, IEnumerable<ColumnDeclaration> existingColumns, TableAlias newAlias, IEnumerable<TableAlias> existingAliases) {
            this.language = language;
            this.newAlias = newAlias;
            this.existingAliases = new HashSet<TableAlias>(existingAliases);
            map = new Dictionary<ColumnExpression, ColumnExpression>();
            if(existingColumns != null) {
                columns = new List<ColumnDeclaration>(existingColumns);
                columnNames = new HashSet<string>(existingColumns.Select(c => c.Name));
            }
            else {
                columns = new List<ColumnDeclaration>();
                columnNames = new HashSet<string>();
            }
            candidates = Nominator.Nominate(language, expression);
        }

        public static ProjectedColumns ProjectColumns(QueryLanguage language, Expression expression, IEnumerable<ColumnDeclaration> existingColumns, TableAlias newAlias,
                                                      IEnumerable<TableAlias> existingAliases) {
            var projector = new ColumnProjector(language, expression, existingColumns, newAlias, existingAliases);
            Expression expr = projector.Visit(expression);
            return new ProjectedColumns(expr, projector.columns.AsReadOnly());
        }

        public static ProjectedColumns ProjectColumns(QueryLanguage language, Expression expression, IEnumerable<ColumnDeclaration> existingColumns, TableAlias newAlias,
                                                      params TableAlias[] existingAliases) {
            return ProjectColumns(language, expression, existingColumns, newAlias, (IEnumerable<TableAlias>)existingAliases);
        }

        protected override Expression Visit(Expression expression) {
            if(candidates.Contains(expression)) {
                if(expression.NodeType == (ExpressionType)DbExpressionType.Column) {
                    var column = (ColumnExpression)expression;
                    ColumnExpression mapped;
                    if(map.TryGetValue(column, out mapped)) {
                        return mapped;
                    }
                    // check for column that already refers to this column
                    foreach(ColumnDeclaration existingColumn in columns) {
                        var cex = existingColumn.Expression as ColumnExpression;
                        if(cex != null && cex.Alias == column.Alias && cex.Name == column.Name) {
                            // refer to the column already in the column list
                            return new ColumnExpression(column.Type, column.QueryType, newAlias, existingColumn.Name);
                        }
                    }
                    if(existingAliases.Contains(column.Alias)) {
                        int ordinal = columns.Count;
                        string columnName = GetUniqueColumnName(column.Name);
                        columns.Add(new ColumnDeclaration(columnName, column, column.QueryType));
                        mapped = new ColumnExpression(column.Type, column.QueryType, newAlias, columnName);
                        map.Add(column, mapped);
                        columnNames.Add(columnName);
                        return mapped;
                    }
                    // must be referring to outer scope
                    return column;
                }
                else {
                    string columnName = GetNextColumnName();
                    QueryType colType = language.TypeSystem.GetColumnType(expression.Type);
                    columns.Add(new ColumnDeclaration(columnName, expression, colType));
                    return new ColumnExpression(expression.Type, colType, newAlias, columnName);
                }
            }
            else {
                return base.Visit(expression);
            }
        }

        private bool IsColumnNameInUse(string name) {
            return columnNames.Contains(name);
        }

        private string GetUniqueColumnName(string name) {
            string baseName = name;
            int suffix = 1;
            while(IsColumnNameInUse(name)) {
                name = baseName + (suffix++);
            }
            return name;
        }

        private string GetNextColumnName() {
            return GetUniqueColumnName("c" + (iColumn++));
        }

        #region Nested type: Nominator

        /// <summary>
        /// Nominator is a class that walks an expression tree bottom up, determining the set of candidate expressions that are possible columns of a select expression
        /// </summary>
        private class Nominator : DbExpressionVisitor {
            private readonly HashSet<Expression> candidates;
            private readonly QueryLanguage language;
            private bool isBlocked;

            private Nominator(QueryLanguage language) {
                this.language = language;
                candidates = new HashSet<Expression>();
                isBlocked = false;
            }

            internal static HashSet<Expression> Nominate(QueryLanguage language, Expression expression) {
                var nominator = new Nominator(language);
                nominator.Visit(expression);
                return nominator.candidates;
            }

            protected override Expression Visit(Expression expression) {
                if(expression != null) {
                    bool saveIsBlocked = isBlocked;
                    isBlocked = false;
                    if(language.MustBeColumn(expression)) {
                        candidates.Add(expression);
                        // don't merge saveIsBlocked
                    }
                    else {
                        base.Visit(expression);
                        if(!isBlocked) {
                            if(language.CanBeColumn(expression)) {
                                candidates.Add(expression);
                            }
                            else {
                                isBlocked = true;
                            }
                        }
                        isBlocked |= saveIsBlocked;
                    }
                }
                return expression;
            }

            protected override Expression VisitProjection(ProjectionExpression proj) {
                Visit(proj.Projector);
                return proj;
            }
        }

        #endregion
    }
}