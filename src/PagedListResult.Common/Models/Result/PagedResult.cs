// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-24 14:56
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-10 01:13
// ***********************************************************************
//  <copyright file="PagedResult.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage;
using PagedListResult.Common.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
#pragma warning disable CS1584

#endregion

namespace PagedListResult.Common.Models.Result
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Encapsulates the result of a paged.</summary>
    /// <remarks>RzR, 10-Nov-23.</remarks>
    /// <typeparam name="TSource">Type of the source.</typeparam>
    /// <seealso cref="AggregatedGenericResultMessage.Result{IList{TSource}}" />
    /// <seealso cref="PagedListResult.Common.Abstractions.IPagedResult{TSource}" />
    /// ###
    /// <inheritdoc cref="IPagedResult{TSource}" />
    /// =================================================================================================
    public class PagedResult<TSource> : Result<IList<TSource>>, IPagedResult<TSource>
        where TSource : class
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Current page.</summary>
        /// <value>The current page.</value>
        /// =================================================================================================
        public virtual int CurrentPage { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Total page number.</summary>
        /// <value>The number of pages.</value>
        /// =================================================================================================
        public virtual int PageCount { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Items per page.</summary>
        /// <value>The size of the page.</value>
        /// =================================================================================================
        public virtual int PageSize { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Total filtered items.</summary>
        /// <value>The number of rows.</value>
        /// =================================================================================================
        public virtual int RowCount { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets information describing the execution of current paged result.</summary>
        /// <value>Information describing the execution of current paged result.</value>
        /// =================================================================================================
        public PagedExecDetailsResult ExecutionDetails { get; set; } = new PagedExecDetailsResult();

        /// <inheritdoc />
        [XmlArray]
        public override IList<TSource> Response { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Map paged result.</summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <typeparam name="TDestination">Destination type result/response.</typeparam>
        /// <param name="data">.</param>
        /// <returns>A PagedResult&lt;TDestination&gt;</returns>
        /// =================================================================================================
        public virtual PagedResult<TDestination> Map<TDestination>(IEnumerable<TDestination> data)
            where TDestination : class
            => new PagedResult<TDestination>()
            {
                Response = data.ToList(),
                CurrentPage = CurrentPage,
                PageCount = PageCount,
                PageSize = PageSize,
                RowCount = RowCount,
                IsSuccess = IsSuccess,
                Messages = Messages,
                ExecutionDetails = ExecutionDetails
            };
    }
}