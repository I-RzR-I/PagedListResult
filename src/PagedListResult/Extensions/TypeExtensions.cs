// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2023-11-07 20:19
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 00:29
// ***********************************************************************
//  <copyright file="TypeExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable RedundantLambdaParameterType

#endregion

namespace PagedListResult.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Type extensions.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    internal static class TypeExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Get property or field.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Current type.</param>
        /// <param name="name">Property or field name.</param>
        /// <returns>The field or property.</returns>
        /// =================================================================================================
        internal static MemberInfo GetFieldOrProperty(this Type type, string name)
            => type.GetAllMembers().FirstOrDefault((MemberInfo mi) => mi.Name == name);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Get all inheritance type.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Current type.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the type inheritances in this
        ///     collection.
        /// </returns>
        /// =================================================================================================
        private static IEnumerable<Type> GetTypeInheritance(this Type type)
        {
            yield return type;
            var baseType = type.BaseType;
            while (baseType.IsNotNull())
            {
                yield return baseType;
                baseType = baseType?.BaseType;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Get all members.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Current type.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process all members in this collection.
        /// </returns>
        /// =================================================================================================
        private static IEnumerable<MemberInfo> GetAllMembers(this Type type)
            => type.GetTypeInheritance().Concat(type.GetInterfaces()).SelectMany((Type i) => i.GetDeclaredMembers());

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Get all declares members.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Current type.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the declared members in this
        ///     collection.
        /// </returns>
        /// =================================================================================================
        private static IEnumerable<MemberInfo> GetDeclaredMembers(this Type type)
            => type.GetTypeInfo().DeclaredMembers;
    }
}