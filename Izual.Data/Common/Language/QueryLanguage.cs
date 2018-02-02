﻿#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.QueryLanguage.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Izual.Linq;

namespace Izual.Data.Common {
    /// <summary>
    /// Defines the language rules for the query provider
    /// </summary>
    public abstract class QueryLanguage {
        public abstract QueryTypeResolver TypeSystem{ get; }

        public virtual bool AllowsMultipleCommands {
            get { return false; }
        }

        public virtual bool AllowSubqueryInSelectWithoutFrom {
            get { return false; }
        }

        public virtual bool AllowDistinctInAggregates {
            get { return false; }
        }

        public abstract Expression GetGeneratedIdExpression(MemberInfo member);

        public virtual string Quote(string name) {
            return name;
        }

        public virtual Expression GetRowsAffectedExpression(Expression command) {
            return new FunctionExpression(typeof(int), "@@ROWCOUNT", null);
        }

        public virtual bool IsRowsAffectedExpressions(Expression expression) {
            var fex = expression as FunctionExpression;
            return fex != null && fex.Name == "@@ROWCOUNT";
        }

        public virtual Expression GetOuterJoinTest(SelectExpression select) {
            // if the column is used in the join condition (equality test)
            // if it is null in the database then the join test won't match (null != null) so the row won't appear
            // we can safely use this existing column as our test to determine if the outer join produced a row

            // find a column that is used in equality test
            HashSet<TableAlias> aliases = DeclaredAliasGatherer.Gather(select.From);
            List<ColumnExpression> joinColumns = JoinColumnGatherer.Gather(aliases, select).ToList();
            if(joinColumns.Count > 0) {
                // prefer one that is already in the projection list.
                foreach(ColumnExpression jc in joinColumns) {
                    foreach(ColumnDeclaration col in select.Columns) {
                        if(jc.Equals(col.Expression)) {
                            return jc;
                        }
                    }
                }
                return joinColumns[0];
            }

            // fall back to introducing a constant
            return Expression.Constant(1, typeof(int?));
        }

        public virtual ProjectionExpression AddOuterJoinTest(ProjectionExpression proj) {
            Expression test = GetOuterJoinTest(proj.Select);
            SelectExpression select = proj.Select;
            ColumnExpression testCol = null;
            // look to see if test expression exists in columns already
            foreach(ColumnDeclaration col in select.Columns) {
                if(test.Equals(col.Expression)) {
                    QueryType colType = TypeSystem.GetColumnType(test.Type);
                    testCol = new ColumnExpression(test.Type, colType, select.Alias, col.Name);
                    break;
                }
            }
            if(testCol == null) {
                // add expression to projection
                testCol = test as ColumnExpression;
                string colName = (testCol != null) ? testCol.Name : "Test";
                colName = proj.Select.Columns.GetAvailableColumnName(colName);
                QueryType colType = TypeSystem.GetColumnType(test.Type);
                select = select.AddColumn(new ColumnDeclaration(colName, test, colType));
                testCol = new ColumnExpression(test.Type, colType, select.Alias, colName);
            }
            var newProjector = new OuterJoinedExpression(testCol, proj.Projector);
            return new ProjectionExpression(select, newProjector, proj.Aggregator);
        }

        /// <summary>
        /// Determines whether the CLR type corresponds to a scalar data type in the query language
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public virtual bool IsScalar(Type type) {
            type = TypeHelper.GetNonNullableType(type);
            switch(Type.GetTypeCode(type)) {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return false;
                case TypeCode.Object:
                    return  type == typeof(DateTimeOffset) || type == typeof(TimeSpan) || type == typeof(Guid) || type == typeof(byte[]);
                default:
                    return true;
            }
        }

        public virtual bool IsAggregate(MemberInfo member) {
            var method = member as MethodInfo;
            if(method != null) {
                if(method.DeclaringType == typeof(Queryable) || method.DeclaringType == typeof(Enumerable)) {
                    switch(method.Name) {
                        case "Count":
                        case "LongCount":
                        case "Sum":
                        case "Min":
                        case "Max":
                        case "Average":
                            return true;
                    }
                }
            }
            var property = member as PropertyInfo;
            if(property != null && property.Name == "Count" && typeof(IEnumerable).IsAssignableFrom(property.DeclaringType)) {
                return true;
            }
            return false;
        }

        public virtual bool AggregateArgumentIsPredicate(string aggregateName) {
            return aggregateName == "Count" || aggregateName == "LongCount";
        }

