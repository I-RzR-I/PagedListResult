// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-26 21:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 23:10
// ***********************************************************************
//  <copyright file="IPageableMetadataRegistry.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.ResultMessage.Pagination.Web.Models;
using System;
using System.Collections.Generic;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Abstractions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Singleton registry of per-type <see cref="PageableMetadata" /> entries consumed by the
    ///     binder and the request-validation filter.
    /// </summary>
    /// =================================================================================================
    public interface IPageableMetadataRegistry
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds (or replaces) the metadata for <paramref name="type" />.
        /// </summary>
        /// <param name="type">The type to get.</param>
        /// <param name="metadata">The metadata.</param>
        /// =================================================================================================
        void Register(Type type, PageableMetadata metadata);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the metadata registered for <paramref name="type" />, or <c>null</c>.
        /// </summary>
        /// <param name="type">The type to get.</param>
        /// <returns>
        ///     A PageableMetadata.
        /// </returns>
        /// =================================================================================================
        PageableMetadata Get(Type type);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns all registered entries.
        /// </summary>
        /// <returns>
        ///     A list of.
        /// </returns>
        /// =================================================================================================
        IReadOnlyCollection<PageableMetadata> All();
    }
}