// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-26 20:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 20:45
// ***********************************************************************
//  <copyright file="PagedListResultMvcBuilderExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RzR.Extensions.Domain.Primitives;
using System;
using System.Linq;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Configuration
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     <see cref="IMvcBuilder" /> extensions for the <c>PagedListResult.Web</c> package.
    /// </summary>
    /// =================================================================================================
    public static class PagedListResultMvcBuilderExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Registers the <see cref="PagedResultApplicationModelConvention" /> so MVC actions
        ///     returning
        ///     <c>IPagedResult&lt;T&gt;</c> / <c>PagedResult&lt;T&gt;</c> (optionally wrapped in
        ///     <c>Task&lt;&gt;</c>, <c>ValueTask&lt;&gt;</c> or <c>ActionResult&lt;&gt;</c>) are
        ///     automatically annotated with <c>[Produces("application/json")]</c>,
        ///     <c>[ProducesResponseType(typeof(PagedResult&lt;T&gt;), 200)]</c> and
        ///     <c>[ProducesResponseType(typeof(ProblemDetails), 400)]</c>.
        ///     <para>
        ///         User-declared <c>[ProducesResponseType]</c> with the same status code are preserved.
        ///     </para>
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="builder">The MVC builder.</param>
        /// <returns>
        ///     The same builder for chaining.
        /// </returns>
        /// =================================================================================================
        public static IMvcBuilder AddPagedListResultApiExplorer(this IMvcBuilder builder)
        {
            if (builder.IsNull())
                throw new ArgumentNullException(nameof(builder));

            builder.Services.Configure<MvcOptions>(o =>
            {
                if (!o.Conventions.OfType<PagedResultApplicationModelConvention>().Any())
                    o.Conventions.Add(new PagedResultApplicationModelConvention());
            });

            return builder;
        }
    }
}