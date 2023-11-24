// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-24 14:56
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 00:41
// ***********************************************************************
//  <copyright file="FilterType.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace PagedListResult.Common.Enums
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Record filter type.</summary>
    /// <remarks>RzR, 13-Nov-23.</remarks>
    ///=================================================================================================
    public enum FilterType
    {
        /// <summary>
        ///     Equals filter
        /// </summary>
        Equals,

        /// <summary>
        ///     Not equals filter
        /// </summary>
        NotEquals,

        /// <summary>
        ///     Check if property value start with X filter
        /// </summary>
        StartsWith,

        /// <summary>
        ///     Check if property value does not start with X filter
        /// </summary>
        DoesNotStartWith,

        /// <summary>
        ///     Check if property value end with X filter
        /// </summary>
        EndsWith,

        /// <summary>
        ///     Check if property value does not end with X filter
        /// </summary>
        DoesNotEndsWith,

        /// <summary>
        ///     Check if property value contains X filter
        /// </summary>
        Contains,

        /// <summary>
        ///     Check if property value contains X filter (Case sensitive)
        /// </summary>
        SensitiveContains,

        /// <summary>
        ///     Check if property value does not contains X filter
        /// </summary>
        DoesNotContains,

        /// <summary>
        ///     Check if property value does not contains X filter (Case sensitive)
        /// </summary>
        SensitiveDoesNotContains,

        /// <summary>
        ///     Check if property value is greater than X filter
        /// </summary>
        GreaterThan,

        /// <summary>
        ///     Check if property value is greater than or equals X filter
        /// </summary>
        GreaterThanOrEquals,

        /// <summary>
        ///     Check if property value is less than X filter
        /// </summary>
        LessThan,

        /// <summary>
        ///     Check if property value is less than or equals X filter
        /// </summary>
        LessThanOrEquals,

        /// <summary>
        ///     Check if property value are between X and Y filter
        /// </summary>
        Between,

        /// <summary>
        ///     Check if property value/s are in X filter
        /// </summary>
        IsIn,

        /// <summary>
        ///     Check if property value/s are not in X filter
        /// </summary>
        IsNotIn,

        /// <summary>
        ///     Check if property value/s IS NULL
        /// </summary>
        IsNull,

        /// <summary>
        ///     Check if property value/s IS NOT NULL
        /// </summary>
        IsNotNull
    }
}