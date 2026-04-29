// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-26 21:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 22:50
// ***********************************************************************
//  <copyright file="PagedListResultWebOptions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.ResultMessage.Abstractions;
using System;
using System.Text.Json;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Options for the <c>PagedListResult.Web</c> integration. Configured via
    ///     <c>IServiceCollection.AddPagedListResultWeb()</c>.
    /// </summary>
    /// =================================================================================================
    public sealed class PagedListResultWebOptions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Hard upper bound enforced at the binder / action filter level. Default <c>200</c>.
        /// </summary>
        /// <value>
        ///     The maximum size of the page.
        /// </value>
        /// =================================================================================================
        public int MaxPageSize { get; set; } = 200;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Default page size used when the request does not provide one. Default <c>10</c>.
        /// </summary>
        /// <value>
        ///     The default page size.
        /// </value>
        /// =================================================================================================
        public int DefaultPageSize { get; set; } = 10;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit the RFC 5988 <c>Link</c> header on paged responses. Default <c>true</c>.
        /// </summary>
        /// <remarks>
        ///     The link header will be added only for <c>HTTP GET</c> request.
        /// </remarks>
        /// <value>
        ///     True if emit link header, false if not.
        /// </value>
        /// =================================================================================================
        public bool EmitLinkHeader { get; set; } = true;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit <c>X-Total-Count</c> / <c>X-Page-Count</c> / <c>X-Page-Size</c> / <c>X-Current-Page</c>
        ///     . Default <c>true</c>.
        /// </summary>
        /// <value>
        ///     True if emit total count header, false if not.
        /// </value>
        /// =================================================================================================
        public bool EmitTotalCountHeader { get; set; } = true;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit <c>Server-Timing: paged;dur={ExecutionTimeMs}</c>. Default <c>false</c>.
        /// </summary>
        /// <value>
        ///     True if emit server timing header, false if not.
        /// </value>
        /// =================================================================================================
        public bool EmitServerTimingHeader { get; set; } = false;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Optional pluggable status-code mapper. Receives the failed <see cref="IResult" /> and the
        ///     fallback status code (typically <c>400</c>) and returns the resolved HTTP status code.
        ///     Default <c>null</c> (always use fallback).
        /// </summary>
        /// <value>
        ///     A function delegate that yields an int.
        /// </value>
        /// =================================================================================================
        public Func<IResult, int, int> StatusCodeMapper { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Base URI used to build RFC 9457 <c>type</c> values. Default <c>null</c> (factory default).
        /// </summary>
        /// <value>
        ///     The problem type base URI.
        /// </value>
        /// =================================================================================================
        public Uri ProblemTypeBaseUri { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Optional <see cref="JsonSerializerOptions" /> override for paged payloads. Default <c>
        ///     null</c>.
        /// </summary>
        /// <value>
        ///     Options that control the JSON serializer.
        /// </value>
        /// =================================================================================================
        public JsonSerializerOptions JsonSerializerOptions { get; set; }
    }
}