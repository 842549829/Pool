#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.QueryCompiler.cs
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
using System.Threading;

namespace Izual.Linq {
    /// <summary>
    /// Creates a reusable, parameterized representation of a query that caches the execution plan
    /// </summary>
    public static class QueryCompiler {
        public static Delegate Compile(LambdaExpression query) {
            var cq = new CompiledQuery(query);
            return StrongDelegate.CreateDelegate(query.Type, cq.Invoke);
        }

        public static D Compile<D>(Expression<D> query) {
            return (D)(object)Compile((LambdaExpression)query);
        }

        public static Func<TResult> Compile<TResult>(Expression<Func<TResult>> query) {
            return new CompiledQuery(query).Invoke<TResult>;
        }

        public static Func<T1, TResult> Compile<T1, TResult>(Expression<Func<T1, TResult>> query) {
            return new CompiledQuery(query).Invoke<T1, TResult>;
        }

        public static Func<T1, T2, TResult> Compile<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> query) {
            return new CompiledQuery(query).Invoke<T1, T2, TResult>;
        }

        public static Func<T1, T2, T3, TResult> Compile<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> query) {
            return new CompiledQuery(query).Invoke<T1, T2, T3, TResult>;
        }

        public static Func<T1, T2, T3, T4, TResult> Compile<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> query) {
            return new CompiledQuery(query).Invoke<T1, T2, T3, T4, TResult>;
        }

        public static Func<IEnumerable<T>> Compile<T>(this IQueryable<T> source) {
            return Compile(Expression.Lambda<Func<IEnumerable<T>>>((source).Expression));
        }

        #region Nested type: CompiledQuery

        public class CompiledQuery {
            private readonly LambdaExpression query;
            private bool checkedForInvoker;
            private Delegate fnQuery;
            private Func<object[], object> invoker;

            internal CompiledQuery(LambdaExpression query) {
                this.query = query;
            }

            public LambdaExpression Query {
                get { return query; }
            }

            internal void Compile(params object[] args) {
                if(fnQuery == null) {
                    // first identify the query provider being used
                    Expression body = query.Body;

                    // ask the query provider to compile the query by 'executing' the lambda expression
                    IQueryProvider provider = FindProvider(body, args);
                    if(provider == null) {
                        throw new InvalidOperationException("Could not find query provider");
                    }

                    var result = (Delegate)provider.Execute(query);
                    Interlocked.CompareExchange(ref fnQuery, result, null);
                }
            }

            internal IQueryProvider FindProvider(Expression expression, object[] args) {
                Expression root = FindProviderInExpression(expression) as ConstantExpression;
                if(root == null && args != null && args.Length > 0) {
                    Expression replaced = ExpressionReplacer.ReplaceAll(expression, query.Parameters.ToArray(), args.Select((a, i) => Expression.Constant(a, query.Parameters[i].Type)).ToArray());
                    root = FindProviderInExpression(replaced);
                }
                if(root != null) {
                    var cex = root as ConstantExpression;
                    if(cex == null) {
                        cex = PartialEvaluator.Eval(root) as ConstantExpression;
                    }
                    if(cex != null) {
                        var provider = cex.Value as IQueryProvider;
                        if(provider == null) {
                            var query = cex.Value as IQueryable;
                            if(query != null) {
                                provider = query.Provider;
                            }
                        }
                        return provider;
                    }
                }
                return null;
            }

            private Expression FindProviderInExpression(Expression expression) {
                Expression root = TypedSubtreeFinder.Find(expression, typeof(IQueryProvider));
                if(root == null) {
                    root = TypedSubtreeFinder.Find(expression, typeof(IQueryable));
                }
                return root;
            }

            public object Invoke(object[] args) {
                Compile(args);
                if(invoker == null) {
                    invoker = GetInvoker();
                }
                if(invoker != null) {
                    return invoker(args);
                }
                else {
                    try {
                        return fnQuery.DynamicInvoke(args);
                    }
                    catch(TargetInvocationException tie) {
                        throw tie.InnerException;
                    }
                }
            }

            private Func<object[], object> GetInvoker() {
                if(fnQuery != null && invoker == null && !checkedForInvoker) {
                    checkedForInvoker = true;
                    Type fnType = fnQuery.GetType();
                    if(fnType.FullName.StartsWith("System.Func`")) {
                        Type[] typeArgs = fnType.GetGenericArguments();
                        MethodInfo method = GetType().GetMethod("FastInvoke" + typeArgs.Length, BindingFlags.Public | BindingFlags.Instance);
                        if(method != null) {
                            invoker = (Func<object[], object>)Delegate.CreateDelegate(typeof(Func<object[], object>), this, method.MakeGenericMethod(typeArgs));
                        }
                    }
                }
                return invoker;
            }

            public object FastInvoke1<R>(object[] args) {
                return ((Func<R>)fnQuery)();
            }

            public object FastInvoke2<A1, R>(object[] args) {
                return ((Func<A1, R>)fnQuery)((A1)args[0]);
            }

            public object FastInvoke3<A1, A2, R>(object[] args) {
                return ((Func<A1, A2, R>)fnQuery)((A1)args[0], (A2)args[1]);
            }

            public object FastInvoke4<A1, A2, A3, R>(object[] args) {
                return ((Func<A1, A2, A3, R>)fnQuery)((A1)args[0], (A2)args[1], (A3)args[2]);
            }

            public object FastInvoke5<A1, A2, A3, A4, R>(object[] args) {
                return ((Func<A1, A2, A3, A4, R>)fnQuery)((A1)args[0], (A2)args[1], (A3)args[2], (A4)args[3]);
            }

            internal TResult Invoke<TResult>() {
                Compile(null);
                return ((Func<TResult>)fnQuery)();
            }

            internal TResult Invoke<T1, TResult>(T1 arg) {
                Compile(arg);
                return ((Func<T1, TResult>)fnQuery)(arg);
            }

            internal TResult Invoke<T1, T2, TResult>(T1 arg1, T2 arg2) {
                Compile(arg1, arg2);
                return ((Func<T1, T2, TResult>)fnQuery)(arg1, arg2);
            }

            internal TResult Invoke<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3) {
                Compile(arg1, arg2, arg3);
                return ((Func<T1, T2, T3, TResult>)fnQuery)(arg1, arg2, arg3);
            }

            internal TResult Invoke<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
                Compile(arg1, arg2, arg3, arg4);
                return ((Func<T1, T2, T3, T4, TResult>)fnQuery)(arg1, arg2, arg3, arg4);
            }
        }

        #endregion
    }
}