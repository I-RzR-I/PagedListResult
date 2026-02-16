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
using DomainCommonExtensions.Utilities.Ensure;
using PagedListResult.Common.Extensions.Internal;
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
        ///     properties(string/text) defined in the data model; otherwise, only in specified. <br />
        ///     The text will be searched only in the string/text fields or only in specified fields.
        /// </param>
        /// <param name="searchInAllFields">
        ///     Required. If set to <see langword="true" />, then search text will be applied in all
        ///     properties defined in the data model; otherwise, only in specified. <br />
        ///     The text will be searched in data model fields or only in specified fields.
        /// </param>
        /// <param name="searchText">Required. Search text.</param>
        /// <param name="searchProperties">(Optional) Required. Properties to search.</param>
        /// <param name="queryType">(Optional) Optional. The default value is null.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        public static IQueryable<TSource> FilterSourceByText<TSource>(
            this IQueryable<TSource> query,
            bool searchInAllTextFields, 
            bool searchInAllFields, 
            string searchText,
            ICollection<string> searchProperties = null, 
            Type queryType = null) where TSource : class
        {
            query.ThrowIfNull("Current query can not be null!");

            if (searchText.IsNullOrEmpty())
                return query;
            
            queryType ??= typeof(TSource);

            if ((searchInAllFields.IsTrue() && searchInAllTextFields.IsTrue()) || searchInAllFields.IsTrue())
            {
                query = searchProperties.IsNullOrEmptyEnumerable()
                    ? query.FilterByTextAllTypes(searchText, queryType)
                    : query.FilterByTextAllTypes(searchText, searchProperties, queryType);
            }
            else
            {
                query = searchProperties.IsNullOrEmptyEnumerable()
                    ? query.FilterByText(searchText, queryType)
                    : query.FilterByText(searchText, searchProperties, queryType);
            }

            return query;
        }
    }
}