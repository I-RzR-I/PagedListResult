// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 15:37
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-02 15:57
// ***********************************************************************
//  <copyright file="SourceSearchFilterExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Helpers.Internal.Builder;
using PagedListResult.Common.Helpers.Internal.Common;

#endregion

namespace PagedListResult.Common.Extensions.Internal
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Source query filter by search.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    internal static class SourceSearchFilterExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Filter objects by text expression.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="source">.</param>
        /// <param name="toSearch">.</param>
        /// <param name="type">(Optional) Optional.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        ///=================================================================================================
        internal static IQueryable<TSource> FilterByText<TSource>(
            this IQueryable<TSource> source,
            string toSearch, Type type = null)
        {
            if (toSearch.IsNullOrEmpty())
                return source;

            var predicate = SearchPredicateBuilder.CreateTextFilterExpression<TSource>(toSearch, type);
            if (predicate.IsSuccess.IsFalse())
                ThrowHelper.Exception(predicate.GetFirstMessage());

            return predicate.IsNull() 
                ? source
                : source.Where(predicate.Response);
        }
        internal static IQueryable<TSource> FilterByTextAllTypes<TSource>(
            this IQueryable<TSource> source,
            string toSearch, Type type = null)
        {
            if (toSearch.IsNullOrEmpty())
                return source;

            var predicate = SearchPredicateBuilder.CreateTextFilterExpressionAllTypes<TSource>(toSearch, type);
            if (predicate.IsSuccess.IsFalse())
                ThrowHelper.Exception(predicate.GetFirstMessage());

            return predicate.IsNull() 
                ? source
                : source.Where(predicate.Response);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Filter objects by text expression on specified fields.
        /// </summary>
        /// <remarks>
        ///     RzR, 14-Nov-23.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="toSearch">The search string/text.</param>
        /// <param name="properties">Properties/fields where is applied search value.</param>
        /// <param name="type">(Optional) Optional.</param>
        /// <returns>
        ///     An IQueryable&lt;TSource&gt;
        /// </returns>
        /// =================================================================================================
        internal static IQueryable<TSource> FilterByText<TSource>(
            this IQueryable<TSource> source,
            string toSearch, ICollection<string> properties, Type type = null)
        {
            if (toSearch.IsNullOrEmpty())
                return source;

            if (properties.IsNullOrEmptyEnumerable())
                throw new ArgumentNullException(nameof(properties));

            var predicate = SearchPredicateBuilder.CreateTextFilterExpression<TSource>(toSearch, properties, type);
            if (predicate.IsSuccess.IsFalse())
                ThrowHelper.Exception(predicate.GetFirstMessage());

            return predicate.Response.IsNull() 
                ? source 
                : source.Where(predicate.Response);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;TSource&gt; extension method that filter by text all fields types.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="toSearch">The search string/text.</param>
        /// <param name="properties">Properties/fields where is applied search value.</param>
        /// <param name="type">(Optional) Optional.</param>
        /// <returns>
        ///     An IQueryable&lt;TSource&gt;
        /// </returns>
        /// =================================================================================================
        internal static IQueryable<TSource> FilterByTextAllTypes<TSource>(
            this IQueryable<TSource> source,
            string toSearch, ICollection<string> properties, Type type = null)
        {
            if (toSearch.IsNullOrEmpty())
                return source;

            if (properties.IsNullOrEmptyEnumerable())
                throw new ArgumentNullException(nameof(properties));

            var predicate = SearchPredicateBuilder.CreateTextFilterExpressionAllTypes<TSource>(toSearch, properties, type);
            if (predicate.IsSuccess.IsFalse())
                ThrowHelper.Exception(predicate.GetFirstMessage());

            return predicate.Response.IsNull() 
                ? source 
                : source.Where(predicate.Response);
        }
    }
}