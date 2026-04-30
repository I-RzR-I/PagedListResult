// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-26 23:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 21:31
// ***********************************************************************
//  <copyright file="PagedResultActionFilter.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using RzR.Extensions.Domain.Primitives;
using RzR.ResultMessage.Pagination.Abstractions.Abstractions;
using RzR.ResultMessage.Pagination.AspNetCore.Helpers;
using RzR.ResultMessage.Pagination.AspNetCore.Models;
using System;
using System.Threading.Tasks;

#endregion

namespace RzR.ResultMessage.Pagination.AspNetCore.Filters
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     <see cref="IAsyncResultFilter" /> that, when the action result body is an
    ///     <see cref="IPagedResult{TSource}" />, emits standard pagination headers
    ///     (<c>X-Total-Count</c>, <c>X-Page-Count</c>, <c>X-Page-Size</c>, <c>X-Current-Page</c>),
    ///     RFC 5988 <c>Link</c> (first/prev/next/last), and optionally <c>Server-Timing</c>.
    ///     <para>All header emission is gated by <see cref="PagedListResultWebOptions" />.</para>
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Filters.IAsyncResultFilter" />
    /// =================================================================================================
    public sealed class PagedResultActionFilter : IAsyncResultFilter
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) options for controlling the operation.
        /// </summary>
        /// =================================================================================================
        private readonly IOptions<PagedListResultWebOptions> _options;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of <see cref="PagedResultActionFilter" />.
        /// </summary>
        /// <param name="options">Options for controlling the operation.</param>
        /// =================================================================================================
        public PagedResultActionFilter(IOptions<PagedListResultWebOptions> options)
            => _options = options ?? throw new ArgumentNullException(nameof(options));

        /// <inheritdoc />
        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.IsNull())
                throw new ArgumentNullException(nameof(context));
            if (next.IsNull())
                throw new ArgumentNullException(nameof(next));

            var payload = context.Result switch
            {
                ObjectResult or => or.Value,
                JsonResult jr => jr.Value,
                _ => null
            };

            PagedResponseHeaderWriter.TryApply(context.HttpContext, payload, _options.Value);

            return next();
        }
    }
}