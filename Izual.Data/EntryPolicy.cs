#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.EntryPolicy.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Izual.Data.Common;
using Izual.Linq;
using ExpressionVisitor = Izual.Linq.ExpressionVisitor;

namespace Izual.Data {
    public class EntryPolicy : QueryPolicy {
        private readonly HashSet<MemberInfo> deferred = new HashSet<MemberInfo>();
        private readonly HashSet<MemberInfo> included = new HashSet<MemberInfo>();
        private readonly Dictionary<MemberInfo, List<LambdaExpression>> operations = new Dictionary<MemberInfo, List<LambdaExpression>>();

        public void Apply(LambdaExpression fnApply) {
            if(fnApply == null)
                throw new ArgumentNullException("fnApply");
            if(fnApply.Parameters.Count != 1)
                throw new ArgumentException("Apply function has wrong number of arguments.");
            AddOperation(TypeHelper.GetElementType(fnApply.Parameters[0].Type), fnApply);
        }

        public void Apply<TEntity>(Expression<Func<IEnumerable<TEntity>, IEnumerable<TEntity>>> fnApply) {
            Apply((LambdaExpression)fnApply);
        }

        public void Include(MemberInfo member) {
            Include(member, false);
        }

        public void Include(MemberInfo member, bool deferLoad) {
            included.Add(member);
            if(deferLoad)
                Defer(member);
        }

        public void IncludeWith(LambdaExpression fnMember) {
            IncludeWith(fnMember, false);
        }

        public void IncludeWith(LambdaExpression fnMember, bool deferLoad) {
            MemberExpression rootMember = RootMemberFinder.Find(fnMember, fnMember.Parameters[0]);
            if(rootMember == null)
                throw new InvalidOperationException("Subquery does not originate with a member access");
            Include(rootMember.Member, deferLoad);
            if(rootMember != fnMember.Body) {
                AssociateWith(fnMember);
            }
        }

        public void IncludeWith<TEntity>(Expression<Func<TEntity, object>> fnMember) {
            IncludeWith((LambdaExpression)fnMember, false);
        }

        public void IncludeWith<TEntity>(Expression<Func<TEntity, object>> fnMember, bool deferLoad) {
            IncludeWith((LambdaExpression)fnMember, deferLoad);
        }

        private void Defer(MemberInfo member) {
            Type mType = TypeHelper.GetMemberType(member);
            if(mType.IsGenericType) {
                Type gType = mType.GetGenericTypeDefinition();
                if(gType != typeof(IEnumerable<>) && gType != typeof(IList<>) && !typeof(IDeferLoadable).IsAssignableFrom(mType)) {
                    throw new InvalidOperationException(string.Format("The member '{0}' cannot be deferred due to its type.", member));
                }
            }
            deferred.Add(member);
        }

        public void AssociateWith(LambdaExpression memberQuery) {
            MemberExpression rootMember = RootMemberFinder.Find(memberQuery, memberQuery.Parameters[0]);
            if(rootMember == null)
                throw new InvalidOperationException("Subquery does not originate with a member access");
            if(rootMember != memberQuery.Body) {
                ParameterExpression memberParam = Expression.Parameter(rootMember.Type, "root");
                Expression newBody = ExpressionReplacer.Replace(memberQuery.Body, rootMember, memberParam);
                AddOperation(rootMember.Member, Expression.Lambda(newBody, memberParam));
            }
        }

        private void AddOperation(MemberInfo member, LambdaExpression operation) {
            List<LambdaExpression> memberOps;
            if(!operations.TryGetValue(member, out memberOps)) {
                memberOps = new List<LambdaExpression>();
                operations.Add(member, memberOps);
            }
            memberOps.Add(operation);
        }

        public void AssociateWith<TEntity>(Expression<Func<TEntity, IEnumerable>> memberQuery) {
            AssociateWith((LambdaExpression)memberQuery);
        }

        public override bool IsIncluded(MemberInfo member) {
            return included.Contains(member);
        }

        public override bool IsDeferLoaded(MemberInfo member) {
            return deferred.Contains(member);
        }

        public override QueryPolice CreatePolice(QueryTranslator translator) {
            return new Police(this, translator);
        }

        #region Nested type: Police

        private class Police : QueryPolice {
            private readonly EntryPolicy policy;

            public Police(EntryPolicy policy, QueryTranslator translator) : base(policy, translator) {
                this.policy = policy;
            }

            public override Expression ApplyPolicy(Expression expression, MemberInfo member) {
                List<LambdaExpression> ops;
                if(policy.operations.TryGetValue(member, out ops)) {
                    Expression result = expression;
                    foreach(LambdaExpression fnOp in ops) {
                        Expression pop = PartialEvaluator.Eval(fnOp, Translator.Mapper.Mapping.CanBeEvaluatedLocally);
                        result = Translator.Mapper.ApplyMapping(Expression.Invoke(pop, result));
                    }
                    var projection = (ProjectionExpression)result;
                    if(projection.Type != expression.Type) {
                        LambdaExpression fnAgg = Aggregator.GetAggregator(expression.Type, projection.Type);
                        projection = new ProjectionExpression(projection.Select, projection.Projector, fnAgg);
                    }
                    return projection;
                }
                return expression;
            }
        }

        #endregion

        #region Nested type: RootMemberFinder

        private class RootMemberFinder : ExpressionVisitor {
            private readonly ParameterExpression parameter;
            private MemberExpression found;

            private RootMemberFinder(ParameterExpression parameter) {
                this.parameter = parameter;
            }

            public static MemberExpression Find(Expression query, ParameterExpression parameter) {
                var finder = new RootMemberFinder(parameter);
                finder.Visit(query);
                return finder.found;
            }

            protected override Expression VisitMethodCall(MethodCallExpression m) {
                if(m.Object != null) {
                    Visit(m.Object);
                }
                else if(m.Arguments.Count > 0) {
                    Visit(m.Arguments[0]);
                }
                return m;
            }

            protected override Expression VisitMemberAccess(MemberExpression m) {
                if(m.Expression == parameter) {
                    found = m;
                    return m;
                }
                else {
                    return base.VisitMemberAccess(m);
                }
            }
        }

        #endregion
    }
}