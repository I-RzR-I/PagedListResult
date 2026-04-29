// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-26 21:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 09:56
// ***********************************************************************
//  <copyright file="PagedListResultServiceCollectionExtensions.cs" company="RzR SOFT & TECH">
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
using RzR.ResultMessage.Pagination.AspNetCore.Abstractions;
using RzR.ResultMessage.Pagination.AspNetCore.Builders;
using RzR.ResultMessage.Pagination.AspNetCore.Filters;
using RzR.ResultMessage.Pagination.AspNetCore.ModelBinding;
using RzR.ResultMessage.Pagination.AspNetCore.Models;
using RzR.ResultMessage.Pagination.AspNetCore.Registries;
using System;
using System.Linq;

#endregion

namespace RzR.ResultMessage.Pagination.AspNetCore.Configuration
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     <see cref="IServiceCollection" /> extensions for the <c>PagedListResult.Web</c> package.
    /// </summary>
    /// =================================================================================================
    public static class PagedListResultServiceCollectionExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Registers the <c>PagedListResult.Web</c> services: <see cref="PagedListResultWebOptions" />
        ///     , the singleton <see cref="IPageableMetadataRegistry" />, and the
        ///     <see cref="PagedResultApplicationModelConvention" /> via <see cref="MvcOptions" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">
        ///     (Optional) Optional <see cref="PagedListResultWebOptions" /> configuration.
        /// </param>
        /// <returns>
        ///     A <see cref="PagedListResultBuilder" /> for chaining (e.g.
        ///     <c>
        ///         .ConfigurePageable&lt;T&gt;
        ///         (...)
        ///     </c>
        ///     ).
        /// </returns>
        /// =================================================================================================
        public static PagedListResultBuilder AddPagedListResultWeb(
            this IServiceCollection services,
            Action<PagedListResultWebOptions> configure = null)
        {
            if (services.IsNull())
                throw new ArgumentNullException(nameof(services));

            services.AddOptions();

            if (configure.IsNotNull())
                services.Configure(configure!);

            // Use (or create) a single registry instance so the builder writes through to the
            // exact same store that consumers will later resolve from DI.
            var existing = services.FirstOrDefault(d => d.ServiceType == typeof(IPageableMetadataRegistry));
            var registry = existing?.ImplementationInstance as IPageableMetadataRegistry;
            if (registry.IsNull())
            {
                registry = new PageableMetadataRegistry();

                if (existing.IsNotNull())
                    services.Remove(existing);

                services.AddSingleton(registry);
            }

            services.Configure<MvcOptions>(o =>
            {
                if (!o.Conventions.OfType<PagedResultApplicationModelConvention>().Any())
                    o.Conventions.Add(new PagedResultApplicationModelConvention());

                if (!o.Filters.OfType<ServiceFilterAttribute>().Any(f => f.ServiceType == typeof(PagedResultActionFilter)))
                    o.Filters.Add(new ServiceFilterAttribute(typeof(PagedResultActionFilter)));

                if (!o.ModelBinderProviders.OfType<PagedRequestModelBinderProvider>().Any())
                    o.ModelBinderProviders.Insert(0, new PagedRequestModelBinderProvider());
            });

            services.AddSingleton<PagedResultActionFilter>();

#if NET7_0_OR_GREATER
            services.AddSingleton<PagedResultEndpointFilter>();
#endif

            return new PagedListResultBuilder(services, registry);
        }
    }
}