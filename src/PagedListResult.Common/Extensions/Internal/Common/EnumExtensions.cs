// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-30 21:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-30 21:03
// ***********************************************************************
//  <copyright file="EnumExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

#endregion

using PagedListResult.DataModels.Enums;

namespace PagedListResult.Common.Extensions.Internal.Common
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Enum extensions.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    internal static class EnumExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Check if record filter allow null values in result.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="filter">Filter type.</param>
        /// <returns>True if we allow null in result, false if not.</returns>
        ///=================================================================================================
        public static bool AllowNullInResult(this FilterType filter)
            => filter == FilterType.LessThan || filter == FilterType.LessThanOrEquals;
    }
}