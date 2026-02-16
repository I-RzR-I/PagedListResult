// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 15:27
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 22:11
// ***********************************************************************
//  <copyright file="QueryableParamPropExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.CommonExtensions.Reflection;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Utilities.Ensure;
using PagedListResult.Common.Extensions.Internal.Common;
using PagedListResult.Common.Helpers.Internal.Builder;
using PagedListResult.Common.Helpers.Internal.Common;
using PagedListResult.Common.Helpers.Internal.ConstNamesHelper;
using PagedListResult.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace PagedListResult.Common.Extensions.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>A queryable parameter property extensions.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    internal static class QueryableParamPropExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Property invoke method.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="query">.</param>
        /// <param name="filter">.</param>
        /// <param name="methodName">.</param>
        /// <param name="propertyName">.</param>
        /// <param name="value">.</param>
        /// <param name="negate">
        ///     (Optional) Optionally. If set to true then expression will be negate.
        /// </param>
        /// <param name="queryType">(Optional)</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        internal static IQueryable<TSource> CommonPropertyMethodInvoke<TSource>(
            this IQueryable<TSource> query,
            FilterType filter, string methodName, string propertyName, object value, bool negate = false, Type queryType = null)
        {
            if (value.IsNull() && methodName != MethodInfoNamesHelper.EqualsMethodName)
                return query;

            var propParamData = QueryableParamPropBuilder.GetQueryableParamProp<TSource>(propertyName, queryType);
            if (propParamData.IsSuccess.IsFalse())
                ThrowHelper.Exception(propParamData.GetFirstMessage());

            var property = propParamData.Response.PropertyExpression;
            var parameter = propParamData.Response.ParameterExpression;

            if (property.Type.IsAbstract)
                return query;

            if (value.IsNull() && methodName == MethodInfoNamesHelper.EqualsMethodName)
            {
                var nullBinary = negate.IsTrue()
                    ? property.BuildCompareBinaryBodyByFilter<TSource>(FilterType.IsNotNull)
                    : property.BuildCompareBinaryBodyByFilter<TSource>(FilterType.IsNull);
                var nullExpr = Expression.Lambda<Func<TSource, bool>>(nullBinary, parameter);

                return query.Where(nullExpr);
            }

            var body = property.BuildCompareBinaryBodyByFilter<TSource>(filter, new[] { value });
            query = query.Where(Expression.Lambda<Func<TSource, bool>>(body, parameter));

            return query;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build Property contains invoke method.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="query">.</param>
        /// <param name="filter">.</param>
        /// <param name="propertyName">.</param>
        /// <param name="value">.</param>
        /// <param name="queryType">(Optional)</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        internal static IQueryable<TSource> CommonPropertyContainsMethodInvoke<TSource>(
            this IQueryable<TSource> query,
            FilterType filter, string propertyName, object value, Type queryType = null)
        {
            if (value.IsNull())
                return query;

            var propParamData = QueryableParamPropBuilder.GetQueryableParamProp<TSource>(propertyName, queryType);
            if (propParamData.IsSuccess.IsFalse())
                ThrowHelper.Exception(propParamData.GetFirstMessage());

            var property = propParamData.Response.PropertyExpression;
            var parameter = propParamData.Response.ParameterExpression;
            var propertyType = propParamData.Response.PropertyExpression.Type;

            if (!propertyType.IsStringType())
                ThrowHelper.NotSupportedFilterException(filter, propertyType.Name, $"{filter} is supported only for string types.");

            var body = property.BuildCompareBinaryBodyByFilter<TSource>(filter, new[] { value });
            query = query.Where(Expression.Lambda<Func<TSource, bool>>(body, parameter));

            return query;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;TSource&gt; extension method that common property null method invoke.
        /// </summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="query">Required.</param>
        /// <param name="filter">.</param>
        /// <param name="propertyName">Required.</param>
        /// <param name="queryType">(Optional) Optional. The default value is null.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        internal static IQueryable<TSource> CommonPropertyNullMethodInvoke<TSource>(
            this IQueryable<TSource> query,
            FilterType filter, string propertyName, Type queryType = null)
        {
            var propParamData = QueryableParamPropBuilder.GetQueryableParamProp<TSource>(propertyName, queryType);
            if (propParamData.IsSuccess.IsFalse())
                ThrowHelper.Exception(propParamData.GetFirstMessage());

            var property = propParamData.Response.PropertyExpression;
            var parameter = propParamData.Response.ParameterExpression;
            var propertyType = propParamData.Response.PropertyExpression.Type;

            if (propertyType.IsAbstract)
                return query;

            var body = property.BuildCompareBinaryBodyByFilter<TSource>(filter);

            return query.Where(Expression.Lambda<Func<TSource, bool>>(body, parameter));
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;TSource&gt; extension method that common property compare method
        ///     invoke.
        /// </summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="query">Required.</param>
        /// <param name="filter">Required.</param>
        /// <param name="propertyName">Required.</param>
        /// <param name="value">Required.</param>
        /// <param name="queryType">(Optional) Optional. The default value is null.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        internal static IQueryable<TSource> CommonPropertyCompareMethodInvoke<TSource>(
            this IQueryable<TSource> query,
            FilterType filter, string propertyName, object value, Type queryType = null)
        {
            if (value.IsNull())
                return query;

            var propParamData = QueryableParamPropBuilder.GetQueryableParamProp<TSource>(propertyName, queryType);
            if (propParamData.IsSuccess.IsFalse())
                ThrowHelper.Exception(propParamData.GetFirstMessage());

            var property = propParamData.Response.PropertyExpression;
            var parameter = propParamData.Response.ParameterExpression;
            var propertyType = propParamData.Response.PropertyExpression.Type;

            if (propertyType.IsStringType() || propertyType.IsBoolPropType())
                ThrowHelper.NotSupportedFilterException(filter, propertyType.Name);

            if (propertyType.IsAbstract)
                return query;

            var compareObj = ExpressionObjectCompareHelper.GenerateObjectCompare(property, value);
            if (compareObj.IsSuccess.IsFalse())
                ThrowHelper.Exception(compareObj.GetFirstMessage());

            var bodyExpression = property.BuildCompareBinaryBodyByFilter<TSource>(filter, new[] { compareObj.Response });
            var predicateExpression = Expression.Lambda<Func<TSource, bool>>(bodyExpression, parameter);

            return query.Where(predicateExpression);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Filter collection by property name between 2 values.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Query source type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="leftValue">Object value to compare.</param>
        /// <param name="rightValue">Object value to compare.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        internal static IQueryable<TSource> CommonPropertyBetweenMethodInvoke<TSource>(
            this IQueryable<TSource> query,
            string propertyName, object leftValue, object rightValue, Type queryType = null)
        {
            if (leftValue.IsNull() || rightValue.IsNull())
                return query;

            var propParamData = QueryableParamPropBuilder.GetQueryableParamProp<TSource>(propertyName, queryType);
            if (propParamData.IsSuccess.IsFalse())
                ThrowHelper.Exception(propParamData.GetFirstMessage());

            var property = propParamData.Response.PropertyExpression;
            var parameter = propParamData.Response.ParameterExpression;
            var propertyType = propParamData.Response.PropertyExpression.Type;

            if (propertyType.IsStringType() || propertyType.IsBoolPropType())
                ThrowHelper.NotSupportedFilterException(FilterType.Between, propertyType.Name);

            var bodyExpression = property.BuildCompareBinaryBodyByFilter<TSource>(
                FilterType.Between, new[] { leftValue }, new[] { rightValue }, parameter);

            var predicateExpression = Expression.Lambda<Func<TSource, bool>>(bodyExpression, parameter);

            return query.Where(predicateExpression);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;TSource&gt; extension method that common property in not in method
        ///     invoke.
        /// </summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="query">Required.</param>
        /// <param name="filter">Required.</param>
        /// <param name="propertyName">Required.</param>
        /// <param name="values">Required.</param>
        /// <param name="queryType">(Optional) Optional. The default value is null.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        internal static IQueryable<TSource> CommonPropertyInNotInMethodInvoke<TSource>(
            this IQueryable<TSource> query,
            FilterType filter, string propertyName, IEnumerable<object> values, Type queryType = null)
        {
            var enumerable = values as object[] ?? values.ToArray();
            if (enumerable.IsNull() || !enumerable.Any() || enumerable.Any(x => x.IsNull() || ((string)x).IsNullOrEmpty()))
                return query;

            var propParamData = QueryableParamPropBuilder.GetQueryableParamProp<TSource>(propertyName, queryType);
            if (propParamData.IsSuccess.IsFalse())
                ThrowHelper.Exception(propParamData.GetFirstMessage());

            var property = propParamData.Response.PropertyExpression;
            var parameter = propParamData.Response.ParameterExpression;

            var body = property.BuildCompareBinaryBodyByFilter<TSource>(filter, enumerable);
            var predicate = Expression.Lambda<Func<TSource, bool>>(body, parameter);
            predicate.ThrowIfArgNull(nameof(predicate));

            return query.Where(predicate!);
        }
    }
}