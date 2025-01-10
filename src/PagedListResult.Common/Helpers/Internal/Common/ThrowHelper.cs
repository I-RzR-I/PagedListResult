// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-26 09:17
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 02:10
// ***********************************************************************
//  <copyright file="ThrowHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using PagedListResult.DataModels.Enums;
using System;

#endregion

namespace PagedListResult.Common.Helpers.Internal.Common
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Throw exception helper.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    internal static class ThrowHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Throw simple exception.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="message">Exception message.</param>
        /// =================================================================================================
        internal static void Exception(string message)
            => throw new Exception(message);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Throw simple argument exception.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="message">Exception message.</param>
        /// =================================================================================================
        internal static void ArgumentException(string message)
            => throw new ArgumentException(message);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Throw simple argument exception.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        /// =================================================================================================
        internal static void ArgumentException(string message, Exception innerException)
            => throw new ArgumentException(message, innerException);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Throw simple argument null exception.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="message">Exception message.</param>
        /// =================================================================================================
        internal static void ArgumentNullException(string message)
            => throw new ArgumentNullException(message);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Throw simple argument null exception.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        /// =================================================================================================
        internal static void ArgumentNullException(string message, Exception innerException)
            => throw new ArgumentNullException(message, innerException);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Throw simple not supported exception.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="message">Exception message.</param>
        /// =================================================================================================
        internal static void NotSupportedException(string message)
            => throw new NotSupportedException(message);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Throw simple not supported exception.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        /// =================================================================================================
        internal static void NotSupportedException(string message, Exception innerException)
            => throw new NotSupportedException(message, innerException);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Throw simple argument out of range exception.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="propertyName">Current property name.</param>
        /// <param name="value">Current property value.</param>
        /// <param name="message">Exception message.</param>
        /// =================================================================================================
        internal static void ArgumentOutOfRangeException(string propertyName, object value, string message)
            => throw new ArgumentOutOfRangeException(propertyName, value, message);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Throw simple not supported filter exception.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="filter">Current filter.</param>
        /// <param name="propertyName">current property.</param>
        /// <param name="customMessage">(Optional) Additional custom message.</param>
        /// =================================================================================================
        internal static void NotSupportedFilterException(FilterType filter, string propertyName, string customMessage = "")
            => throw new NotSupportedException($"{filter} is not supported for type: '{propertyName}'. {customMessage}");
    }
}