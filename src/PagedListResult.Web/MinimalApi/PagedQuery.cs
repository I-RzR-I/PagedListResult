#if NET7_0_OR_GREATER

// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 20:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 22:06
// ***********************************************************************
//  <copyright file="PagedQuery.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Http;
using RzR.ResultMessage.Pagination.DataModels.Models.Request.Page;
using RzR.ResultMessage.Pagination.Web.Abstractions;
using RzR.ResultMessage.Pagination.Web.Helpers;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

#endregion

namespace RzR.ResultMessage.Pagination.Web.MinimalApi
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Minimal-API bindable wrapper around <see cref="PagedRequest" />. Implements the
    ///     <c>BindAsync(HttpContext, ParameterInfo)</c> pattern recognized by ASP.NET Core's
    ///     parameter binding so endpoint handlers can accept it directly from the query string.
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The entity type used for allow-list look-up via <see cref="IPageableMetadataRegistry" />.
    /// </typeparam>
    /// <remarks>
    ///     <example>
    ///         A complete minimal-API endpoint:
    ///         <code>
    ///        // 1. Register and (optionally) configure an allow-list per entity:
    ///        builder.Services
    ///            .AddPagedListResultWeb(o => { o.MaxPageSize = 100; })
    ///            .ConfigurePageable&lt;Product&gt;(b => b
    ///                .AllowSort("name", "price")
    ///                .AllowFilter("status", "price")
    ///                .AllowSearch("name", "description"));
    /// 
    ///        // 2. Accept PagedQuery&lt;TEntity&gt; in the handler:
    ///        app.MapGet("/products", (PagedQuery&lt;Product&gt; q, IProductService svc, HttpContext ctx) =&gt;
    ///        {
    ///            if (!q.IsValid)
    ///                return Results.ValidationProblem(
    ///                    q.Errors.ToDictionary(e =&gt; e.Key, e =&gt; new[] { e.Value }));
    /// 
    ///            return svc.GetPaged(q.Page, q.PageSize).ToPagedHttpResult(ctx);
    ///        })
    ///        .WithPagedResult();   // emit X-Total-Count, Link, Server-Timing
    /// 
    ///        // 3. Call: GET /products?page=2&amp;pageSize=20&amp;order=price:desc&amp;search=usb
    ///        </code>
    ///     </example>
    /// </remarks>
    /// =================================================================================================
    public class PagedQuery<TEntity> : PagedRequest
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
        public static ValueTask<PagedQuery<TEntity>> BindAsync(
            HttpContext httpContext, ParameterInfo parameter)
        {
            var instance = new PagedQuery<TEntity>();
            instance.Errors = MinimalApiQueryBinder.Bind<TEntity>(instance, httpContext);

            return new ValueTask<PagedQuery<TEntity>>(instance);
        }
    }
}

#endif