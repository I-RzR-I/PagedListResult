// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 14:13
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="IPagedResult.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage.Abstractions;
using PagedListResult.DataModels.Models.Result;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace PagedListResult.DataModels.Abstractions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Paged result.
    /// </summary>
    /// <remarks>
    ///     RzR, 14-Nov-23.
    /// </remarks>
    /// <typeparam name="TSource">Type of paged result.</typeparam>
    /// =================================================================================================
    [XmlInclude(typeof(PagedResult<>))]
    public interface IPagedResult<TSource> : IResult<IList<TSource>> where TSource : class
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Current page.
        /// </summary>
        /// <value>
        ///     The current page.
        /// </value>
        /// =================================================================================================
        int CurrentPage { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Items on current page.
        /// </summary>
        /// <value>
        ///     The number of pages.
        /// </value>
        /// =================================================================================================
        int PageCount { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Items per page.
        /// </summary>
        /// <value>
        ///     The size of the page.
        /// </value>
        /// =================================================================================================
        int PageSize { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Total filtered items.
        /// </summary>
        /// <value>
        ///     The number of rows.
        /// </value>
        /// =================================================================================================
        int RowCount { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object has previous page.
        /// </summary>
        /// <value>
        ///     True if this object has previous page, false if not.
        /// </value>
        /// =================================================================================================
        bool HasPreviousPage { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object has next page.
        /// </summary>
        /// <value>
        ///     True if this object has next page, false if not.
        /// </value>
        /// =================================================================================================
        bool HasNextPage { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the execution details.
        /// </summary>
        /// <value>
        ///     The execution details.
        /// </value>
        /// =================================================================================================
        PagedExecDetailsResult ExecutionDetails { get; set; }
    }
}