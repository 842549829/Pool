﻿#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.Aggregator.cs
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

namespace Izual.Data.Common {
    public static class Aggregator {
        /// <summary>
        /// Get a function that coerces a sequence of one type into another type. This is primarily used for aggregators stored in ProjectionExpression's, which are used to represent the final transformation of the entire result set of a query.
        /// </summary>
        /// <param name="expectedType"> </param>
        /// <param name="projector"> </param>
        /// <returns> </returns>
        public static LambdaExpression GetAggregator(Type expectedType, Type actualType) {
            Type actualElementType = TypeHelper.GetElementType(actualType);
            if(!expectedType.IsAssignableFrom(actualType)) {
                Type expectedElementType = TypeHelper.GetElementType(expectedType);
                ParameterExpression p = Expression.Parameter(actualType, "p");
                Expression body = null;
                if(expectedType.IsAssignableFrom(actualElementType)) {
                    body = Expression.Call(typeof(Enumerable), "SingleOrDefault", new[] {actualElementType}, p);
                }
                else if(expectedType.IsGenericType &&
                        (expectedType == typeof(IQueryable) || expectedType == typeof(IOrderedQueryable) || expectedType.GetGenericTypeDefinition() == typeof(IQueryable<>) ||
                         expectedType.GetGenericTypeDefinition() == typeof(IOrderedQueryable<>))) {
                    body = Expression.Call(typeof(Queryable), "AsQueryable", new[] {expectedElementType}, CoerceElement(expectedElementType, p));
                    if(body.Type != expectedType) {
                        body = Expression.Convert(body, expectedType);
                    }
                }
                else if(expectedType.IsArray && expectedType.GetArrayRank() == 1) {
                    body = Expression.Call(typeof(Enumerable), "ToArray", new[] {expectedElementType}, CoerceElement(expectedElementType, p));
                }
                else if(expectedType.IsGenericType && expectedType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IList<>))) {
                    Type gt = typeof(DeferredList<>).MakeGenericType(expectedType.GetGenericArguments());
                    ConstructorInfo cn = gt.GetConstructor(new[] {typeof(IEnumerable<>).MakeGenericType(expectedType.GetGenericArguments())});
                    body = Expression.New(cn, CoerceElement(expectedElementType, p));
                }
                else if(expectedType.IsAssignableFrom(typeof(List<>).MakeGenericType(actualElementType))) {
                    // List<T> can be assigned to expectedType
                    body = Expression.Call(typeof(Enumerable), "ToList", new[] {expectedElementType}, CoerceElement(expectedElementType, p));
                }
                else {
                    // some other collection type that has a constructor that takes IEnumerable<T>
                    ConstructorInfo ci = expectedType.GetConstructor(new[] {actualType});
                    if(ci != null) {
                        body = Expression.New(ci, p);
                    }
                }
                if(body != null) {
                    return Expression.Lambda(body, p);
                }
            }
            return null;
        }

        private static Expression CoerceElement(Type expectedElementType, Expression expression) {
            Type elementType = TypeHelper.GetElementType(expression.Type);
            if(expectedElementType != elementType && (expectedElementType.IsAssignableFrom(elementType) || elementType.IsAssignableFrom(expectedElementType))) {
                return Expression.Call(typeof(Enumerable), "Cast", new[] {expectedElementType}, expression);
            }
            return expression;
        }
    }
}