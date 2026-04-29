#if NET7_0_OR_GREATER

// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 08:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 09:41
// ***********************************************************************
//  <copyright file="PagedResultEndpointRouteExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RzR.Extensions.Domain.Primitives;
using RzR.ResultMessage.Pagination.Web.Filters;
using RzR.ResultMessage.Pagination.Web.Models;
using System;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Configuration
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Endpoint route-builder extensions that opt a minimal-API route (or a route group) into the
    ///     <see cref="PagedResultEndpointFilter" />.
    /// </summary>
    /// =================================================================================================
    public static class PagedResultEndpointRouteExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attaches <see cref="PagedResultEndpointFilter" /> to the supplied route handler so that
        ///     <see cref="PagedListResultWebOptions" />-gated pagination headers are emitted when the
        ///     endpoint returns an <c>IPagedResult&lt;T&gt;</c>.
        /// </summary>
        /// <typeparam name="TBuilder">A route handler builder type.</typeparam>
        /// <param name="builder">The endpoint convention builder (e.g. <c>RouteHandlerBuilder</c>).</param>
        /// <returns>The same <paramref name="builder" /> for chaining.</returns>
        /// =================================================================================================
        public static TBuilder AddPagedResultEndpointFilter<TBuilder>(this TBuilder builder)
            where TBuilder : IEndpointConventionBuilder
        {
            if (builder.IsNull())
                throw new ArgumentNullException(nameof(builder));

            return builder.AddEndpointFilter<TBuilder, PagedResultEndpointFilter>();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Friendlier alias for <see cref="AddPagedResultEndpointFilter{TBuilder}(TBuilder)"/>.
        ///     Marks the route (or group) as paged so that the
        ///     <see cref="PagedResultEndpointFilter"/> emits standard pagination headers when the
        ///     endpoint returns an <c>IPagedResult&lt;T&gt;</c> (directly or wrapped in
        ///     <c>Ok&lt;T&gt;</c> / <c>JsonHttpResult&lt;T&gt;</c>).
        /// </summary>
        /// <typeparam name="TBuilder">A route handler builder type.</typeparam>
        /// <param name="builder">The endpoint convention builder.</param>
        /// <returns>The same <paramref name="builder" /> for chaining.</returns>
        /// =================================================================================================
        public static TBuilder WithPagedResult<TBuilder>(this TBuilder builder)
            where TBuilder : IEndpointConventionBuilder
            => AddPagedResultEndpointFilter(builder);
    }
}

#endif