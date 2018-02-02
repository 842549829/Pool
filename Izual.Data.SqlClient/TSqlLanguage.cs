#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.SqlClient.TSqlLanguage.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Izual.Data.Common;
using Izual.Linq;

namespace Izual.Data.SqlClient {
    /// <summary>
    /// TSQL specific QueryLanguage
    /// </summary>
    public class TSqlLanguage : QueryLanguage {
        private static readonly char[] splitChars = new[] {'.'};
        private static TSqlLanguage _default;
        private readonly DbTypeResolver typeSystem = new DbTypeResolver();

        public override QueryTypeResolver TypeSystem {
            get { return typeSystem; }
        }

        public override bool AllowsMultipleCommands {
            get { return true; }
        }

        public override bool AllowSubqueryInSelectWithoutFrom {
            get { return true; }
        }

        public override bool AllowDistinctInAggregates {
            get { return true; }
        }

        public static TSqlLanguage Default {
            get {
                if(_default == null) {
                    Interlocked.CompareExchange(ref _default, new TSqlLanguage(), null);
                }
                return _default;
            }
        }

        public override string Quote(string name) {
            if(name.StartsWith("[") && name.EndsWith("]")) {
                return name;
            }
            else if(name.IndexOf('.') > 0) {
                return "[" + string.Join("].[", name.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)) + "]";
            }
            else {
                return "[" + name + "]";
            }
        }

        public override Expression GetGeneratedIdExpression(MemberInfo member) {
            return new FunctionExpression(TypeHelper.GetMemberType(member), "SCOPE_IDENTITY()", null);
        }

        public override QueryLinguist CreateLinguist(QueryTranslator translator) {
            return new TSqlLinguist(this, translator);
        }

        #region Nested type: TSqlLinguist

        private class TSqlLinguist : QueryLinguist {
            public TSqlLinguist(TSqlLanguage language, QueryTranslator translator) : base(language, translator) {}

            public override Expression Translate(Expression expression) {
                // fix up any order-by's
                expression = OrderByRewriter.Rewrite(Language, expression);

                expression = base.Translate(expression);

                // convert skip/take info into RowNumber pattern
                expression = SkipToRowNumberRewriter.Rewrite(Language, expression);

                // fix up any order-by's we may have changed
                expression = OrderByRewriter.Rewrite(Language, expression);

                return expression;
            }

            public override string Format(Expression expression) {
                return TSqlFormatter.Format(expression, Language);
            }
        }

        #endregion
    }
}