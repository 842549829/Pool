#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.QueryCache.cs
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

namespace Izual.Linq {
    public class QueryCache {
        private static readonly Func<QueryCompiler.CompiledQuery, QueryCompiler.CompiledQuery, bool> fnCompareQueries = CompareQueries;
        private static readonly Func<object, object, bool> fnCompareValues = CompareConstantValues;
        private readonly MostRecentlyUsedCache<QueryCompiler.CompiledQuery> cache;

        public QueryCache(int maxSize) {
            cache = new MostRecentlyUsedCache<QueryCompiler.CompiledQuery>(maxSize, fnCompareQueries);
        }

        public int Count {
            get { return cache.Count; }
        }

        private static bool CompareQueries(QueryCompiler.CompiledQuery x, QueryCompiler.CompiledQuery y) {
            return ExpressionComparer.AreEqual(x.Query, y.Query, fnCompareValues);
        }

        private static bool CompareConstantValues(object x, object y) {
            if(x == y) return true;
            if(x == null || y == null) return false;
            if(x is IQueryable && y is IQueryable && x.GetType() == y.GetType()) return true;
            return Equals(x, y);
        }

        public object Execute(Expression query) {
            object[] args;
            QueryCompiler.CompiledQuery cached = Find(query, true, out args);
            return cached.Invoke(args);
        }

        public object Execute(IQueryable query) {
            return Equals(query.Expression);
        }

        public IEnumerable<T> Execute<T>(IQueryable<T> query) {
            return (IEnumerable<T>)Execute(query.Expression);
        }

        public void Clear() {
            cache.Clear();
        }

        public bool Contains(Expression query) {
            object[] args;
            return Find(query, false, out args) != null;
        }

        public bool Contains(IQueryable query) {
            return Contains(query.Expression);
        }

        private QueryCompiler.CompiledQuery Find(Expression query, bool add, out object[] args) {
            LambdaExpression pq = Parameterize(query, out args);
            var cq = new QueryCompiler.CompiledQuery(pq);
            QueryCompiler.CompiledQuery cached;
            cache.Lookup(cq, add, out cached);
            return cached;
        }

        private LambdaExpression Parameterize(Expression query, out object[] arguments) {
            IQueryProvider provider = FindProvider(query);
            if(provider == null) {
                throw new ArgumentException("Cannot deduce query provider from query");
            }

            var ep = provider as IEntryProvider;
            Func<Expression, bool> fn = ep != null ? (Func<Expression, bool>)ep.CanBeEvaluatedLocally : null;
            var parameters = new List<ParameterExpression>();
            var values = new List<object>();

            Expression body = PartialEvaluator.Eval(query, fn, c => {
                                                                   bool isQueryRoot = c.Value is IQueryable;
                                                                   if(!isQueryRoot && ep != null && !ep.CanBeParameter(c))
                                                                       return c;
                                                                   ParameterExpression p = Expression.Parameter(c.Type, "p" + parameters.Count);
                                                                   parameters.Add(p);
                                                                   values.Add(c.Value);
                                                                   // if query root then parameterize but don't replace in the tree 
                                                                   if(isQueryRoot)
                                                                       return c;
                                                                   return p;
                                                               });

            if(body.Type != typeof(object))
                body = Expression.Convert(body, typeof(object));

            arguments = values.ToArray();
            if(arguments.Length < 5) {
                return Expression.Lambda(body, parameters.ToArray());
            }
            else {
                arguments = new object[] {arguments};
                return ExplicitToObjectArray.Rewrite(body, parameters);
            }
        }

        private IQueryProvider FindProvider(Expression expression) {
            var root = TypedSubtreeFinder.Find(expression, typeof(IQueryProvider)) as ConstantExpression;
            if(root == null) {
                root = TypedSubtreeFinder.Find(expression, typeof(IQueryable)) as ConstantExpression;
            }
            if(root != null) {
                var provider = root.Value as IQueryProvider;
                if(provider == null) {
                    var query = root.Value as IQueryable;
                    if(query != null) {
                        provider = query.Provider;
                    }
                }
                return provider;
            }
            return null;
        }

        #region Nested type: ExplicitToObjectArray

        private class ExplicitToObjectArray : ExpressionVisitor {
            private readonly ParameterExpression array = Expression.Parameter(typeof(object[]), "array");
            private readonly IList<ParameterExpression> parameters;

            private ExplicitToObjectArray(IList<ParameterExpression> parameters) {
                this.parameters = parameters;
            }

            internal static LambdaExpression Rewrite(Expression body, IList<ParameterExpression> parameters) {
                var visitor = new ExplicitToObjectArray(parameters);
                return Expression.Lambda(visitor.Visit(body), visitor.array);
            }

            protected override Expression VisitParameter(ParameterExpression p) {
                for(int i = 0, n = parameters.Count; i < n; i++) {
                    if(parameters[i] == p) {
                        return Expression.Convert(Expression.ArrayIndex(array, Expression.Constant(i)), p.Type);
                    }
                }
                return p;
            }
        }

        #endregion
    }
}