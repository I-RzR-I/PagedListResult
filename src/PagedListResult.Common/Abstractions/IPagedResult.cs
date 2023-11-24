// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-24 14:56
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 00:40
// ***********************************************************************
//  <copyright file="IPagedResult.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage.Abstractions;
using PagedListResult.Common.Models.Result;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace PagedListResult.Common.Abstractions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Paged result.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// <typeparam name="TSource">Type of paged result.</typeparam>
    ///=================================================================================================
    [XmlInclude(typeof(PagedResult<>))]
    public interface IPagedResult<TSource> : IResult<IList<TSource>> where TSource : class
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Current page.</summary>
        /// <value>The current page.</value>
        ///=================================================================================================
        int CurrentPage { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Items on current page.</summary>
        /// <value>The number of pages.</value>
        ///=================================================================================================
        int PageCount { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Items per page.</summary>
        /// <value>The size of the page.</value>
        ///=================================================================================================
        int PageSize { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Total filtered items.</summary>
        /// <value>The number of rows.</value>
        ///=================================================================================================
        int RowCount { get; set; }
    }
}