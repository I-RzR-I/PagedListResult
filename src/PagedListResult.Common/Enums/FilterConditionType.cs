// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-24 14:56
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 00:41
// ***********************************************************************
//  <copyright file="FilterConditionType.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace PagedListResult.Common.Enums
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