// ***********************************************************************
//  Assembly          : RzR.Shared.Entity.PagedListResult.Web
//  Author            : RzR
//  Created           : 02-07-2026 22:07
// 
//  Last Modified By : RzR
//  Last Modified On : 02-07-2026 23:11
//  ***********************************************************************
//  <copyright file="IPagedQueryValidation.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System.Collections.Generic;

#endregion

namespace RzR.ResultMessage.Pagination.AspNetCore.Abstractions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Exposes a mutable parser-error dictionary on a request type. Implemented by the Minimal
    ///     API wrapper types (<c>PagedQuery&lt;TEntity&gt;</c>,
    ///     <c>PagedQueryWithFilters&lt;TEntity&gt;</c>) so that <see cref="T:RzR.ResultMessage.Pagination.AspNetCore.ModelBinding.PagedRequestModelBinder" />
    ///     can surface parse errors on the instance's own <c>Errors</c>/<c>IsValid</c> members when
    ///     one of these types is (mis)used as an MVC <c>[FromPagedQuery]</c> action parameter
    ///     instead of a Minimal API handler parameter.
    /// </summary>
    /// =================================================================================================
    public interface IPagedQueryValidation
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Errors recorded by the parser when the query string was malformed. Getter-only; the
        ///     parser (or the MVC model binder) mutates the returned dictionary in place.
        /// </summary>
        /// <value>
        ///     The errors.
        /// </value>
        /// =================================================================================================
        IDictionary<string, string> Errors { get; }
    }
}