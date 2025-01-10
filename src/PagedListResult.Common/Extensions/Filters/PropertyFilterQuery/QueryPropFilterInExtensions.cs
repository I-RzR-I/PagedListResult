// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 15:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 21:56
// ***********************************************************************
//  <copyright file="QueryPropFilterInExtensions.cs" company="">
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
using System.Collections.Generic;
using System.Linq;

#endregion

namespace PagedListResult.Common.Extensions.Filters.PropertyFilterQuery
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Expression filter in/notin extensions.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    public static partial class QueryPropFilterExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'IN'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="values">Property values.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyIsIn<TSource>(
            this IQueryable<TSource> query,
            string propertyName, IEnumerable<object> values, Type queryType = null) where TSource : class
            => query.CommonPropertyInNotInMethodInvoke(FilterType.IsIn, propertyName, values, queryType);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build property condition 'NOT IN'.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="values">Property values.</param>
        /// <param name="queryType">(Optional) Query type.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> PropertyIsNotIn<TSource>(
            this IQueryable<TSource> query,
            string propertyName, IEnumerable<object> values, Type queryType = null) where TSource : class
            => query.CommonPropertyInNotInMethodInvoke(FilterType.IsNotIn, propertyName, values, queryType);
    }
}