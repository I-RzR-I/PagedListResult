// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 15:28
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-13 16:35
// ***********************************************************************
//  <copyright file="ObjectExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using System;

#endregion

namespace PagedListResult.Common.Extensions.Internal.Common
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Object extensions.</summary>
    /// <remarks>RzR, 13-Nov-23.</remarks>
    ///=================================================================================================
    internal static class ObjectExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Change object type to non-nullable type.</summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// <typeparam name="T">.</typeparam>
        /// <param name="value">Object value.</param>
        /// <returns>A T.</returns>
        ///=================================================================================================
        internal static T ChangeToNotNullType<T>(this object value)
        {
            var t = typeof(T);

            if (!t.IsGenericType || t.GetGenericTypeDefinition() != typeof(Nullable<>))
                return (T)Convert.ChangeType(value, t);
            if (value.IsNull())
                return default;

            t = Nullable.GetUnderlyingType(t);

            return (T)Convert.ChangeType(value, t!);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Change object type to non-nullable type.</summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// <param name="value">Object value.</param>
        /// <param name="conversion">Object type.</param>
        /// <returns>An object.</returns>
        ///=================================================================================================
        internal static object ChangeToNotNullType(this object value, Type conversion)
        {
            var t = conversion;

            if (!t.IsGenericType || t.GetGenericTypeDefinition() != typeof(Nullable<>))
                return Convert.ChangeType(value, t);
            if (value.IsNull())
                return null;

            t = Nullable.GetUnderlyingType(t);

            return Convert.ChangeType(value, t!);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An object extension method that query if 'sourceObject' is not null or default.
        /// </summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// <param name="sourceObject">The sourceObject to act on.</param>
        /// <returns>True if not null or default, false if not.</returns>
        ///=================================================================================================
        internal static bool IsNotNullOrDefault(this object sourceObject)
            => sourceObject.IsNotNull() && sourceObject != default;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An object extension method that query if 'sourceObject' is null or default.
        /// </summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// <param name="sourceObject">The sourceObject to act on.</param>
        /// <returns>True if null or default, false if not.</returns>
        ///=================================================================================================
        internal static bool IsNullOrDefault(this object sourceObject)
            => sourceObject.IsNull() || sourceObject == default;
    }
}