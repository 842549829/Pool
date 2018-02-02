#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.Updatable.cs
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

namespace Izual.Linq {
    public interface IUpdatable : IQueryable { }

    public interface IUpdatable<T> : IUpdatable, IQueryable<T> { }

    public static class Updatable {
        public static object Insert(IUpdatable collection, object instance, LambdaExpression resultSelector) {
            MethodCallExpression callMyself = Expression.Call(null, (MethodInfo)MethodBase.GetCurrentMethod(), collection.Expression, Expression.Constant(instance),
                                                              resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(LambdaExpression)));
            return collection.Provider.Execute(callMyself);
        }

        public static TResult Insert<T, TResult>(this IUpdatable<T> collection, T instance, Expression<Func<T, TResult>> resultSelector) {
            MethodCallExpression callMyself = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(TResult)), collection.Expression,
                                                              Expression.Constant(instance),
                                                              resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(Expression<Func<T, TResult>>)));
            return (TResult)collection.Provider.Execute(callMyself);
        }

        public static int Insert<T>(this IUpdatable<T> collection, T instance) {
            return Insert<T, int>(collection, instance, null);
        }

        public static int Update<T>(this IUpdatable<T> collection, Expression<Func<T,object>> action, Expression<Func<T, bool>> predicate) {
            var callMyself = Expression.Call(null,
                ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                collection.Expression,
                action,
                predicate);
            return (int)collection.Provider.Execute(callMyself);
        }


        public static int Update<T>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> updateCheck) {
            return Update<T, int>(collection, instance, updateCheck, null);
        }

        public static int Update<T>(this IUpdatable<T> collection, T instance) {
            return Update<T, int>(collection, instance, null, null);
        }


        public static TResult Update<T, TResult>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> updateCheck, Expression<Func<T, TResult>> resultSelector) {
            MethodCallExpression callMyself = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(TResult)), collection.Expression,
                                                              Expression.Constant(instance),
                                                              updateCheck != null ? (Expression)Expression.Quote(updateCheck) : Expression.Constant(null, typeof(Expression<Func<T, bool>>)),
                                                              resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(Expression<Func<T, TResult>>)));
            return (TResult)collection.Provider.Execute(callMyself);
        }

        public static object Update(IUpdatable collection, object instance, LambdaExpression updateCheck, LambdaExpression resultSelector) {
            MethodCallExpression callMyself = Expression.Call(null, (MethodInfo)MethodBase.GetCurrentMethod(), collection.Expression, Expression.Constant(instance),
                                                              updateCheck != null ? (Expression)Expression.Quote(updateCheck) : Expression.Constant(null, typeof(LambdaExpression)),
                                                              resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(LambdaExpression)));
            return collection.Provider.Execute(callMyself);
        }


        public static object InsertOrUpdate(IUpdatable collection, object instance, LambdaExpression updateCheck, LambdaExpression resultSelector) {
            MethodCallExpression callMyself = Expression.Call(null, (MethodInfo)MethodBase.GetCurrentMethod(), collection.Expression, Expression.Constant(instance),
                                                              updateCheck != null ? (Expression)Expression.Quote(updateCheck) : Expression.Constant(null, typeof(LambdaExpression)),
                                                              resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(LambdaExpression)));
            return collection.Provider.Execute(callMyself);
        }

        public static TResult InsertOrUpdate<T, TResult>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> updateCheck, Expression<Func<T, TResult>> resultSelector) {
            MethodCallExpression callMyself = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(TResult)), collection.Expression,
                                                              Expression.Constant(instance),
                                                              updateCheck != null ? (Expression)Expression.Quote(updateCheck) : Expression.Constant(null, typeof(Expression<Func<T, bool>>)),
                                                              resultSelector != null ? (Expression)Expression.Quote(resultSelector) : Expression.Constant(null, typeof(Expression<Func<T, TResult>>)));
            return (TResult)collection.Provider.Execute(callMyself);
        }

        public static int InsertOrUpdate<T>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> updateCheck) {
            return InsertOrUpdate<T, int>(collection, instance, updateCheck, null);
        }

        public static int InsertOrUpdate<T>(this IUpdatable<T> collection, T instance) {
            return InsertOrUpdate<T, int>(collection, instance, null, null);
        }

        public static object Delete(IUpdatable collection, object instance, LambdaExpression deleteCheck) {
            MethodCallExpression callMyself = Expression.Call(null, (MethodInfo)MethodBase.GetCurrentMethod(), collection.Expression, Expression.Constant(instance),
                                                              deleteCheck != null ? (Expression)Expression.Quote(deleteCheck) : Expression.Constant(null, typeof(LambdaExpression)));
            return collection.Provider.Execute(callMyself);
        }

        public static int Delete<T>(this IUpdatable<T> collection, T instance, Expression<Func<T, bool>> deleteCheck) {
            MethodCallExpression callMyself = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), collection.Expression, Expression.Constant(instance),
                                                              deleteCheck != null ? (Expression)Expression.Quote(deleteCheck) : Expression.Constant(null, typeof(Expression<Func<T, bool>>)));
            return (int)collection.Provider.Execute(callMyself);
        }

        public static int Delete<T>(this IUpdatable<T> collection, T instance) {
            return Delete(collection, instance, null);
        }

        public static int Delete(IUpdatable collection, LambdaExpression predicate) {
            MethodCallExpression callMyself = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()), collection.Expression,
                                                              predicate != null ? (Expression)Expression.Quote(predicate) : Expression.Constant(null, typeof(LambdaExpression)));
            return (int)collection.Provider.Execute(callMyself);
        }

        public static int Delete<T>(this IUpdatable<T> collection, Expression<Func<T, bool>> predicate) {
            MethodCallExpression callMyself = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), collection.Expression,
                                                              predicate != null ? (Expression)Expression.Quote(predicate) : Expression.Constant(null, typeof(Expression<Func<T, bool>>)));
            return (int)collection.Provider.Execute(callMyself);
        }

        public static IEnumerable Batch(IUpdatable collection, IEnumerable items, LambdaExpression fnOperation, int batchSize, bool stream) {
            MethodCallExpression callMyself = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()), collection.Expression, Expression.Constant(items),
                                                              fnOperation != null ? (Expression)Expression.Quote(fnOperation) : Expression.Constant(null, typeof(LambdaExpression)),
                                                              Expression.Constant(batchSize), Expression.Constant(stream));
            return (IEnumerable)collection.Provider.Execute(callMyself);
        }

        public static IEnumerable<TResult> Batch<TElement, TInstance, TResult>(this IUpdatable<TElement> collection, IEnumerable<TInstance> instances,
                                                                               Expression<Func<IUpdatable<TElement>, TInstance, TResult>> fnOperation, int batchSize, bool stream) {
            MethodCallExpression callMyself = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TElement), typeof(TInstance), typeof(TResult)),
                                                              collection.Expression, Expression.Constant(instances),
                                                              fnOperation != null
                                                                  ? (Expression)Expression.Quote(fnOperation)
                                                                  : Expression.Constant(null, typeof(Expression<Func<IUpdatable<TElement>, TInstance, TResult>>)), Expression.Constant(batchSize),
                                                              Expression.Constant(stream));
            return (IEnumerable<TResult>)collection.Provider.Execute(callMyself);
        }

        public static IEnumerable<TResult> Batch<TElement, TInstance, TResult>(this IUpdatable<TElement> collection, IEnumerable<TInstance> instances,
                                                                               Expression<Func<IUpdatable<TElement>, TInstance, TResult>> fnOperation) {
            return Batch(collection, instances, fnOperation, 50, false);
        }
    }
}