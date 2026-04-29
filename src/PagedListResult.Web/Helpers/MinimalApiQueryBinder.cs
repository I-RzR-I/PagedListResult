#if NET7_0_OR_GREATER

// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 22:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 22:11
// ***********************************************************************
//  <copyright file="MinimalApiQueryBinder.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RzR.ResultMessage.Pagination.Abstractions.Models.Request.Page;
using RzR.ResultMessage.Pagination.AspNetCore.Abstractions;
using RzR.ResultMessage.Pagination.AspNetCore.Models;
using System.Collections.Generic;

#endregion

namespace RzR.ResultMessage.Pagination.AspNetCore.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Helper that resolves <see cref="PagedListResultWebOptions" /> and the per-entity
    ///     allow-list from the request's service provider, then dispatches to the correct <see cref="PagedRequestQueryParser" />
    ///     overload.
    /// </summary>
    /// =================================================================================================
    internal static class MinimalApiQueryBinder
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Binds.
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <param name="target">Target for the paged request.</param>
        /// <param name="httpContext">Context for the HTTP.</param>
        /// <returns>
        ///     An IDictionary&lt;string,string&gt;
        /// </returns>
        /// =================================================================================================
        public static IDictionary<string, string> Bind<TEntity>(
            PagedRequest target, HttpContext httpContext)
            where TEntity : class
        {
            var services = httpContext.RequestServices;
            var options = services.GetService<IOptions<PagedListResultWebOptions>>()?.Value
                          ?? new PagedListResultWebOptions();
            var allowList = services.GetService<IPageableMetadataRegistry>()?.Get(typeof(TEntity));

            var parse = target is PageRequestWithFilters pwf
                ? PagedRequestQueryParser.Populate(pwf, httpContext.Request.Query, options, allowList)
                : PagedRequestQueryParser.Populate(target, httpContext.Request.Query, options, allowList);

            return parse.Errors;
        }
    }
}

#endif