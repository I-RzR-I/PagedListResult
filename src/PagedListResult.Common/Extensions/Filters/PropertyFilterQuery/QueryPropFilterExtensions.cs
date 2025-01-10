// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 15:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 21:58
// ***********************************************************************
//  <copyright file="QueryPropFilterExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using PagedListResult.Common.Extensions.Internal;
using PagedListResult.Common.Helpers.Internal.ConstNamesHelper;
using PagedListResult.DataModels.Enums;
using System;
using System.Linq;

#endregion

namespace PagedListResult.Common.Extensions.Filters.PropertyFilterQuery
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Expression filter text extensions.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    public static partial class QueryPropFilterExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Equals'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyEquals<TSource>(
            this IQueryable<TSource> query, string propertyName,
            object value, Type queryType = null) where TSource : class
            => query.CommonPropertyMethodInvoke(FilterType.Equals, MethodInfoNamesHelper.EqualsMethodName,
                propertyName, value, false, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'NotEquals'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyNotEquals<TSource>(
            this IQueryable<TSource> query, string propertyName,
            object value, Type queryType = null) where TSource : class
            => query.CommonPropertyMethodInvoke(FilterType.NotEquals, MethodInfoNamesHelper.EqualsMethodName,
                propertyName, value, true, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Contains'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyContains<TSource>(
            this IQueryable<TSource> query, string propertyName,
            object value, Type queryType = null) where TSource : class
            => query.CommonPropertyContainsMethodInvoke(FilterType.Contains,
                propertyName, value, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Contains' case sensitive.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertySensitiveContains<TSource>(
            this IQueryable<TSource> query, string propertyName,
            object value, Type queryType = null) where TSource : class
            => query.CommonPropertyContainsMethodInvoke(FilterType.SensitiveContains,
                propertyName, value, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Does not contains'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyDoesNotContains<TSource>(
            this IQueryable<TSource> query, string propertyName,
            object value, Type queryType = null) where TSource : class
            => query.CommonPropertyContainsMethodInvoke(FilterType.DoesNotContains,
                propertyName, value, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Does not contains' case sensitive.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertySensitiveDoesNotContains<TSource>(
            this IQueryable<TSource> query, string propertyName,
            object value, Type queryType = null) where TSource : class
            => query.CommonPropertyContainsMethodInvoke(FilterType.SensitiveDoesNotContains,
                propertyName, value, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Starts with'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyStartsWith<TSource>(
            this IQueryable<TSource> query,
            string propertyName, object value, Type queryType = null) where TSource : class
            => query.CommonPropertyMethodInvoke(FilterType.StartsWith, MethodInfoNamesHelper.StartsWithMethodName,
                propertyName, value, false, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Does not starts with'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyDoesNotStartWith<TSource>(
            this IQueryable<TSource> query,
            string propertyName, object value, Type queryType = null) where TSource : class
            => query.CommonPropertyMethodInvoke(FilterType.DoesNotStartWith, MethodInfoNamesHelper.StartsWithMethodName,
                propertyName, value, true, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Ends with'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyEndsWith<TSource>(
            this IQueryable<TSource> query,
            string propertyName, object value, Type queryType = null) where TSource : class
            => query.CommonPropertyMethodInvoke(FilterType.EndsWith, MethodInfoNamesHelper.EndsWithMethodName,
                propertyName, value, false, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'Does not ends with'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyDoesNotEndsWith<TSource>(
            this IQueryable<TSource> query,
            string propertyName, object value, Type queryType = null) where TSource : class
            => query.CommonPropertyMethodInvoke(FilterType.DoesNotEndsWith, MethodInfoNamesHelper.EndsWithMethodName,
                propertyName, value, true, queryType);
    }
}