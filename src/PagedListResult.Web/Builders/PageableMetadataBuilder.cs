// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-26 21:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 23:01
// ***********************************************************************
//  <copyright file="PageableMetadataBuilder.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Text;
using RzR.ResultMessage.Pagination.AspNetCore.Models;
using System;
using System.Collections.Generic;

#endregion

namespace RzR.ResultMessage.Pagination.AspNetCore.Builders
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Fluent builder for <see cref="PageableMetadata" />.
    /// </summary>
    /// <typeparam name="T">The type whose allow-list is being configured.</typeparam>
    /// =================================================================================================
    public sealed class PageableMetadataBuilder<T> where T : class
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) specifies the filter.
        /// </summary>
        /// =================================================================================================
        private readonly HashSet<string> _filter = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the search.
        /// </summary>
        /// =================================================================================================
        private readonly HashSet<string> _search = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the sort.
        /// </summary>
        /// =================================================================================================
        private readonly HashSet<string> _sort = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds property names allowed in <c>order=</c>.
        /// </summary>
        /// <param name="propertyNames">
        ///     A variable-length parameters list containing property names.
        /// </param>
        /// <returns>
        ///     A PageableMetadataBuilder&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public PageableMetadataBuilder<T> AllowSort(params string[] propertyNames)
        {
            AddRange(_sort, propertyNames);

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds property names allowed in <c>filter=</c>.
        /// </summary>
        /// <param name="propertyNames">
        ///     A variable-length parameters list containing property names.
        /// </param>
        /// <returns>
        ///     A PageableMetadataBuilder&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public PageableMetadataBuilder<T> AllowFilter(params string[] propertyNames)
        {
            AddRange(_filter, propertyNames);

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds property names allowed in <c>search=</c>.
        /// </summary>
        /// <param name="propertyNames">
        ///     A variable-length parameters list containing property names.
        /// </param>
        /// <returns>
        ///     A PageableMetadataBuilder&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public PageableMetadataBuilder<T> AllowSearch(params string[] propertyNames)
        {
            AddRange(_search, propertyNames);

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the build.
        /// </summary>
        /// <returns>
        ///     A PageableMetadata.
        /// </returns>
        /// =================================================================================================
        internal PageableMetadata Build() => new PageableMetadata(typeof(T), _sort, _filter, _search);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds a range to 'values'.
        /// </summary>
        /// <param name="target">Target for the.</param>
        /// <param name="values">The values.</param>
        /// =================================================================================================
        private static void AddRange(HashSet<string> target, string[] values)
        {
            if (values.IsNullOrEmptyEnumerable())
                return;

            foreach (var v in values)
            {
                if (v.IsPresent())
                    target.Add(v.Trim());
            }
        }
    }
}