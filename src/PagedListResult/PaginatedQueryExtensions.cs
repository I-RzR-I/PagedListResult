// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2023-11-02 15:17
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-10 00:53
// ***********************************************************************
//  <copyright file="PaginatedQueryExtensions.cs" company="">
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
using Microsoft.EntityFrameworkCore;
using MockAsyncEnumerable;
using PagedListResult.Common.Extensions.Filters;
using PagedListResult.Common.Helpers;
using PagedListResult.Common.Helpers.Internal.Common;
using PagedListResult.DataModels.Enums;
using PagedListResult.DataModels.Models.Request;
using PagedListResult.DataModels.Models.Request.Page;
using PagedListResult.DataModels.Models.Result;
using PagedListResult.Extensions;
using PagedListResult.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace PagedListResult
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Paginated query extensions.</summary>
    /// <remarks>RzR, 10-Nov-23.</remarks>
    ///=================================================================================================
    public static class PaginatedQueryExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>An IQueryable&lt;TSource&gt; extension method that gets a paged.</summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TPageRequest">Type of the page request.</typeparam>
        /// <param name="query">The query to act on.</param>
        /// <param name="request">The request.</param>
        /// <param name="defaultPrimaryKey">(Optional) The default primary key.</param>
        /// <returns>The paged.</returns>
        ///=================================================================================================
        public static PagedResult<TSource> GetPaged<TSource, TPageRequest>(
            this IQueryable<TSource> query, TPageRequest request,
            DefaultPrimaryKeyDefinition defaultPrimaryKey = null)
            where TSource : class
            where TPageRequest : PagedRequest
        {
            if (query.IsNull())
                ThrowHelper.ArgumentNullException("Invalid query");

            if (request.IsNull())
                ThrowHelper.ArgumentNullException("Request can't be null");

            var watch = TimeWatchHelper.Instance();
            watch.StartNew();

            var queryType = typeof(TSource);
            query = query.FilterSourceByText(request.Search.SearchInAllTextFields, request.Search.Search,
                request.Search.CustomSearchTextProperties, queryType);

            var result = new PagedResult<TSource>
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                RowCount = query.Count()
            };

            if (request.Order.IsNotNull())
            {
                if (!request.Order.OrderByProperty.IsNullOrEmpty())
                {
                    query = query.OrderByWithDirection(request.Order.OrderByProperty, request.Order.OrderDirection,
                        null, queryType, request.Order.OrderByDefaultProperty);
                }
                else if (request.Order.OrderByDefaultProperty.IsTrue())
                {
                    var defaultOrderProperty = ReflectionTypeStorage.GetDefaultOrderByPropertyInfo(queryType);
                    if(defaultOrderProperty.IsSuccess.IsFalse())
                        ThrowHelper.Exception(defaultOrderProperty.GetFirstMessage());

                    if (defaultOrderProperty.Response != null)
                        query = query.OrderByWithDirectionByDefaultProperty(request.Order.OrderDirection, null, queryType);
                }
                else
                {
                    query = query.OrderBy(x => 0);
                }
            }

            var pageCount = (double)result.RowCount / request.PageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var skip = (request.Page - 1) * request.PageSize;

            /*
             * If skip record == 0, then execute commands that allow and set
             * specified record ids in the top of list
             * otherwise execute standard command for get paged result data
             */
            if (skip.IsLessOrEqualZero())
            {
                if (defaultPrimaryKey.IsNotNull())
                {
                    var defaultKeys = query.GetDefaultPrimaryKeyProp(defaultPrimaryKey);
                    var hasIds = request.PredefinedRecords.IsNotNull()
                                 && request.PredefinedRecords.Any()
                                 && request.PredefinedRecords.All(x => !string.IsNullOrEmpty(x));
                    var idCount = hasIds.IsTrue() ? request.PredefinedRecords?.Count ?? 0 : 0;
                    var res = new List<TSource>();
                    if (hasIds.IsTrue())
                    {
                        var topXRecords = query.GetInTopPredefinedRecords(request.PredefinedRecords, defaultKeys)
                            .ToList();
                        res.AddRange(topXRecords);

                        var mainRecords = query
                            .GetNotInTopPredefinedRecords(request.PredefinedRecords, defaultKeys)
                            .Skip(skip)
                            .Take(request.PageSize - idCount)
                            .ToList();

                        res.AddRange(mainRecords);

                        result.Response = res;
                    }
                    else
                    {
                        result.Response = query.GetPageRecordList(skip, request.PageSize);
                    }
                }
                else
                {
                    result.Response = query.GetPageRecordList(skip, request.PageSize);
                }
            }
            else
            {
                result.Response = query.GetPageRecordList(skip, request.PageSize);
            }

            if (!request.Fields.IsNullOrEmptyEnumerable())
            {
                var fieldsCorrectNames = request.Fields.Select(field => field.FirstCharToUpper());
                var parsedRecords = result.Response.ParseEnumerableOfTInDynamic(fieldsCorrectNames);

                result.Response = JsonSerializer.Deserialize<List<TSource>>(JsonSerializer.Serialize(parsedRecords));
            }

            result.ExecutionDetails.SetExecutionTimeMs(watch.Stop(), DateTime.Now);
            result.IsSuccess = true;

            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>An IQueryable&lt;TSource&gt; extension method that gets a paged.</summary>
        /// <remarks>RzR, 12-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="query">The query to act on.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>The paged.</returns>
        ///=================================================================================================
        public static PagedResult<TSource> GetPaged<TSource>(
            this IQueryable<TSource> query, int page, int pageSize)
            where TSource : class
        {
            if (query.IsNull())
                ThrowHelper.ArgumentNullException("Invalid query");

            if (page.IsNull() || page.IsLessOrEqualZero())
                ThrowHelper.ArgumentException("Page number must be greater on equals with 1");

            if (pageSize.IsNull() || pageSize.IsLessOrEqualZero())
                ThrowHelper.ArgumentException("Page size must be greater on equals with 1");

            var watch = TimeWatchHelper.Instance();
            watch.StartNew();

            var result = new PagedResult<TSource>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Response = query
                .Skip(skip).Take(pageSize)
                .ToList();

            result.ExecutionDetails.SetExecutionTimeMs(watch.Stop(), DateTime.Now);
            result.IsSuccess = true;

            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;TSource&gt; extension method that gets paged asynchronous.
        /// </summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="query">The query to act on.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// <returns>The paged async&lt; t source&gt;</returns>
        ///=================================================================================================
        public static async Task<PagedResult<TSource>> GetPagedAsync<TSource>(
            this IQueryable<TSource> query, int page, int pageSize, CancellationToken cancellationToken = default)
            where TSource : class
        {
            if (query.IsNull())
                ThrowHelper.ArgumentNullException("Invalid query");

            if (page.IsNull() || page.IsLessOrEqualZero())
                ThrowHelper.ArgumentException("Page number must be greater on equals with 1");

            if (pageSize.IsNull() || pageSize.IsLessOrEqualZero())
                ThrowHelper.ArgumentException("Page size must be greater on equals with 1");

            var watch = TimeWatchHelper.Instance();
            watch.StartNew();

            var result = new PagedResult<TSource>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Response = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            result.ExecutionDetails.SetExecutionTimeMs(watch.Stop(), DateTime.Now);
            result.IsSuccess = true;

            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;TSource&gt; extension method that gets paged asynchronous.
        /// </summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TPageRequest">Type of the page request.</typeparam>
        /// <param name="query">The query to act on.</param>
        /// <param name="request">The request.</param>
        /// <param name="defaultPrimaryKey">(Optional) The default primary key.</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// <returns>The paged async&lt; t source&gt;</returns>
        ///=================================================================================================
        public static async Task<PagedResult<TSource>> GetPagedAsync<TSource, TPageRequest>(
            this IQueryable<TSource> query,
            TPageRequest request, DefaultPrimaryKeyDefinition defaultPrimaryKey = null,
            CancellationToken cancellationToken = default)
            where TSource : class
            where TPageRequest : PagedRequest
        {
            if (query.IsNull())
                ThrowHelper.ArgumentNullException("Invalid query");

            if (request.IsNull())
                ThrowHelper.ArgumentNullException("Request can't be null");

            var watch = TimeWatchHelper.Instance();
            watch.StartNew();

            var queryType = typeof(TSource);
            query = query.FilterSourceByText(request.Search.SearchInAllTextFields, request.Search.Search, request.Search.CustomSearchTextProperties, queryType);

            if (request.Order.IsNotNull())
            {
                if (request.Order.OrderByProperty.IsNullOrEmpty().IsFalse())
                {
                    query = query.OrderByWithDirection(request.Order.OrderByProperty, request.Order.OrderDirection, null, queryType, request.Order.OrderByDefaultProperty);
                }
                else if (request.Order.OrderByDefaultProperty.IsTrue())
                {
                    var defaultOrderProperty = ReflectionTypeStorage.GetDefaultOrderByPropertyInfo(queryType);
                    if (defaultOrderProperty.IsSuccess.IsFalse())
                        ThrowHelper.Exception(defaultOrderProperty.GetFirstMessage());

                    if (defaultOrderProperty.Response != null)
                        query = query.OrderByWithDirectionByDefaultProperty(request.Order.OrderDirection, null, queryType);
                }
                else
                {
                    query = query.OrderBy(x => 0);
                }
            }

            var result = new PagedResult<TSource>()
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                RowCount = await query.CountAsync(cancellationToken)
            };

            var pageCount = (double)result.RowCount / request.PageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (request.Page - 1) * request.PageSize;

            /*
             * If skip record == 0, then execute commands that allow and set
             * specified record ids in the top of list
             * otherwise execute standard command for get paged result data
             */
            if (skip.IsLessOrEqualZero())
            {
                var defaultKeys = query.GetDefaultPrimaryKeyProp(defaultPrimaryKey);

                var hasIds = request.PredefinedRecords.IsNotNull()
                             && request.PredefinedRecords.Any()
                             && request.PredefinedRecords.All(x => !string.IsNullOrEmpty(x));
                var idCount = hasIds.Equals(true) ? request.PredefinedRecords?.Count ?? 0 : 0;
                var res = new List<TSource>();
                if (hasIds)
                {
                    var topXRecords = await EnumerableInvoker.Invoke(query.GetInTopPredefinedRecords(request.PredefinedRecords, defaultKeys))
                        .ToListAsync(cancellationToken);
                    res.AddRange(topXRecords);

                    var mainRecords = await query
                        .GetNotInTopPredefinedRecords(request.PredefinedRecords, defaultKeys)
                        .Skip(skip)
                        .Take(request.PageSize - idCount)
                        .ToListAsync(cancellationToken);

                    res.AddRange(mainRecords);

                    result.Response = res;
                }
                else
                {
                    result.Response = await query
                        .Skip(skip)
                        .Take(request.PageSize)
                        .ToListAsync(cancellationToken);
                }
            }
            else
            {
                result.Response = await query
                    .Skip(skip)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);
            }

            if (request.Fields.IsNullOrEmptyEnumerable().IsFalse())
            {
                var fieldsCorrectNames = request.Fields.Select(field => field.FirstCharToUpper());
                var parsedRecords = result.Response.ParseEnumerableOfTInDynamic(fieldsCorrectNames);

                result.Response = JsonSerializer.Deserialize<List<TSource>>(JsonSerializer.Serialize(parsedRecords));
            }

            result.ExecutionDetails.SetExecutionTimeMs(watch.Stop(), DateTime.Now);
            result.IsSuccess = true;

            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;TSource&gt; extension method that gets paged with main filters
        ///     asynchronous.
        /// </summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TPageRequest">Type of the page request.</typeparam>
        /// <param name="query">The query to act on.</param>
        /// <param name="request">The request.</param>
        /// <param name="defaultPrimaryKey">(Optional) The default primary key.</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// <returns>The paged with main filters async&lt; t source, t page request&gt;</returns>
        ///=================================================================================================
        public static async Task<PagedResult<TSource>> GetPagedWithMainFiltersAsync<TSource, TPageRequest>(
            this IQueryable<TSource> query, TPageRequest request,
            DefaultPrimaryKeyDefinition defaultPrimaryKey = null, CancellationToken cancellationToken = default)
            where TSource : class
            where TPageRequest : PageRequestWithFilters
        {
            if (query.IsNull())
                ThrowHelper.ArgumentNullException("Invalid query");

            if (request.IsNull())
                ThrowHelper.ArgumentNullException("Request can't be null");

            InitValidationRequestFilter(request);

            var watch = TimeWatchHelper.Instance();
            watch.StartNew();

            var resultData = await query
                    .AsSimpleFilterable(request.Filters)
                    .GetPagedAsync(request, defaultPrimaryKey, cancellationToken);

            resultData.ExecutionDetails.SetExecutionTimeMs(watch.Stop(), DateTime.Now);

            return resultData;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;TSource&gt; extension method that gets paged with filters asynchronous.
        /// </summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TPageRequest">Type of the page request.</typeparam>
        /// <param name="query">The query to act on.</param>
        /// <param name="request">The request.</param>
        /// <param name="defaultPrimaryKey">(Optional) The default primary key.</param>
        /// <param name="filterLink">(Optional) The main filters link type. Default value: And</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// <returns>The paged with filters async&lt; t source, t page request&gt;</returns>
        ///=================================================================================================
        public static async Task<PagedResult<TSource>> GetPagedWithFiltersAsync<TSource, TPageRequest>(
            this IQueryable<TSource> query, TPageRequest request,
            DefaultPrimaryKeyDefinition defaultPrimaryKey = null,
            FilterConditionType filterLink = FilterConditionType.And,
            CancellationToken cancellationToken = default)
            where TSource : class
            where TPageRequest : PageRequestWithFilters
        {
            if (query.IsNull())
                ThrowHelper.ArgumentNullException("Invalid query");

            if (request.IsNull())
                ThrowHelper.ArgumentNullException("Request can't be null");

            InitValidationRequestFilter(request);

            var watch = TimeWatchHelper.Instance();
            watch.StartNew();

            var resultData = await query
                .AsFilterable(request.Filters, filterLink)
                .GetPagedAsync(request, defaultPrimaryKey, cancellationToken);

            resultData.ExecutionDetails.SetExecutionTimeMs(watch.Stop(), DateTime.Now);

            return resultData;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;TSource&gt; extension method that gets default primary key property.
        /// </summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="query">The query to act on.</param>
        /// <param name="defaultPrimaryKey">(Optional) The default primary key.</param>
        /// <returns>The default primary key property.</returns>
        ///=================================================================================================
        private static ICollection<string> GetDefaultPrimaryKeyProp<TSource>(
            this IQueryable<TSource> query,
            DefaultPrimaryKeyDefinition defaultPrimaryKey = null) where TSource : class
        {
            if (defaultPrimaryKey.IsNull())
                return null;

            if (!defaultPrimaryKey!.DefaultPrimaryKey.IsNullOrEmpty())
                return new[] { defaultPrimaryKey.DefaultPrimaryKey };

            if (defaultPrimaryKey.FindByEntity.IsTrue())
                return query.GetPrimaryKeysNameList();

            if (defaultPrimaryKey.FindByAttribute.IsTrue())
                return null;

            return null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Initializes the validation request filter.</summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// <typeparam name="TPageRequest">Type of the page request.</typeparam>
        /// <param name="pageRequest">The page request.</param>
        ///=================================================================================================
        private static void InitValidationRequestFilter<TPageRequest>(TPageRequest pageRequest)
        where TPageRequest : PageRequestWithFilters
        {
            var requestValidation = pageRequest
                .Validate(new ValidationContext(pageRequest, null, null)).ToList();
            if (requestValidation.Any())
            {
                ThrowHelper.Exception(requestValidation.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}