// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 13:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="FilterConditionType.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace PagedListResult.DataModels.Enums
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Conditions between filters.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    public enum FilterConditionType
    {
        /// <summary>
        ///     AND condition
        /// </summary>
        And,

        /// <summary>
        ///     OR condition
        /// </summary>
        Or
    }
}