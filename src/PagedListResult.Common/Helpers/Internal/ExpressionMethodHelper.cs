// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-24 17:31
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-24 17:40
// ***********************************************************************
//  <copyright file="ExpressionMethodHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using AggregatedGenericResultMessage.Extensions.Result;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using DomainCommonExtensions.CommonExtensions;
using PagedListResult.Common.Helpers.Internal.ConstNamesHelper;

#endregion

namespace PagedListResult.Common.Helpers.Internal
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Expression method helper.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    internal static class ExpressionMethodHelper
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable)
        ///     Runtime method infos.
        /// </summary>
        ///=================================================================================================
        private static readonly ConcurrentDictionary<string, MethodInfo> MethodInfos = new ConcurrentDictionary<string, MethodInfo>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get string to lower method info.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <returns>The string to lower method.</returns>
        ///=================================================================================================
        internal static IResult<MethodInfo> GetStringToLowerMethod()
        {
            try
            {
                MethodInfos.TryGetValue(MethodInfoNamesHelper.ToLowerMethodName, out var toLowerMethod);

                if (toLowerMethod.IsNotNull())
                    return Result<MethodInfo>.Success(toLowerMethod);

                toLowerMethod = typeof(string).GetMethod(MethodInfoNamesHelper.ToLowerMethodName, Type.EmptyTypes);
                MethodInfos.TryAdd(MethodInfoNamesHelper.ToLowerMethodName, toLowerMethod);

                return Result<MethodInfo>.Success(toLowerMethod);
            }
            catch (Exception e)
            {
                return Result<MethodInfo>.Failure().WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get ToString() method.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <returns>to string method.</returns>
        ///=================================================================================================
        internal static IResult<MethodInfo> GetToStringMethod()
        {
            try
            {
                MethodInfos.TryGetValue(MethodInfoNamesHelper.ToStringMethodName, out var toStringMethod);

                if (toStringMethod.IsNotNull())
                    return Result<MethodInfo>.Success(toStringMethod);

                toStringMethod = typeof(string).GetMethod(MethodInfoNamesHelper.ToStringMethodName, Type.EmptyTypes);
                MethodInfos.TryAdd(MethodInfoNamesHelper.ToStringMethodName, toStringMethod);

                return Result<MethodInfo>.Success(toStringMethod);
            }
            catch (Exception e)
            {
                return Result<MethodInfo>.Failure().WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get Equals() method.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <returns>The equals method.</returns>
        ///=================================================================================================
        internal static IResult<MethodInfo> GetEqualsMethod()
        {
            try
            {
                MethodInfos.TryGetValue(MethodInfoNamesHelper.EqualsMethodName, out var equalsMethod);

                if (equalsMethod.IsNotNull())
                    return Result<MethodInfo>.Success(equalsMethod);

                // Get Equals instance method
                equalsMethod = typeof(object).GetMethod(MethodInfoNamesHelper.EqualsMethodName, new[] { typeof(object) });
                if (equalsMethod.IsNull())
                    // Get Equals static method
                    equalsMethod = typeof(object).GetMethod(MethodInfoNamesHelper.EqualsMethodName,
                        new[] { typeof(object), typeof(object) });

                MethodInfos.TryAdd(MethodInfoNamesHelper.EqualsMethodName, equalsMethod);

                return Result<MethodInfo>.Success(equalsMethod);
            }
            catch (Exception e)
            {
                return Result<MethodInfo>.Failure().WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get string contains method.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <returns>The string contains method.</returns>
        ///=================================================================================================
        internal static IResult<MethodInfo> GetStringContainsMethod()
        {
            try
            {
                MethodInfos.TryGetValue(MethodInfoNamesHelper.ContainsMethodName, out var containsMethod);
                if (containsMethod.IsNotNull())
                    return Result<MethodInfo>.Success(containsMethod);

                containsMethod = typeof(string).GetMethod(MethodInfoNamesHelper.ContainsMethodName, new[] { typeof(string) });
                MethodInfos.TryAdd(MethodInfoNamesHelper.ContainsMethodName, containsMethod);

                return Result<MethodInfo>.Success(containsMethod);
            }
            catch (Exception e)
            {
                return Result<MethodInfo>.Failure().WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get method info.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="propertyType">Prop type.</param>
        /// <param name="methodName">Method name.</param>
        /// <returns>The type method information.</returns>
        ///=================================================================================================
        internal static IResult<MethodInfo> GetTypeMethodInfo(Type propertyType, string methodName)
        {
            try
            {
                var keyName = $"{propertyType.FullName}_{methodName}";
                MethodInfos.TryGetValue(keyName, out var method);
                if (method.IsNotNull())
                    return Result<MethodInfo>.Success(method);

                method = propertyType.GetMethod(methodName, new[] { propertyType });
                MethodInfos.TryAdd(keyName, method);

                return Result<MethodInfo>.Success(method);
            }
            catch (Exception e)
            {
                return Result<MethodInfo>.Failure().WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get property ToString().ToLower()</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="propertyAccess">Property/parameter.</param>
        /// <returns>The string lower case property access.</returns>
        ///=================================================================================================
        internal static IResult<MethodCallExpression> GetStringLowerCasePropertyAccess(MemberExpression propertyAccess)
        {
            try
            {
                var toStringMethod = typeof(object).GetMethod(MethodInfoNamesHelper.ToStringMethodName, Type.EmptyTypes);
                var toLowerMethod = typeof(string).GetMethod(MethodInfoNamesHelper.ToLowerMethodName, Type.EmptyTypes);

                if (toStringMethod.IsNotNull() && toLowerMethod.IsNotNull())
                {
                    return Result<MethodCallExpression>
                        .Success(Expression.Call(Expression.Call(propertyAccess, toStringMethod!), toLowerMethod!));
                }

                MethodInfos.TryGetValue(MethodInfoNamesHelper.ToStringMethodName, out toStringMethod);
                MethodInfos.TryGetValue(MethodInfoNamesHelper.ToLowerMethodName, out toLowerMethod);

                return Result<MethodCallExpression>
                    .Success(Expression.Call(Expression.Call(propertyAccess, toStringMethod!), toLowerMethod!));
            }
            catch (Exception e)
            {
                return Result<MethodCallExpression>.Failure().WithError(e);
            }
        }
    }
}