// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2026-07-31 13:31
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-07-31 13:31
// ***********************************************************************
//  <copyright file="ExpressionObjectCompareHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.Domain.Primitives;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;

#endregion

namespace RzR.ResultMessage.Pagination.Core.Helpers.Internal.Common
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Safe type conversion helper.</summary>
    /// <remarks>
    ///     <see cref="Convert.ChangeType(object,Type)"/> only supports <see cref="IConvertible"/> target
    ///     types. It cannot convert to <see cref="Guid"/>, <see cref="Guid"/>?, <see cref="DateTimeOffset"/>,
    ///     <see cref="TimeSpan"/>, or enums by name, which are common filter/comparison target types. This
    ///     helper falls back to <see cref="TypeDescriptor"/> converters for those cases.
    /// </remarks>
    /// =================================================================================================
    internal static class SafeTypeConvertHelper
    {
        /// <summary>
        ///     Caches the resolved <see cref="TypeConverter"/> per target type. <see cref="TypeDescriptor.GetConverter(Type)"/>
        ///     walks attributes and takes a process-wide lock; a filter with a large IN-list would otherwise
        ///     re-resolve the converter once per value. The built-in converters used here are stateless and
        ///     thread-safe.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, TypeConverter> ConverterCache =
            new ConcurrentDictionary<Type, TypeConverter>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Change the type of <paramref name="value"/> to <paramref name="targetType"/>.</summary>
        /// <remarks>RzR.</remarks>
        /// <param name="value">Current value.</param>
        /// <param name="targetType">Target type.</param>
        /// <returns>The converted value.</returns>
        /// =================================================================================================
        internal static object ChangeType(object value, Type targetType)
        {
            if (value.IsNull())
                return null;

            if (targetType.IsInstanceOfType(value))
                return value;

            if (value is string stringValue)
            {
                var converter = ConverterCache.GetOrAdd(targetType, TypeDescriptor.GetConverter);
                if (converter.CanConvertFrom(typeof(string)))
                    return converter.ConvertFromInvariantString(stringValue);
            }

            return Convert.ChangeType(value, targetType);
        }
    }
}
