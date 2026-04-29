// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 08:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 21:27
// ***********************************************************************
//  <copyright file="PagedResponseHeaderWriter.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Http;
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;
using RzR.ResultMessage.Pagination.DataModels.Abstractions;
using RzR.ResultMessage.Pagination.Web.Filters;
using RzR.ResultMessage.Pagination.Web.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Internal helper shared by the MVC <see cref="PagedResultActionFilter" />
    ///     and the minimal-API <c>PagedResultEndpointFilter</c> (net7+).
    ///     Detects an <see cref="IPagedResult{T}" /> payload and writes the standard pagination headers
    ///     gated by <see cref="PagedListResultWebOptions" />.
    /// </summary>
    /// =================================================================================================
    internal static class PagedResponseHeaderWriter
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the accessor cache.
        /// </summary>
        /// =================================================================================================
        private static readonly ConcurrentDictionary<Type, PagedAccessors> AccessorCache = new();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     If <paramref name="payload" /> is an <see cref="IPagedResult{T}" />, writes the configured
        ///     pagination headers to the response. Returns <c>true</c> if headers were emitted.
        /// </summary>
        /// <param name="httpContext">Context for the HTTP.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="options">Options for controlling the operation.</param>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        /// =================================================================================================
        public static bool TryApply(HttpContext httpContext, object payload, PagedListResultWebOptions options)
        {
            if (httpContext.IsNull() || payload.IsNull() || options.IsNull())
                return false;

            var pagedType = payload!.GetType();
            if (ImplementsIPagedResult(pagedType).IsFalse())
                return false;

            ApplyHeaders(httpContext, payload, pagedType, options);

            return true;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns <c>true</c> when <paramref name="t" /> implements <c>IPagedResult&lt;&gt;</c>.
        /// </summary>
        /// <param name="t">A Type to process.</param>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        /// =================================================================================================
        public static bool ImplementsIPagedResult(Type t)
            => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPagedResult<>));

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Extracts the inner paged value from common minimal-API typed results (e.g.
        ///     <c>Ok&lt;TPaged&gt;</c>, <c>JsonHttpResult&lt;TPaged&gt;</c>) by reading the
        ///     <c>Value</c> property; otherwise returns the input.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>
        ///     An object.
        /// </returns>
        /// =================================================================================================
        public static object UnwrapResultValue(object payload)
        {
            if (payload.IsNull())
                return null;

            var t = payload.GetType();

            // If the payload itself implements IPagedResult<T>, no need to unwrap.
            if (ImplementsIPagedResult(t))
                return payload;

            // Microsoft.AspNetCore.Http typed results expose `Value` of the body type.
            var valueProp = t.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
            if (valueProp.IsNull())
                return payload;

            var value = valueProp!.GetValue(payload);

            return value;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the headers.
        /// </summary>
        /// <param name="httpContext">Context for the HTTP.</param>
        /// <param name="pagedValue">The paged value.</param>
        /// <param name="pagedType">Type of the paged.</param>
        /// <param name="opts">Options for controlling the operation.</param>
        /// =================================================================================================
        private static void ApplyHeaders(
            HttpContext httpContext, object pagedValue, Type pagedType, PagedListResultWebOptions opts)
        {
            var headers = httpContext.Response.Headers;
            var accessors = AccessorCache.GetOrAdd(pagedType, PagedAccessors.For);

            var currentPage = accessors.GetCurrentPage(pagedValue);
            var pageCount = accessors.GetPageCount(pagedValue);
            var pageSize = accessors.GetPageSize(pagedValue);
            var rowCount = accessors.GetRowCount(pagedValue);

            if (opts.EmitTotalCountHeader.IsTrue())
            {
                headers["X-Total-Count"] = rowCount.ToString();
                headers["X-Page-Count"] = pageCount.ToString();
                headers["X-Page-Size"] = pageSize.ToString();
                headers["X-Current-Page"] = currentPage.ToString();
            }

            if (opts.EmitLinkHeader.IsTrue() && pageCount > 0 && httpContext.Request.Method.ToUpper() == "GET")
            {
                var link = BuildLinkHeader(httpContext.Request, currentPage, pageCount, pageSize);
                if (link.IsPresent())
                    headers["Link"] = link;
            }

            if (opts.EmitServerTimingHeader.IsTrue())
            {
                var execMs = accessors.GetExecutionTimeMs?.Invoke(pagedValue);
                if (execMs.HasValue && execMs.Value >= 0)
                    headers["Server-Timing"] = $"paged;dur={execMs.Value}";
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds link header.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageCount">Number of pages.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        /// =================================================================================================
        private static string BuildLinkHeader(HttpRequest request, int currentPage, int pageCount, int pageSize)
        {
            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}";
            var preserved = request.Query
                .Where(kv => !string.Equals(kv.Key, "page", StringComparison.OrdinalIgnoreCase)
                             && !string.Equals(kv.Key, "pageSize", StringComparison.OrdinalIgnoreCase)).ToList();

            var links = new List<string>(4);
            links.Add($"<{Build(1)}>; rel=\"first\"");

            if (currentPage > 1) 
                links.Add($"<{Build(currentPage - 1)}>; rel=\"prev\"");
            if (currentPage < pageCount) 
                links.Add($"<{Build(currentPage + 1)}>; rel=\"next\"");

            links.Add($"<{Build(pageCount)}>; rel=\"last\"");

            return links.ListToString(", ");

            string Build(int page)
            {
                var sb = new StringBuilder();
                sb.Append(baseUrl).Append('?');
                foreach (var kv in preserved)
                {
                    foreach (var v in kv.Value)
                    {
                        sb.Append(Uri.EscapeDataString(kv.Key)).Append('=')
                            .Append(Uri.EscapeDataString(v ?? string.Empty)).Append('&');
                    }
                }

                sb.Append("page=").Append(page).Append("&pageSize=").Append(pageSize);

                return sb.ToString();
            }
        }
    }
}