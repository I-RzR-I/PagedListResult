#if NET7_0_OR_GREATER

// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 18:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 21:23
// ***********************************************************************
//  <copyright file="PagedResultsHttpExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Http;
using RzR.Extensions.Domain.Primitives;
using RzR.ResultMessage.Pagination.DataModels.Abstractions;
using RzR.ResultMessage.Web.Extensions.MinimalApi;
using System.Net;

using LibResult = RzR.ResultMessage.Abstractions.IResult;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Minimal-API adapters for <see cref="IPagedResult{TSource}" />.
    ///     <para>
    ///         The upstream <c>ResultToHttpResult.ToHttpResult&lt;T&gt;</c> serializes only
    ///         <c>IResult&lt;T&gt;.Response</c> on the success path, which would discard the paging
    ///         metadata (<c>CurrentPage</c>, <c>PageCount</c>, <c>RowCount</c>, <c>ExecutionDetails</c>)
    ///         when applied to an <see cref="IPagedResult{T}" /> (because it is
    ///         <c>IResult&lt;IList&lt;T&gt;&gt;</c>). This helper preserves the full paged envelope on
    ///         success and delegates failure to the upstream ProblemDetails machinery so the body
    ///         shape stays identical to the MVC pipeline.
    ///     </para>
    /// </summary>
    /// =================================================================================================
    public static class PagedResultsHttpExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Converts an <see cref="IPagedResult{TSource}" /> into a Minimal-API
        ///     <see cref="Microsoft.AspNetCore.Http.IResult" />. On success returns
        ///     <c>200 OK</c> with the entire paged envelope as the body so the
        ///     <c>PagedResultEndpointFilter</c> can detect it and emit pagination headers.
        ///     On failure delegates to the upstream
        ///     <see cref="ResultToHttpResult.ToHttpResult(LibResult, HttpStatusCode?, string, string, string, System.Collections.Generic.IDictionary{string, object}, HttpContext)" />
        ///     which renders RFC 9457 <c>application/problem+json</c> via the configured
        ///     <c>IProblemDetailsResultFactory</c>.
        /// </summary>
        /// <typeparam name="TSource">The paged item type.</typeparam>
        /// <param name="paged">The paged result.</param>
        /// <param name="httpContext">
        ///     (Optional) Ambient <see cref="HttpContext" /> used by the failure path for correlation
        ///     (e.g. autopopulating <c>traceId</c>).
        /// </param>
        /// <returns>A Minimal-API result.</returns>
        /// =================================================================================================
        public static IResult ToPagedHttpResult<TSource>(
            this IPagedResult<TSource> paged,
            HttpContext httpContext = null)
            where TSource : class
        {
            if (paged.IsNull())
                return Results.StatusCode((int)HttpStatusCode.NoContent);

            if (paged.IsSuccess)
                return Results.Ok(paged);

            // Failure: reuse upstream ProblemDetails pipeline. Cast through the non-generic
            // LibResult so BuildHttpResult is invoked with hasResponseBody=false (matches MVC).
            return ((LibResult)paged).ToHttpResult(httpContext: httpContext);
        }
    }
}

#endif