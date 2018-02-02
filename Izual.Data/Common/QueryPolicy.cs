#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.QueryPolicy.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Linq.Expressions;
using System.Reflection;

namespace Izual.Data.Common {
    /// <summary>
    /// 定义查询和实例化对象的策略
    /// </summary>
    public class QueryPolicy {
        public static readonly QueryPolicy Default = new QueryPolicy();

        /// <summary>
        /// <para>决定一个关系成员是否包含在查询结果中。</para>
        /// <para>例如：Order 对象聚合一个 OrderDetail 的列表作为属性 OrderDetails，</para>
        /// <para>如果 IsIncluded 返回 true，在查询 Order 对象的同时，填充该 Order 对象相关联的所有 OrderDetail 对象到属性 OrderDetails 中。</para>
        /// <para>具体的填充策略，由方法 IsDeferLoaded 的返回值决定。</para>
        /// </summary>
        /// <param name="member">聚合成员</param>
        /// <returns>返回一个值，true 表示指定的关系成员在查询结果中将被填充，否则保留该成员的默认值。</returns>
        public virtual bool IsIncluded(MemberInfo member) {
            return false;
        }

        /// <summary>
        /// 如果指定的关系成员包含在查询结果中，isDeferLoaded 的返回值将决定该属性是否延迟加载。
        /// </summary>
        /// <param name="member"> </param>
        /// <returns> </returns>
        public virtual bool IsDeferLoaded(MemberInfo member) {
            return false;
        }

        public virtual QueryPolice CreatePolice(QueryTranslator translator) {
            return new QueryPolice(this, translator);
        }
    }

    public class QueryPolice {
        private readonly QueryPolicy policy;
        private readonly QueryTranslator translator;

        public QueryPolice(QueryPolicy policy, QueryTranslator translator) {
            this.policy = policy;
            this.translator = translator;
        }

        public QueryPolicy Policy {
            get { return policy; }
        }

        public QueryTranslator Translator {
            get { return translator; }
        }

        public virtual Expression ApplyPolicy(Expression expression, MemberInfo member) {
            return expression;
        }

        /// <summary>
        /// Provides policy specific query translations. This is where choices about inclusion of related objects and how heirarchies are materialized affect the definition of the queries.
        /// </summary>
        /// <param name="expression"> </param>
        /// <returns> </returns>
        public virtual Expression Translate(Expression expression) {
            // add included relationships to client projection
            Expression rewritten = RelationshipIncluder.Include(translator.Mapper, expression);
            if(rewritten != expression) {
                expression = rewritten;
                expression = UnusedColumnRemover.Remove(expression);
                expression = RedundantColumnRemover.Remove(expression);
                expression = RedundantSubqueryRemover.Remove(expression);
                expression = RedundantJoinRemover.Remove(expression);
            }

            // convert any singleton (1:1 or n:1) projections into server-side joins (cardinality is preserved)
            rewritten = SingletonProjectionRewriter.Rewrite(translator.Linguist.Language, expression);
            if(rewritten != expression) {
                expression = rewritten;
                expression = UnusedColumnRemover.Remove(expression);
                expression = RedundantColumnRemover.Remove(expression);
                expression = RedundantSubqueryRemover.Remove(expression);
                expression = RedundantJoinRemover.Remove(expression);
            }

            // convert projections into client-side joins
            rewritten = ClientJoinedProjectionRewriter.Rewrite(policy, translator.Linguist.Language, expression);
            if(rewritten != expression) {
                expression = rewritten;
                expression = UnusedColumnRemover.Remove(expression);
                expression = RedundantColumnRemover.Remove(expression);
                expression = RedundantSubqueryRemover.Remove(expression);
                expression = RedundantJoinRemover.Remove(expression);
            }

            return expression;
        }

        /// <summary>
        /// Converts a query into an execution plan. The plan is an function that executes the query and builds the resulting objects.
        /// </summary>
        /// <param name="projection"> </param>
        /// <param name="provider"> </param>
        /// <returns> </returns>
        public virtual Expression BuildExecutionPlan(Expression query, Expression provider) {
            return ExecutionBuilder.Build(translator.Linguist, policy, query, provider);
        }
    }
}