        /// <summary>
        /// Determines whether the given expression can be represented as a column in a select expressionss
        /// </summary>
        /// <param name="expression"> </param>
        /// <returns> </returns>
        public virtual bool CanBeColumn(Expression expression) {
            // by default, push all work in projection to client
            return MustBeColumn(expression);
        }

        public virtual bool MustBeColumn(Expression expression) {
            switch(expression.NodeType) {
                case (ExpressionType)DbExpressionType.Column:
                case (ExpressionType)DbExpressionType.Scalar:
                case (ExpressionType)DbExpressionType.Exists:
                case (ExpressionType)DbExpressionType.AggregateSubquery:
                case (ExpressionType)DbExpressionType.Aggregate:
                    return true;
                default:
                    return false;
            }
        }

        public virtual QueryLinguist CreateLinguist(QueryTranslator translator) {
            return new QueryLinguist(this, translator);
        }

        #region Nested type: JoinColumnGatherer

        private class JoinColumnGatherer {
            private readonly HashSet<TableAlias> aliases;
            private readonly HashSet<ColumnExpression> columns = new HashSet<ColumnExpression>();

            private JoinColumnGatherer(HashSet<TableAlias> aliases) {
                this.aliases = aliases;
            }

            public static HashSet<ColumnExpression> Gather(HashSet<TableAlias> aliases, SelectExpression select) {
                var gatherer = new JoinColumnGatherer(aliases);
                gatherer.Gather(select.Where);
                return gatherer.columns;
            }

            private void Gather(Expression expression) {
                var b = expression as BinaryExpression;
                if(b != null) {
                    switch(b.NodeType) {
                        case ExpressionType.Equal:
                        case ExpressionType.NotEqual:
                            if(IsExternalColumn(b.Left) && GetColumn(b.Right) != null) {
                                columns.Add(GetColumn(b.Right));
                            }
                            else if(IsExternalColumn(b.Right) && GetColumn(b.Left) != null) {
                                columns.Add(GetColumn(b.Left));
                            }
                            break;
                        case ExpressionType.And:
                        case ExpressionType.AndAlso:
                            if(b.Type == typeof(bool) || b.Type == typeof(bool?)) {
                                Gather(b.Left);
                                Gather(b.Right);
                            }
                            break;
                    }
                }
            }

            private ColumnExpression GetColumn(Expression exp) {
                while(exp.NodeType == ExpressionType.Convert)
                    exp = ((UnaryExpression)exp).Operand;
                return exp as ColumnExpression;
            }

            private bool IsExternalColumn(Expression exp) {
                ColumnExpression col = GetColumn(exp);
                if(col != null && !aliases.Contains(col.Alias))
                    return true;
                return false;
            }
        }

        #endregion
    }

    public class QueryLinguist {
        private readonly QueryLanguage language;
        private readonly QueryTranslator translator;

        public QueryLinguist(QueryLanguage language, QueryTranslator translator) {
            this.language = language;
            this.translator = translator;
        }

        public QueryLanguage Language {
            get { return language; }
        }

        public QueryTranslator Translator {
            get { return translator; }
        }

        /// <summary>
        /// Provides language specific query translation. Use this to apply language specific rewrites or to make assertions/validations about the query.
        /// </summary>
        /// <param name="expression"> </param>
        /// <returns> </returns>
        public virtual Expression Translate(Expression expression) {
            // remove redundant layers again before cross apply rewrite
            expression = UnusedColumnRemover.Remove(expression);
            expression = RedundantColumnRemover.Remove(expression);
            expression = RedundantSubqueryRemover.Remove(expression);

            // convert cross-apply and outer-apply joins into inner & left-outer-joins if possible
            Expression rewritten = CrossApplyRewriter.Rewrite(language, expression);

            // convert cross joins into inner joins
            rewritten = CrossJoinRewriter.Rewrite(rewritten);

            if(rewritten != expression) {
                expression = rewritten;
                // do final reduction
                expression = UnusedColumnRemover.Remove(expression);
                expression = RedundantSubqueryRemover.Remove(expression);
                expression = RedundantJoinRemover.Remove(expression);
                expression = RedundantColumnRemover.Remove(expression);
            }

            return expression;
        }

        /// <summary>
        /// Converts the query expression into text of this query language
        /// </summary>
        /// <param name="expression"> </param>
        /// <returns> </returns>
        public virtual string Format(Expression expression) {
            // use common SQL formatter by default
            return SqlFormatter.Format(expression);
        }

        /// <summary>
        /// Determine which sub-expressions must be parameters
        /// </summary>
        /// <param name="expression"> </param>
        /// <returns> </returns>
        public virtual Expression Parameterize(Expression expression) {
            return Parameterizer.Parameterize(language, expression);
        }
    }
}