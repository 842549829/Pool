#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.QueryMapping.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Izual.Data.Common.Translation;
using Izual.Linq;

namespace Izual.Data.Common {
    //public abstract class EntryMapping {
    //    public abstract string EntryId { get; }
    //    public abstract Type ElementType { get; }
    //    public abstract Type EntityType { get; }
    //    public abstract IEnumerable<MemberMapping> Members { get; set; }
    //}

    public interface IMapped {
        EntryMapping Entry{ get; }
    }

    /// <summary>
    /// Defines mapping information & rules for the query provider
    /// </summary>
    public abstract class QueryMapping {
        /// <summary>
        /// Determines the entity Id based on the type of the entity alone
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public virtual string GetEntryId(Type type) {
            return GetEntry(type).Name;
        }

        /// <summary>
        /// Get the meta entity directly corresponding to the CLR type
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public abstract EntryMapping GetEntry(Type type);

        /// <summary>
        /// Get the meta entity represented by the IQueryable context member
        /// </summary>
        /// <param name="contextMember"> </param>
        /// <returns> </returns>
        public abstract EntryMapping GetEntry(MemberInfo contextMember);

        public abstract IEnumerable<MemberInfo> GetMappedMembers(EntryMapping entity);

        public abstract bool IsPrimaryKey(EntryMapping entity, MemberInfo member);

        public virtual IEnumerable<MemberInfo> GetPrimaryKeyMembers(EntryMapping entry) {
            return entry.Members.Where(m => m.Identifier).Select(m => m.MemberInfo);
        }

        /// <summary>
        /// Determines if a property is mapped as a relationship
        /// </summary>
        /// <param name="entity"> </param>
        /// <param name="member"> </param>
        /// <returns> </returns>
        public abstract bool IsRelationship(EntryMapping entity, MemberInfo member);

        /// <summary>
        /// Determines if a relationship property refers to a single entity (as opposed to a collection.)
        /// </summary>
        /// <param name="member"> </param>
        /// <returns> </returns>
        public virtual bool IsSingletonRelationship(EntryMapping entry, MemberInfo member) {
            if(!IsRelationship(entry, member))
                return false;
            Type ieType = TypeHelper.FindIEnumerable(TypeHelper.GetMemberType(member));
            return ieType == null;
        }

        /// <summary>
        /// Determines whether a given expression can be executed locally. (It contains no parts that should be translated to the target environment.)
        /// </summary>
        /// <param name="expression"> </param>
        /// <returns> </returns>
        public virtual bool CanBeEvaluatedLocally(Expression expression) {
            // any operation on a query can't be done locally
            var cex = expression as ConstantExpression;
            if(cex != null) {
                var query = cex.Value as IQueryable;
                if(query != null && query.Provider == this)
                    return false;
            }
            var mc = expression as MethodCallExpression;
            if(mc != null && (mc.Method.DeclaringType == typeof(Enumerable) || mc.Method.DeclaringType == typeof(Queryable) || mc.Method.DeclaringType == typeof(Updatable))) {
                return false;
            }
            if(expression.NodeType == ExpressionType.Convert && expression.Type == typeof(object))
                return true;
            return expression.NodeType != ExpressionType.Parameter && expression.NodeType != ExpressionType.Lambda;
        }

        public abstract object GetPrimaryKey(EntryMapping entity, object instance);
        public abstract Expression GetPrimaryKeyQuery(EntryMapping entity, Expression source, Expression[] keys);
        public abstract IEnumerable<EntryInfo> GetDependentEntities(EntryMapping entity, object instance);
        public abstract IEnumerable<EntryInfo> GetDependingEntities(EntryMapping entity, object instance);
        public abstract object CloneEntity(EntryMapping entity, object instance);
        public abstract bool IsModified(EntryMapping entity, object instance, object original);

        public abstract QueryMapper CreateMapper(QueryTranslator translator);
    }

    public abstract class QueryMapper {
        public abstract QueryMapping Mapping{ get; }
        public abstract QueryTranslator Translator{ get; }

        /// <summary>
        /// Get a query expression that selects all entities from a table
        /// </summary>
        /// <param name="rowType"> </param>
        /// <returns> </returns>
        public abstract ProjectionExpression GetQueryExpression(EntryMapping entity);

        /// <summary>
        /// Gets an expression that constructs an entity instance relative to a root. The root is most often a TableExpression, but may be any other experssion such as a ConstantExpression.
        /// </summary>
        /// <param name="root"> </param>
        /// <param name="entity"> </param>
        /// <returns> </returns>
        public abstract EntryExpression GetEntityExpression(Expression root, EntryMapping entity);

