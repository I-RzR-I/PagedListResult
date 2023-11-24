// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 16:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 21:53
// ***********************************************************************
//  <copyright file="FilterSourceByTextExpression.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Extensions.Internal;
using PagedListResult.Common.Helpers.Internal.Common;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace PagedListResult.Common.Extensions.Filters
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Filter source query by provider search text and/or searchProperties.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    public static class FilterSourceByTextExpression
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Filter source query by text.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="query">Required. Current query.</param>
        /// <param name="searchInAllTextFields">
        ///     Required. If set to <see langword="true" />, then search text will be applied in all
        ///     searchProperties; otherwise, only in specified.
        /// </param>
        /// <param name="searchText">Required. Search text.</param>
        /// <param name="searchProperties">(Optional) Required. Properties to search.</param>
        /// <param name="queryType">(Optional) Optional. The default value is null.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> FilterSourceByText<TSource>(
            this IQueryable<TSource> query,
            bool searchInAllTextFields, string searchText,
            ICollection<string> searchProperties = null, Type queryType = null) where TSource : class
        {
            query.ThrowIfNull("Current query can not be null!");

            if (searchText.IsNullOrEmpty())
                return query;

            if (searchInAllTextFields.IsFalse() && searchProperties.IsNullOrEmptyEnumerable())
                ThrowHelper.Exception($"{nameof(searchInAllTextFields)} is set to false, {nameof(searchProperties)} must have a values!");

            queryType ??= typeof(TSource);

            query = searchInAllTextFields.IsTrue()
                ? query.FilterByText(searchText, queryType)
                : query.FilterByText(searchText, searchProperties, queryType);

            return query;
        }
    }
}