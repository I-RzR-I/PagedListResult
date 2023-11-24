// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-24 22:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-24 22:04
// ***********************************************************************
//  <copyright file="TypeExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using DomainCommonExtensions.DataTypeExtensions;

namespace PagedListResult.Common.Extensions.Internal.Common
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>A type extensions.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    internal static class TypeExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable)
        ///     Nullable dictionary type.
        /// </summary>
        ///=================================================================================================
        private static readonly Dictionary<Type, Type> NullableTypeDict = new Dictionary<Type, Type>
        {
            [typeof(byte)] = typeof(byte?),
            [typeof(sbyte)] = typeof(sbyte?),
            [typeof(short)] = typeof(short?),
            [typeof(ushort)] = typeof(ushort?),
            [typeof(int)] = typeof(int?),
            [typeof(uint)] = typeof(uint?),
            [typeof(long)] = typeof(long?),
            [typeof(ulong)] = typeof(ulong?),
            [typeof(float)] = typeof(float?),
            [typeof(double)] = typeof(double?),
            [typeof(decimal)] = typeof(decimal?),
            [typeof(bool)] = typeof(bool?),
            [typeof(char)] = typeof(char?),
            [typeof(Guid)] = typeof(Guid?),
            [typeof(DateTime)] = typeof(DateTime?),
            [typeof(DateTimeOffset)] = typeof(DateTimeOffset?),
            [typeof(TimeSpan)] = typeof(TimeSpan?)
        };

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get nullable type.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Property type.</param>
        /// <returns>The nullable type.</returns>
        ///=================================================================================================
        internal static Type GetNullableType(this Type type)
        {
            type.ThrowIfArgNull(nameof(type));

            return NullableTypeDict.FirstOrDefault(x => x.Key == type).Value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Check if property is type of string.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Property type.</param>
        /// <returns>True if string property type, false if not.</returns>
        ///=================================================================================================
        internal static bool IsStringPropType(this Type type)
        {
            type.ThrowIfArgNull(nameof(type));

            return (type == typeof(string));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Check if property is type of bool.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Property type.</param>
        /// <returns>True if bool property type, false if not.</returns>
        ///=================================================================================================
        internal static bool IsBoolPropType(this Type type)
        {
            type.ThrowIfArgNull(nameof(type));

            return (type == typeof(bool) || type == typeof(bool?));
        }
    }
}