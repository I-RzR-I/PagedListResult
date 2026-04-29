#if NET7_0_OR_GREATER

// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 22:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 22:11
// ***********************************************************************
//  <copyright file="PagedQueryWithFilters.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Http;
using RzR.ResultMessage.Pagination.Abstractions.Models.Request.Page;
using RzR.ResultMessage.Pagination.AspNetCore.Helpers;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

#endregion

namespace RzR.ResultMessage.Pagination.AspNetCore.MinimalApi
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Minimal-API bindable wrapper around <see cref="PageRequestWithFilters" />. Adds
    ///     support for the repeatable <c>?filter=</c> query parameter on top of
    ///     <see cref="PagedQuery{TEntity}" />. Multiple filters are AND-combined by repeating
    ///     the key, e.g. <c>?filter=status:Equals:active&amp;filter=price:GreaterThan:50</c>.
    /// </summary>
    /// <remarks>
    ///     <example>
    ///         <code>
    ///         app.MapGet("/products", (PagedQueryWithFilters&lt;Product&gt; q, IProductService svc) =&gt;
    ///             q.IsValid
    ///                 ? Results.Ok(svc.GetPaged(q))
    ///                 : Results.ValidationProblem(
    ///                     q.Errors.ToDictionary(e =&gt; e.Key, e =&gt; new[] { e.Value })))
    ///         .WithPagedResult();
    /// 
    ///         // Call: GET /products?filter=status:Equals:active&amp;filter=price:GreaterThan:50&amp;order=price:desc
    ///         </code>
    ///     </example>
    /// </remarks>
    /// =================================================================================================
    public sealed class PagedQueryWithFilters<TEntity> : PageRequestWithFilters
        where TEntity : class
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Errors recorded by the parser when the query string was malformed.
        /// </summary>
        /// <value>
        ///     The errors.
        /// </value>
        /// =================================================================================================
        public IDictionary<string, string> Errors { get; internal set; } =
            new Dictionary<string, string>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     True when binding succeeded with no errors.
        /// </summary>
        /// <value>
        ///     True if this object is valid, false if not.
        /// </value>
        /// =================================================================================================
        public bool IsValid => Errors.Count == 0;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Minimal-API binding entry point.
        /// </summary>
        /// <param name="httpContext">Context for the HTTP.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        ///     The bind.
        /// </returns>
        /// =================================================================================================
        public static ValueTask<PagedQueryWithFilters<TEntity>> BindAsync(
            HttpContext httpContext, ParameterInfo parameter)
        {
            var instance = new PagedQueryWithFilters<TEntity>();
            instance.Errors = MinimalApiQueryBinder.Bind<TEntity>(instance, httpContext);

            return new ValueTask<PagedQueryWithFilters<TEntity>>(instance);
        }
    }
}

#endif