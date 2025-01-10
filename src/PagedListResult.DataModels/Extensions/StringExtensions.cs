// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 17:14
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="StringExtensions.cs" company="RzR SOFT & TECH">
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
    ///     A string extensions.
    /// </summary>
    /// =================================================================================================
    [CodeSource(SourceUrl = "https://github.com/I-RzR-I/DomainCommonExtensions", AuthorName = "RzR", Copyright = "RzR", Version = 1.0D)]
    internal static class StringExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Is Null or empty snippet.
        /// </summary>
        /// <param name="str">.</param>
        /// <returns>
        ///     True if the null or is empty, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
    }
}