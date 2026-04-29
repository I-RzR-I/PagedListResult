// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-26 21:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 22:47
// ***********************************************************************
//  <copyright file="PageableMetadataRegistry.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.Domain.Primitives;
using RzR.ResultMessage.Pagination.Web.Abstractions;
using RzR.ResultMessage.Pagination.Web.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Registries
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Default thread-safe implementation of <see cref="IPageableMetadataRegistry" />.
    /// </summary>
    /// <seealso cref="T:RzR.ResultMessage.Pagination.Web.Abstractions.IPageableMetadataRegistry"/>
    /// =================================================================================================
    internal sealed class PageableMetadataRegistry : IPageableMetadataRegistry
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the store.
        /// </summary>
        /// =================================================================================================
        private readonly ConcurrentDictionary<Type, PageableMetadata> _store = new();

        /// <inheritdoc/>
        public void Register(Type type, PageableMetadata metadata)
        {
            if (type.IsNull()) 
                throw new ArgumentNullException(nameof(type));
            if (metadata.IsNull()) 
                throw new ArgumentNullException(nameof(metadata));

            _store[type] = metadata;
        }

        /// <inheritdoc/>
        public PageableMetadata Get(Type type)
        {
            if (type.IsNull()) 
                throw new ArgumentNullException(nameof(type));

            return _store.TryGetValue(type, out var meta) ? meta : null;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<PageableMetadata> All() => _store.Values.ToArray();
    }
}