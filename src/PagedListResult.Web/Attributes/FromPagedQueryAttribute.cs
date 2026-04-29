// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 20:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 21:14
// ***********************************************************************
//  <copyright file="FromPagedQueryAttribute.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Attributes
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Marks a controller / minimal-API parameter (a <c>PagedRequest</c> or
    ///     <c>PageRequestWithFilters</c>) as bound from the request query string by the
    ///     <c>PagedRequestQueryParser</c>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         When <see cref="EntityType" /> is provided and a matching <c>PageableMetadata</c> is
    ///         registered, sort/filter/search property names are validated against the per-entity
    ///         allow-list <em>before</em> any reflection or expression-builder work occurs.
    ///     </para>
    ///     <para>
    ///         Without this attribute, <c>PagedRequest</c> parameters fall through to the default
    ///         complex-object binder (i.e. body-bound usages remain backwards compatible).
    ///     </para>
    ///     <para>
    ///         <example>
    ///             Plain query binding (no allow-list):
    ///             <code>
    ///             [HttpGet]
    ///             public IActionResult List([FromPagedQuery] PagedRequest request)
    ///                 =&gt; PagedOkResult(_service.GetPaged(request));
    ///             // Call: GET /products?page=2&amp;pageSize=20&amp;order=name:asc&amp;search=usb
    ///             </code>
    ///         </example>
    ///         <example>
    ///             Allow-listed binding with filters:
    ///             <code>
    ///             // Startup:
    ///             services.AddPagedListResultWeb()
    ///                 .ConfigurePageable&lt;Product&gt;(b =&gt; b
    ///                     .AllowSort("name", "price")
    ///                     .AllowFilter("status", "price"));
    ///     
    ///             // Action:
    ///             [HttpGet]
    ///             public IActionResult List(
    ///                 [FromPagedQuery(typeof(Product))] PageRequestWithFilters request)
    ///                 =&gt; PagedOkResult(_service.GetPaged(request));
    ///             // GET /products?filter=status:Equals:active&amp;filter=price:GreaterThan:100&amp;order=price:desc
    ///             // Disallowed property =&gt; 400 ValidationProblem ('secret' not in allow-list).
    ///             </code>
    ///         </example>
    ///     </para>
    /// </remarks>
    /// =================================================================================================
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FromPagedQueryAttribute : Attribute, IBindingSourceMetadata
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Default constructor; no entity type allow-list applied.
        /// </summary>
        /// =================================================================================================
        public FromPagedQueryAttribute()
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Constructor that opts into per-entity allow-list validation.
        /// </summary>
        /// <param name="entityType">The entity (item) type the request paginates over.</param>
        /// =================================================================================================
        public FromPagedQueryAttribute(Type entityType) => EntityType = entityType;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The entity type used for allow-list look-up; may be <c>null</c>.
        /// </summary>
        /// <value>
        ///     The type of the entity.
        /// </value>
        /// =================================================================================================
        public Type EntityType { get; }

        /// <inheritdoc />
        public BindingSource BindingSource => BindingSource.Query;
    }
}