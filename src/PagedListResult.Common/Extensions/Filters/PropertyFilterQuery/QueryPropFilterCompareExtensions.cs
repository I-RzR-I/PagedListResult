// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 15:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 22:00
// ***********************************************************************
//  <copyright file="QueryPropFilterCompareExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using PagedListResult.Common.Extensions.Internal;
using PagedListResult.DataModels.Enums;
using System;
using System.Linq;

#endregion

namespace PagedListResult.Common.Extensions.Filters.PropertyFilterQuery
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Expression filter compare extensions.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    public static partial class QueryPropFilterExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Greater than'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Query source type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Object value to compare.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyGreaterThan<TSource>(
            this IQueryable<TSource> query, string propertyName,
            object value, Type queryType = null) where TSource : class
            => query.CommonPropertyCompareMethodInvoke(FilterType.GreaterThan,
                propertyName, value, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Greater than or equals'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Query source type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Object value to compare.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyGreaterThanOrEquals<TSource>(
            this IQueryable<TSource> query, string propertyName,
            object value, Type queryType = null) where TSource : class
            => query.CommonPropertyCompareMethodInvoke(FilterType.GreaterThanOrEquals,
                propertyName, value, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Less than'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Query source type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Object value to compare.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyLessThan<TSource>(
            this IQueryable<TSource> query, string propertyName,
            object value, Type queryType = null) where TSource : class
            => query.CommonPropertyCompareMethodInvoke(FilterType.LessThan, propertyName, value, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Less than or equals'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Query source type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Object value to compare.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyLessThanOrEquals<TSource>(
            this IQueryable<TSource> query, string propertyName,
            object value, Type queryType = null) where TSource : class
            => query.CommonPropertyCompareMethodInvoke(FilterType.LessThanOrEquals, propertyName, value, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition filter between 2 values.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Query source type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="leftValue">Object value to compare.</param>
        /// <param name="rightValue">Object value to compare.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyBetween<TSource>(
            this IQueryable<TSource> query, string propertyName,
            object leftValue, object rightValue, Type queryType = null) where TSource : class
            => query.CommonPropertyBetweenMethodInvoke(propertyName, leftValue, rightValue, queryType);
    }
}