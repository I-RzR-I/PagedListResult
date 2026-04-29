#if NET7_0_OR_GREATER

// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-26 08:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 21:24
// ***********************************************************************
//  <copyright file="PagedResultEndpointFilter.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RzR.Extensions.Domain.Primitives;
using RzR.ResultMessage.Pagination.DataModels.Abstractions;
using RzR.ResultMessage.Pagination.Web.Helpers;
using RzR.ResultMessage.Pagination.Web.Models;
using System;
using System.Threading.Tasks;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Filters
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Minimal-API counterpart of <see cref="PagedResultActionFilter"/>. When the awaited result of
    ///     the endpoint is — directly or wrapped in a typed result such as <c>Ok&lt;T&gt;</c> /
    ///     <c>JsonHttpResult&lt;T&gt;</c> — an <see cref="IPagedResult{T}"/>, emits the standard
    ///     pagination headers (<c>X-Total-Count</c>, <c>X-Page-Count</c>, <c>X-Page-Size</c>,
    ///     <c>X-Current-Page</c>), RFC 5988 <c>Link</c> (first/prev/next/last), and optionally
    ///     <c>Server-Timing</c>. All header emission is gated by
    ///     <see cref="PagedListResultWebOptions"/>.
    ///     <para>
    ///         Available only on net7.0+. Register per-route or per-group via
    ///         <c>RouteHandlerBuilder.AddPagedResultEndpointFilter()</c>.
    ///     </para>
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Http.IEndpointFilter"/>
    /// =================================================================================================
    public sealed class PagedResultEndpointFilter : IEndpointFilter
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) options for controlling the operation.
        /// </summary>
        /// =================================================================================================
        private readonly IOptions<PagedListResultWebOptions> _options;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of <see cref="PagedResultEndpointFilter"/>.
        /// </summary>
        /// <param name="options">Options for controlling the operation.</param>
        /// =================================================================================================
        public PagedResultEndpointFilter(IOptions<PagedListResultWebOptions> options)
            => _options = options ?? throw new ArgumentNullException(nameof(options));

        /// <inheritdoc/>
        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (context.IsNull())
                throw new ArgumentNullException(nameof(context));
            if (next.IsNull())
                throw new ArgumentNullException(nameof(next));

            var result = await next(context).ConfigureAwait(false);

            var payload = PagedResponseHeaderWriter.UnwrapResultValue(result);
            PagedResponseHeaderWriter.TryApply(context.HttpContext, payload, _options.Value);

            return result;
        }
    }
}

#endif