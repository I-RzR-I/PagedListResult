// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 20:26
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 21:54
// ***********************************************************************
//  <copyright file="FilterPredefinedTopRecordExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Extensions.Internal;
using PagedListResult.Common.Helpers;
using PagedListResult.Common.Helpers.Internal.Common;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace PagedListResult.Common.Extensions.Filters
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Predefined record extensions.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    public static class FilterPredefinedTopRecordExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Get top records by specified ids.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="source">.</param>
        /// <param name="ids">.</param>
        /// <param name="defaultPrimaryKeys">(Optional)</param>
        /// <returns>The in top predefined records.</returns>
        /// =================================================================================================
        public static IQueryable<TSource> GetInTopPredefinedRecords<TSource>(
            this IQueryable<TSource> source,
            ICollection<string> ids, ICollection<string> defaultPrimaryKeys = null) where TSource : class
        {
            if (source.IsNull())
                return Enumerable.Empty<TSource>().AsQueryable();

            var defaultProperty = ReflectionTypeStorage.GetDefaultDefaultTopRecordPrimaryKeyPropertyInfo(typeof(TSource));
            if (defaultProperty.IsSuccess.IsFalse())
                ThrowHelper.Exception(defaultProperty.GetFirstMessage());

            var propertyName = defaultProperty.Response.Name;

            defaultPrimaryKeys = (defaultPrimaryKeys ?? new List<string>()).Where(x => x.IsNotNull()).ToList();
            defaultPrimaryKeys = defaultPrimaryKeys.IsNullOrEmptyEnumerable() ? new[] { propertyName } : defaultPrimaryKeys;

            if (defaultPrimaryKeys.IsNullOrEmptyEnumerable())
                ThrowHelper.Exception(
                    $"Default property primary key must be not null or empty. Specify input param '{nameof(defaultPrimaryKeys)}' or add attribute 'PaginationDefaultTopRecordPrimaryKey' to PK property!");

            return source.GetPredefinedRecordsInTop(ids, defaultPrimaryKeys);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Get records excluded by specified ids.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="source">.</param>
        /// <param name="ids">.</param>
        /// <param name="defaultPrimaryKeys">(Optional)</param>
        /// <returns>The not in top predefined records.</returns>
        /// =================================================================================================
        public static IQueryable<TSource> GetNotInTopPredefinedRecords<TSource>(
            this IQueryable<TSource> source,
            ICollection<string> ids, ICollection<string> defaultPrimaryKeys = null) where TSource : class
        {
            if (source.IsNull())
                return Enumerable.Empty<TSource>().AsQueryable();

            var defaultProperty = ReflectionTypeStorage.GetDefaultDefaultTopRecordPrimaryKeyPropertyInfo(typeof(TSource));
            if (defaultProperty.IsSuccess.IsFalse())
                ThrowHelper.Exception(defaultProperty.GetFirstMessage());

            var propertyName = defaultProperty.Response.Name;

            defaultPrimaryKeys = (defaultPrimaryKeys ?? new List<string>()).Where(x => x.IsNotNull()).ToList();
            defaultPrimaryKeys = defaultPrimaryKeys.IsNullOrEmptyEnumerable() ? new[] { propertyName } : defaultPrimaryKeys;

            if (defaultPrimaryKeys.IsNullOrEmptyEnumerable())
                ThrowHelper.Exception(
                    $"Default property primary kay must be not null or empty. Specify input param '{nameof(defaultPrimaryKeys)}' or add attribute 'PaginationDefaultTopRecordPrimaryKey' to PK property!");

            return source.GetPredefinedRecordsInTop(ids, defaultPrimaryKeys, false);
        }
    }
}