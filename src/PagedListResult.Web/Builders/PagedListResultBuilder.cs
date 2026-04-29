// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-26 21:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 23:01
// ***********************************************************************
//  <copyright file="PagedListResultBuilder.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.Extensions.DependencyInjection;
using RzR.Extensions.Domain.Primitives;
using RzR.ResultMessage.Pagination.Web.Abstractions;
using RzR.ResultMessage.Pagination.Web.Configuration;
using System;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Builders
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Fluent builder returned by
    ///     <see cref="PagedListResultServiceCollectionExtensions.AddPagedListResultWeb" />. Exposes
    ///     <see cref="ConfigurePageable{T}" /> for per-type allow-list registration.
    /// </summary>
    /// =================================================================================================
    public sealed class PagedListResultBuilder
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the registry.
        /// </summary>
        /// =================================================================================================
        private readonly IPageableMetadataRegistry _registry;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="PagedListResultBuilder"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="services">The services.</param>
        /// <param name="registry">The registry.</param>
        /// =================================================================================================
        internal PagedListResultBuilder(IServiceCollection services, IPageableMetadataRegistry registry)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The underlying service collection, exposed for further chaining.
        /// </summary>
        /// <value>
        ///     The services.
        /// </value>
        /// =================================================================================================
        public IServiceCollection Services { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Registers a per-type allow-list for <typeparamref name="T" />. Replaces any previous
        ///     entry for the same type.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="configure">The configure pageable metadata option.</param>
        /// <returns>
        ///     A list of.
        /// </returns>
        /// =================================================================================================
        public PagedListResultBuilder ConfigurePageable<T>(Action<PageableMetadataBuilder<T>> configure)
            where T : class
        {
            if (configure.IsNull()) 
                throw new ArgumentNullException(nameof(configure));

            var builder = new PageableMetadataBuilder<T>();
            configure(builder);
            _registry.Register(typeof(T), builder.Build());

            return this;
        }
    }
}