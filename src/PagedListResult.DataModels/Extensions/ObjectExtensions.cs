// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 17:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="ObjectExtensions.cs" company="RzR SOFT & TECH">
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
    ///     An object extensions.
    /// </summary>
    /// =================================================================================================
    [CodeSource(SourceUrl = "https://github.com/I-RzR-I/DomainCommonExtensions", AuthorName = "RzR", Copyright = "RzR", Version = 1.0D)]
    internal static class ObjectExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Is null.
        /// </summary>
        /// <param name="obj">Object to be checked.</param>
        /// <returns>
        ///     True if null, false if not.
        /// </returns>
        /// =================================================================================================
        internal static bool IsNull(this object obj)
            => obj == null;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Is not null.
        /// </summary>
        /// <param name="obj">Object to be checked.</param>
        /// <returns>
        ///     True if not null, false if not.
        /// </returns>
        /// =================================================================================================
        internal static bool IsNotNull(this object obj)
            => obj != null;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An object extension method that query if 'sourceObject' is null or default.
        /// </summary>
        /// <remarks>
        ///     RzR, 13-Nov-23.
        /// </remarks>
        /// <param name="sourceObject">The sourceObject to act on.</param>
        /// <returns>
        ///     True if null or default, false if not.
        /// </returns>
        /// =================================================================================================
        internal static bool IsNullOrDefault(this object sourceObject)
            => sourceObject.IsNull() || sourceObject == default;
    }
}