        /// <summary>
        /// Get an expression for a mapped property relative to a root expression. The root is either a TableExpression or an expression defining an entity instance.
        /// </summary>
        /// <param name="root"> </param>
        /// <param name="entity"> </param>
        /// <param name="member"> </param>
        /// <returns> </returns>
        public abstract Expression GetMemberExpression(Expression root, EntryMapping entity, MemberInfo member);

        /// <summary>
        /// Get an expression that represents the insert operation for the specified instance.
        /// </summary>
        /// <param name="entity"> </param>
        /// <param name="instance"> The instance to insert. </param>
        /// <param name="selector"> A lambda expression that computes a return value from the operation. </param>
        /// <returns> </returns>
        public abstract Expression GetInsertExpression(EntryMapping entity, Expression instance, LambdaExpression selector);

        /// <summary>
        /// Get an expression that represents the update operation for the specified instance.
        /// </summary>
        /// <param name="entity"> </param>
        /// <param name="instance"> </param>
        /// <param name="updateCheck"> </param>
        /// <param name="selector"> </param>
        /// <param name="else"> </param>
        /// <returns> </returns>
        public abstract Expression GetUpdateExpression(EntryMapping entity, Expression instance, Expression updateCheck, LambdaExpression selector, Expression @else);

        /// <summary>
        /// Get an expression that represents the insert-or-update operation for the specified instance.
        /// </summary>
        /// <param name="entity"> </param>
        /// <param name="instance"> </param>
        /// <param name="updateCheck"> </param>
        /// <param name="resultSelector"> </param>
        /// <returns> </returns>
        public abstract Expression GetInsertOrUpdateExpression(EntryMapping entity, Expression instance, LambdaExpression updateCheck, LambdaExpression resultSelector);

        /// <summary>
        /// Get an expression that represents the delete operation for the specified instance.
        /// </summary>
        /// <param name="entity"> </param>
        /// <param name="instance"> </param>
        /// <param name="deleteCheck"> </param>
        /// <returns> </returns>
        public abstract Expression GetDeleteExpression(EntryMapping entity, Expression instance, LambdaExpression deleteCheck);

        /// <summary>
        /// Recreate the type projection with the additional members included
        /// </summary>
        /// <param name="entity"> </param>
        /// <param name="fnIsIncluded"> </param>
        /// <returns> </returns>
        public abstract EntryExpression IncludeMembers(EntryExpression entity, Func<MemberInfo, bool> fnIsIncluded);

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public abstract bool HasIncludedMembers(EntryExpression entity);

        /// <summary>
        /// Apply mapping to a sub query expression
        /// </summary>
        /// <param name="expression"> </param>
        /// <returns> </returns>
        public virtual Expression ApplyMapping(Expression expression) {
            return QueryBinder.Bind(this, expression);
        }

        /// <summary>
        /// Apply mapping translations to this expression
        /// </summary>
        /// <param name="expression"> </param>
        /// <returns> </returns>
        public virtual Expression Translate(Expression expression) {
            // convert references to LINQ operators into query specific nodes
            expression = QueryBinder.Bind(this, expression);

            // move aggregate computations so they occur in same select as group-by
            expression = AggregateRewriter.Rewrite(Translator.Linguist.Language, expression);

            // do reduction so duplicate association's are likely to be clumped together
            expression = UnusedColumnRemover.Remove(expression);
            expression = RedundantColumnRemover.Remove(expression);
            expression = RedundantSubqueryRemover.Remove(expression);
            expression = RedundantJoinRemover.Remove(expression);

            // convert references to association properties into correlated queries
            Expression bound = RelationshipBinder.Bind(this, expression);
            if(bound != expression) {
                expression = bound;
                // clean up after ourselves! (multiple references to same association property)
                expression = RedundantColumnRemover.Remove(expression);
                expression = RedundantJoinRemover.Remove(expression);
            }

            // rewrite comparision checks between entities and multi-valued constructs
            expression = ComparisonRewriter.Rewrite(Mapping, expression);

            return expression;
        }
    }

    public struct EntryInfo {
        private readonly object instance;
        private readonly EntryMapping mapping;

        public EntryInfo(object instance, EntryMapping mapping) {
            this.instance = instance;
            this.mapping = mapping;
        }

        public object Instance {
            get { return instance; }
        }

        public EntryMapping Mapping {
            get { return mapping; }
        }
    }
}