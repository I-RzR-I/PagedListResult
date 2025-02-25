// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2025-02-23 21:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-02-23 23:10
// ***********************************************************************
//  <copyright file="BuildPredefinedFilterDto.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;

#endregion

namespace PagedListResult.Models.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A build predefined filter data transfer object.
    /// </summary>
    /// =================================================================================================
    internal class BuildPredefinedFilterDto
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object has identifiers.
        /// </summary>
        /// <value>
        ///     True if this object has identifiers, false if not.
        /// </value>
        /// =================================================================================================
        public bool HasIds { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the number of identifiers.
        /// </summary>
        /// <value>
        ///     The number of identifiers.
        /// </value>
        /// =================================================================================================
        public int IdsCount { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a list of identifiers of the predefined fields.
        /// </summary>
        /// <value>
        ///     A list of identifiers of the predefined fields.
        /// </value>
        /// =================================================================================================
        public ICollection<string> PredefinedFieldIds { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a list of names of the predefined fields.
        /// </summary>
        /// <value>
        ///     A list of names of the predefined fields.
        /// </value>
        /// =================================================================================================
        public ICollection<string> PredefinedFieldNames { get; set; }
    }
}