// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 13:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="DataFilterDependence.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using PagedListResult.DataModels.Enums;

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace PagedListResult.DataModels.Models.Request
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Data filter dependence.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    public class DataFilterDependence
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Dependence filter condition.</summary>
        /// <value>The type of the parent filter link.</value>
        /// =================================================================================================
        public FilterConditionType ParentFilterLinkType { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Filter value.</summary>
        /// <value>The filter value.</value>
        /// =================================================================================================
        public DataFilterValue FilterValue { get; set; }
    }
}