// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 16:40
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 22:12
// ***********************************************************************
//  <copyright file="QueryableFilterOrderExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Helpers;
using PagedListResult.Common.Helpers.Internal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace PagedListResult.Common.Extensions.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Query filter order extensions.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    internal static class QueryableFilterOrderExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Builds the Queryable functions using a TSource property name.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="T">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="comparer">(Optional) Comparer.</param>
        /// <param name="type">(Optional) Type of query.</param>
        /// <returns>An IOrderedQueryable&lt;T&gt;</returns>
        /// =================================================================================================
        internal static IOrderedQueryable<T> CallOrderedQueryable<T>(
            this IQueryable<T> query, string methodName,
            string propertyName,
            IComparer<object> comparer = null, Type type = null)
        {
            type ??= typeof(T);
            propertyName = propertyName.FirstCharToUpper();
            var property = ReflectionTypeStorage.GetDefaultOrderPropertyByName(type, propertyName);
            if (property.IsSuccess.IsFalse())
                ThrowHelper.Exception(property.GetFirstMessage());

            if (property.Response.IsNull())
                return query.OrderBy(x => 0);

            var param = Expression.Parameter(type, "x");
            var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            return comparer.IsNotNull()
                ? (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { type, body.Type },
                        query.Expression,
                        Expression.Lambda(body, param),
                        Expression.Constant(comparer)
                    )
                )
                : (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { type, body.Type },
                        query.Expression,
                        Expression.Lambda(body, param)
                    )
                );
        }
    }
}