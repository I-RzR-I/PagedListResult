// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 15:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 21:55
// ***********************************************************************
//  <copyright file="QueryPropFilterNullExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using PagedListResult.Common.Enums;
using PagedListResult.Common.Extensions.Internal;
using System;
using System.Linq;

#endregion

namespace PagedListResult.Common.Extensions.Filters.PropertyFilterQuery
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Null filter expressions.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    public static partial class QueryPropFilterExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'IS NULL'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Query source type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyIsNull<TSource>(
            this IQueryable<TSource> query,
            string propertyName, Type queryType = null) where TSource : class
            => query.CommonPropertyNullMethodInvoke(FilterType.IsNull, propertyName, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'IS NOT NULL'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Query source type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyIsNotNull<TSource>(
            this IQueryable<TSource> query,
            string propertyName, Type queryType = null) where TSource : class
            => query.CommonPropertyNullMethodInvoke(FilterType.IsNotNull, propertyName, queryType);
    }
}