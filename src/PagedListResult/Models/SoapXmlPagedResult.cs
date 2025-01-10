// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2023-12-26 23:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-27 02:54
// ***********************************************************************
//  <copyright file="SoapXmlPagedResult.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

#endregion

namespace PagedListResult.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of a SOAP/XML paged.
    /// </summary>
    /// <seealso cref="T:AggregatedGenericResultMessage.SoapResult"/>
    /// =================================================================================================
    [Serializable]
    [DataContract(Name = "SoapXmlPagedResult")]
    [XmlRoot(IsNullable = false)]
    public class SoapXmlPagedResult : SoapResult
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="SoapXmlPagedResult" /> class.
        /// </summary>
        /// =================================================================================================
        public SoapXmlPagedResult()
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Current page.
        /// </summary>
        /// <value>
        ///     The current page.
        /// </value>
        /// =================================================================================================
        [DataMember(Name = "CurrentPage", IsRequired = true)]
        public int CurrentPage { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Total page number.
        /// </summary>
        /// <value>
        ///     The number of pages.
        /// </value>
        /// =================================================================================================
        [DataMember(Name = "PageCount", IsRequired = true)]
        public int PageCount { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Items per page.
        /// </summary>
        /// <value>
        ///     The size of the page.
        /// </value>
        /// =================================================================================================
        [DataMember(Name = "PageSize", IsRequired = true)]
        public int PageSize { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Total filtered items.
        /// </summary>
        /// <value>
        ///     The number of rows.
        /// </value>
        /// =================================================================================================
        [DataMember(Name = "RowCount", IsRequired = true)]
        public int RowCount { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the has next page.
        /// </summary>
        /// <value>
        ///     The has next page.
        /// </value>
        /// =================================================================================================
        [DataMember(Name = "HasNextPage", IsRequired = true)]
        public bool HasNextPage { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the has previous page.
        /// </summary>
        /// <value>
        ///     The has previous page.
        /// </value>
        /// =================================================================================================
        [DataMember(Name = "HasPreviousPage", IsRequired = true)]
        public bool HasPreviousPage { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets information describing the execution of current paged result.
        /// </summary>
        /// <value>
        ///     Information describing the execution of current paged result.
        /// </value>
        /// =================================================================================================
        [DataMember(Name = "ExecutionDetails", IsRequired = false)]
        public SoapXmlPagedExecDetailsResult ExecutionDetails { get; set; }
    }
}