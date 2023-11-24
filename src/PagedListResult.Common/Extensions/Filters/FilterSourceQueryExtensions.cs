// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-04 14:33
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-13 16:50
// ***********************************************************************
//  <copyright file="FilterSourceQueryExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Enums;
using PagedListResult.Common.Extensions.Internal.Common;
using PagedListResult.Common.Helpers.Internal.Builder;
using PagedListResult.Common.Helpers.Internal.Common;
using PagedListResult.Common.Models.Request;
using System.Collections.Generic;
using System.Linq;

#endregion

// ReSharper disable PossibleMultipleEnumeration

namespace PagedListResult.Common.Extensions.Filters
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Filter query extensions.</summary>
    /// <remarks>RzR, 13-Nov-23.</remarks>
    /// =================================================================================================
    public static class FilterSourceQueryExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     As a simple filtrable query. In the current query, it will be applied only base filter.
        ///     Dependencies will not be included.
        /// </summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="filters">Query filters.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> AsSimpleFilterable<TSource>(this IQueryable<TSource> query,
            IEnumerable<DataFilter> filters) where TSource : class
        {
            if (filters.IsNullOrEmptyEnumerable())
                return query;

            foreach (var filter in filters)
            {
                if (filter.IsNotNullOrDefault())
                {
                    var simpleFilter = QuerySourceFiltrableBuilder.BuildSimplePropFilterQuery(query, filter.FilterValue.Condition,
                        filter.FilterValue.PropertyName, filter.FilterValue.Values, new[] { filter.FilterValue.CompareValue });
                    if (simpleFilter.IsSuccess.IsFalse())
                        ThrowHelper.Exception(simpleFilter.GetFirstMessage());

                    query = simpleFilter.Response;
                }
            }

            return query;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     As a filtrable query. In the current query, it will be applied all filters.
        ///     Dependencies will be included.
        /// </summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="filters">Query filters.</param>
        /// <param name="filterLink">(Optional) Main filters link condition.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> AsFilterable<TSource>(this IQueryable<TSource> query,
            IEnumerable<DataFilter> filters, FilterConditionType filterLink = FilterConditionType.And) where TSource : class
        {
            var filter = QuerySourceFiltrableBuilder.BuildFilterableQuery(query, filters, filterLink);
            if (filter.IsSuccess.IsFalse())
                ThrowHelper.Exception(filter.GetFirstMessage());

            return filter.Response;
        }
    }
}