// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-10 01:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-10 01:16
// ***********************************************************************
//  <copyright file="PagedExecResult.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using System;

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace PagedListResult.Common.Models.Result
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Encapsulates the result of a paged execute.</summary>
    /// <remarks>RzR, 10-Nov-23.</remarks>
    /// =================================================================================================
    public class PagedExecDetailsResult
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Default constructor.</summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        ///=================================================================================================
        public PagedExecDetailsResult() { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <param name="executionTimeMs">The execution filters time milliseconds.</param>
        /// <param name="executionDate">The execution date.</param>
        ///=================================================================================================
        public PagedExecDetailsResult(long executionTimeMs, DateTime executionDate)
        {
            ExecutionTimeMs = executionTimeMs;
            ExecutionDate = executionDate.IsNull() ? DateTime.Now : executionDate;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the execution time milliseconds.</summary>
        /// <value>The execution filters time milliseconds.</value>
        ///=================================================================================================
        public long ExecutionTimeMs { get; private set; } = -1;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the execution date.</summary>
        /// <value>The execution date.</value>
        /// =================================================================================================
        public DateTime? ExecutionDate { get; private set; } = DateTime.Now;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Sets execution time milliseconds.</summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <param name="execMs">The execute in milliseconds.</param>
        /// <param name="execDate">The execute date.</param>
        ///=================================================================================================
        public void SetExecutionTimeMs(long execMs, DateTime execDate)
        {
            ExecutionDate = execDate;
            ExecutionTimeMs = execMs;
        }
    }
}