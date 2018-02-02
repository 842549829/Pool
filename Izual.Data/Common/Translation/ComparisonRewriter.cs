#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.ComparisonRewriter.cs
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
using Izual.Linq;

namespace Izual.Data.Common.Translation {
    public class ComparisonRewriter : DbExpressionVisitor {
        private readonly QueryMapping mapping;

        private ComparisonRewriter(QueryMapping mapping) {
            this.mapping = mapping;
        }

        public static Expression Rewrite(QueryMapping mapping, Expression expression) {
            return new ComparisonRewriter(mapping).Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression b) {
            switch(b.NodeType) {
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                    Expression result = Compare(b);
                    if(result == b)
                        goto default;
                    return Visit(result);
                default:
                    return base.VisitBinary(b);
            }
        }

        protected Expression SkipConvert(Expression expression) {
            while(expression.NodeType == ExpressionType.Convert) {
                expression = ((UnaryExpression)expression).Operand;
            }
            return expression;
        }

        protected Expression Compare(BinaryExpression bop) {
            Expression e1 = SkipConvert(bop.Left);
            Expression e2 = SkipConvert(bop.Right);
            var entity1 = e1 as EntryExpression;
            var entity2 = e2 as EntryExpression;
            bool negate = bop.NodeType == ExpressionType.NotEqual;
            if(entity1 != null) {
                return MakePredicate(e1, e2, mapping.GetPrimaryKeyMembers(entity1.Entry), negate);
            }
            else if(entity2 != null) {
                return MakePredicate(e1, e2, mapping.GetPrimaryKeyMembers(entity2.Entry), negate);
            }
            IEnumerable<MemberInfo> dm1 = GetDefinedMembers(e1);
            IEnumerable<MemberInfo> dm2 = GetDefinedMembers(e2);

            if(dm1 == null && dm2 == null) {
                // neither are constructed types
                return bop;
            }

            if(dm1 != null && dm2 != null) {
                // both are constructed types, so they'd better have the same members declared
                var names1 = new HashSet<string>(dm1.Select(m => m.Name));
                var names2 = new HashSet<string>(dm2.Select(m => m.Name));
                if(names1.IsSubsetOf(names2) && names2.IsSubsetOf(names1)) {
                    return MakePredicate(e1, e2, dm1, negate);
                }
            }
            else if(dm1 != null) {
                return MakePredicate(e1, e2, dm1, negate);
            }
            else if(dm2 != null) {
                return MakePredicate(e1, e2, dm2, negate);
            }

            throw new InvalidOperationException("Cannot compare two constructed types with different sets of members assigned.");
        }

        protected Expression MakePredicate(Expression e1, Expression e2, IEnumerable<MemberInfo> members, bool negate) {
            Expression pred = members.Select(m => QueryBinder.BindMember(e1, m).Equal(QueryBinder.BindMember(e2, m))).Join(ExpressionType.And);
            if(negate)
                pred = Expression.Not(pred);
            return pred;
        }

        private IEnumerable<MemberInfo> GetDefinedMembers(Expression expr) {
            var mini = expr as MemberInitExpression;
            if(mini != null) {
                IEnumerable<MemberInfo> members = mini.Bindings.Select(b => FixMember(b.Member));
                if(mini.NewExpression.Members != null) {
                    members.Concat(mini.NewExpression.Members.Select(m => FixMember(m)));
                }
                return members;
            }
            else {
                var nex = expr as NewExpression;
                if(nex != null && nex.Members != null) {
                    return nex.Members.Select(m => FixMember(m));
                }
            }
            return null;
        }

        private static MemberInfo FixMember(MemberInfo member) {
            if(member.MemberType == MemberTypes.Method && member.Name.StartsWith("get_")) {
                return member.DeclaringType.GetProperty(member.Name.Substring(4));
            }
            return member;
        }
    }
}