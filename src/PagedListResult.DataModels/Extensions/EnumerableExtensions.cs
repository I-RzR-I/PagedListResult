// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 17:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="EnumerableExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using CodeSource;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace PagedListResult.DataModels.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An enumerable extensions.
    /// </summary>
    /// =================================================================================================
    [CodeSource(SourceUrl = "https://github.com/I-RzR-I/DomainCommonExtensions", AuthorName = "RzR", Copyright = "RzR", Version = 1.0D)]
    internal static class EnumerableExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Check is enumerable is null or empty.
        /// </summary>
        /// <typeparam name="T">Enumerable type.</typeparam>
        /// <param name="source">Source data.</param>
        /// <returns>
        ///     True if null or empty enumerable, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsNullOrEmptyEnumerable<T>(this IEnumerable<T> source)
            => source.IsNull() || source.Any().IsFalse();
    }
}