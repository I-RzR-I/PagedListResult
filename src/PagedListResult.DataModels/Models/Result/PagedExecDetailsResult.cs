// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 13:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="PagedExecDetailsResult.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using PagedListResult.DataModels.Extensions;
using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace PagedListResult.DataModels.Models.Result
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Encapsulates the result of a paged execute.</summary>
    /// <remarks>RzR, 10-Nov-23.</remarks>
    /// =================================================================================================
    public class PagedExecDetailsResult
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Default constructor.</summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// =================================================================================================
        public PagedExecDetailsResult() { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <param name="executionTimeMs">The execution filters time milliseconds.</param>
        /// <param name="executionDate">The execution date.</param>
        /// =================================================================================================
        public PagedExecDetailsResult(long executionTimeMs, DateTime executionDate)
        {
            ExecutionTimeMs = executionTimeMs.IsNull() ? -1 : executionTimeMs;
            ExecutionDate = executionDate.IsNull() ? DateTime.Now : executionDate;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the execution time milliseconds.</summary>
        /// <value>The execution filters time milliseconds.</value>
        /// =================================================================================================
        public long ExecutionTimeMs { get; private set; } = -1;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the execution date.</summary>
        /// <value>The execution date.</value>
        /// =================================================================================================
        public DateTime? ExecutionDate { get; private set; } = DateTime.Now;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Sets execution time milliseconds.</summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <param name="execMs">The execute in milliseconds.</param>
        /// <param name="execDate">The execute date.</param>
        /// =================================================================================================
        public void SetExecutionTimeMs(long execMs, DateTime execDate)
        {
            ExecutionDate = execDate;
            ExecutionTimeMs = execMs;
        }
    }
}