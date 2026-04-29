// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 21:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 21:45
// ***********************************************************************
//  <copyright file="PagedParseResult.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.ResultMessage.Pagination.AspNetCore.Helpers;
using System;
using System.Collections.Generic;

#endregion

namespace RzR.ResultMessage.Pagination.AspNetCore.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Aggregated outcome of <see cref="PagedRequestQueryParser.Populate(RzR.ResultMessage.Pagination.Abstractions.Models.Request.Page.PagedRequest,Microsoft.AspNetCore.Http.IQueryCollection,RzR.ResultMessage.Pagination.AspNetCore.Models.PagedListResultWebOptions,RzR.ResultMessage.Pagination.AspNetCore.Models.PageableMetadata)" />
    ///     .
    /// </summary>
    /// =================================================================================================
    public sealed class PagedParseResult
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Errors keyed by query parameter name (suitable for ProblemDetails).
        /// </summary>
        /// <value>
        ///     The errors.
        /// </value>
        /// =================================================================================================
        public IDictionary<string, string> Errors { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Non-fatal warnings (e.g. ignored extra order keys).
        /// </summary>
        /// <value>
        ///     The warnings.
        /// </value>
        /// =================================================================================================
        public IList<string> Warnings { get; } = new List<string>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     True when no errors were recorded.
        /// </summary>
        /// <value>
        ///     True if this object is valid, false if not.
        /// </value>
        /// =================================================================================================
        public bool IsValid => Errors.Count == 0;
    }
}