// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.DataModels
//  Author           : RzR
//  Created On       : 2025-02-23 20:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-02-23 20:14
// ***********************************************************************
//  <copyright file="DataPredefinedFilterDefinition.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;

#endregion

namespace PagedListResult.DataModels.Models.Request
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A data pre selected filter definition.
    /// </summary>
    /// =================================================================================================
    public class DataPredefinedFilterDefinition
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name of the predefined field.
        /// </summary>
        /// <value>
        ///     The name of the pre selected field.
        /// </value>
        /// =================================================================================================
        public string PredefinedFieldName { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     List of record/s id/s witch will be injected in top of grid result.
        /// </summary>
        /// <value>
        ///     The predefined records.
        /// </value>
        /// =================================================================================================
        public ICollection<string> PredefinedRecords { get; set; } = new HashSet<string>();
    }
}