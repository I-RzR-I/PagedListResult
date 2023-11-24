// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-24 14:56
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-24 14:58
// ***********************************************************************
//  <copyright file="DataFilterDependence.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using PagedListResult.Common.Enums;

#endregion

namespace PagedListResult.Common.Models.Request
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Data filter dependence.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    public class DataFilterDependence
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Dependence filter condition.</summary>
        /// <value>The type of the parent filter link.</value>
        ///=================================================================================================
        public FilterConditionType ParentFilterLinkType { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Filter value.</summary>
        /// <value>The filter value.</value>
        ///=================================================================================================
        public DataFilterValue FilterValue { get; set; }
    }
}