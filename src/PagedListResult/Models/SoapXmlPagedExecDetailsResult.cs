// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2023-12-27 01:57
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-27 02:08
// ***********************************************************************
//  <copyright file="SoapXmlPagedExecDetailsResult.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

#endregion

namespace PagedListResult.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     (Serializable) encapsulates the result of a SOAP XML paged execute details.
    /// </summary>
    /// <remarks></remarks>
    /// =================================================================================================
    [Serializable]
    [DataContract(Name = "SoapXmlPagedExecDetailsResult")]
    [XmlRoot(IsNullable = false)]
    public class SoapXmlPagedExecDetailsResult
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Initializes a new instance of the <see cref="SoapXmlPagedExecDetailsResult" /> class.</summary>
        /// <remarks></remarks>
        /// =================================================================================================
        public SoapXmlPagedExecDetailsResult()
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the execution time milliseconds.</summary>
        /// <value>The execution filters time milliseconds.</value>
        /// =================================================================================================
        [DataMember(Name = "ExecutionTimeMs", IsRequired = true)]
        public long ExecutionTimeMs { get; set; } = -1;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the execution date.</summary>
        /// <value>The execution date.</value>
        /// =================================================================================================
        [DataMember(Name = "ExecutionDate", IsRequired = true)]
        public DateTime? ExecutionDate { get; set; } = DateTime.Now;
    }
}