// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 13:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="PagedResult.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage;
using PagedListResult.DataModels.Abstractions;
using PagedListResult.DataModels.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
#pragma warning disable CS1584

#endregion

namespace PagedListResult.DataModels.Models.Result
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of a paged.
    /// </summary>
    /// <typeparam name="TSource">Type of the source.</typeparam>
    /// <seealso cref="T:AggregatedGenericResultMessage.Result{System.Collections.Generic.IList{TSource}}"/>
    /// <seealso cref="T:PagedListResult.Common.Abstractions.IPagedResult{TSource}"/>
    /// =================================================================================================
    public class PagedResult<TSource> : Result<IList<TSource>>, IPagedResult<TSource>
        where TSource : class
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     True if has previous page, false if not.
        /// </summary>
        /// =================================================================================================
        private bool? _hasPreviousPage;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     True if has next page, false if not.
        /// </summary>
        /// =================================================================================================
        private bool? _hasNextPage;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="PagedResult{TSource}"/> class.
        /// </summary>
        /// =================================================================================================
        public PagedResult() { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets information describing the execution of current paged result.
        /// </summary>
        /// <value>
        ///     Information describing the execution of current paged result.
        /// </value>
        /// =================================================================================================
        public virtual PagedExecDetailsResult ExecutionDetails { get; set; } = new PagedExecDetailsResult();

        /// <inheritdoc/>
        public virtual int CurrentPage { get; set; }

        /// <inheritdoc/>
        public virtual int PageCount { get; set; }

        /// <inheritdoc/>
        public virtual int PageSize { get; set; }

        /// <inheritdoc/>
        public virtual int RowCount { get; set; }

        /// <inheritdoc/>
        public virtual bool HasPreviousPage
        {
            get => _hasPreviousPage.IsNull() ? CurrentPage > 1 : (bool)_hasPreviousPage!;
            set => _hasPreviousPage = (value != CurrentPage > 1) ? CurrentPage > 1 : null;
        }

        /// <inheritdoc/>
        public virtual bool HasNextPage
        {
            get => _hasNextPage.IsNull() ? CurrentPage < PageCount : (bool)_hasNextPage!;
            set => _hasNextPage = (value != CurrentPage < PageCount) ? CurrentPage < PageCount : null;
        }

        /// <inheritdoc cref="AggregatedGenericResultMessage.Result"/>
        [XmlArray]
        public override IList<TSource> Response { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Map paged result.
        /// </summary>
        /// <remarks>
        ///     RzR, 10-Nov-23.
        /// </remarks>
        /// <typeparam name="TDestination">Destination type result/response.</typeparam>
        /// <param name="data">.</param>
        /// <returns>
        ///     A PagedResult&lt;TDestination&gt;
        /// </returns>
        /// =================================================================================================
        public virtual PagedResult<TDestination> Map<TDestination>(IEnumerable<TDestination> data)
            where TDestination : class
            => new PagedResult<TDestination>
            {
                Response = data.ToList(),
                CurrentPage = CurrentPage,
                PageCount = PageCount,
                PageSize = PageSize,
                RowCount = RowCount,
                IsSuccess = IsSuccess,
                Messages = Messages,
                ExecutionDetails = ExecutionDetails,

                HasNextPage = HasNextPage,
                HasPreviousPage = HasPreviousPage
            };
    }
}