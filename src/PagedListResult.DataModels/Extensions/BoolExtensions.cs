// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 17:13
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="BoolExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using CodeSource;

#endregion

namespace PagedListResult.DataModels.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An boolean extensions.
    /// </summary>
    /// =================================================================================================
    [CodeSource(SourceUrl = "https://github.com/I-RzR-I/DomainCommonExtensions", AuthorName = "RzR", Copyright = "RzR", Version = 1.0D)]
    internal static class BoolExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Check if source value is equals with false.
        /// </summary>
        /// <param name="source">Source object to be checked.</param>
        /// <returns>
        ///     True if false, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsFalse(this bool source)
            => source.IsNull() || source.Equals(false);
    }
}