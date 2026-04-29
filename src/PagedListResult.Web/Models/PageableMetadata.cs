// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-26 21:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 22:56
// ***********************************************************************
//  <copyright file="PageableMetadata.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.Domain.Text;
using System;
using System.Collections.Generic;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Per-type allow-list metadata controlling which property names may be used by the 
    ///     query-string binder for sort, filter, and search operations.
    /// </summary>
    /// =================================================================================================
    public sealed class PageableMetadata
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) a filter specifying the allowed.
        /// </summary>
        /// =================================================================================================
        private readonly HashSet<string> _allowedFilter;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the allowed search.
        /// </summary>
        /// =================================================================================================
        private readonly HashSet<string> _allowedSearch;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the allowed sort.
        /// </summary>
        /// =================================================================================================
        private readonly HashSet<string> _allowedSort;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="PageableMetadata"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="type">The type.</param>
        /// <param name="allowedSort">The allowed sort.</param>
        /// <param name="allowedFilter">The allowed filter.</param>
        /// <param name="allowedSearch">The allowed search.</param>
        /// =================================================================================================
        internal PageableMetadata(
            Type type,
            HashSet<string> allowedSort,
            HashSet<string> allowedFilter,
            HashSet<string> allowedSearch)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            _allowedSort = allowedSort ?? throw new ArgumentNullException(nameof(allowedSort));
            _allowedFilter = allowedFilter ?? throw new ArgumentNullException(nameof(allowedFilter));
            _allowedSearch = allowedSearch ?? throw new ArgumentNullException(nameof(allowedSearch));
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The CLR type these metadata describe.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        /// =================================================================================================
        public Type Type { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Properties allowed in <c>order=</c>.
        /// </summary>
        /// <value>
        ///     The allowed sort.
        /// </value>
        /// =================================================================================================
        public IReadOnlyCollection<string> AllowedSort
            => _allowedSort;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Properties allowed in <c>filter=</c>.
        /// </summary>
        /// <value>
        ///     The allowed filter.
        /// </value>
        /// =================================================================================================
        public IReadOnlyCollection<string> AllowedFilter 
            => _allowedFilter;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Properties allowed in <c>search=</c>.
        /// </summary>
        /// <value>
        ///     The allowed search.
        /// </value>
        /// =================================================================================================
        public IReadOnlyCollection<string> AllowedSearch 
            => _allowedSearch;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns <c>true</c> if <paramref name="propertyName" /> may be used for sort.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///     True if sort allowed, false if not.
        /// </returns>
        /// =================================================================================================
        public bool IsSortAllowed(string propertyName)
            => propertyName.IsPresent() && _allowedSort.Contains(propertyName);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns <c>true</c> if <paramref name="propertyName" /> may be used for filter.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///     True if filter allowed, false if not.
        /// </returns>
        /// =================================================================================================
        public bool IsFilterAllowed(string propertyName)
            => propertyName.IsPresent() && _allowedFilter.Contains(propertyName);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns <c>true</c> if <paramref name="propertyName" /> may be used for search.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///     True if search allowed, false if not.
        /// </returns>
        /// =================================================================================================
        public bool IsSearchAllowed(string propertyName) 
            => propertyName.IsPresent() && _allowedSearch.Contains(propertyName);
    }
}