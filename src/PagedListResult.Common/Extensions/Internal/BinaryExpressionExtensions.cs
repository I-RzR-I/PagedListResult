// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-30 20:52
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-31 16:30
// ***********************************************************************
//  <copyright file="BinaryExpressionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Extensions.Internal.Common;
using PagedListResult.Common.Helpers.Internal;
using PagedListResult.Common.Helpers.Internal.Builder;
using PagedListResult.Common.Helpers.Internal.Common;
using PagedListResult.Common.Helpers.Internal.ConstNamesHelper;
using PagedListResult.DataModels.Enums;
using System.Reflection;

// ReSharper disable RedundantCast

#endregion

namespace PagedListResult.Common.Extensions.Internal
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Binary expression helper.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    internal static class BinaryExpressionExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Build binary expression.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="property">Member expression property.</param>
        /// <param name="compareObj">Object value.</param>
        /// <param name="filter">Property filter.</param>
        /// <returns>A BinaryExpression.</returns>
        ///=================================================================================================
        internal static BinaryExpression BuildBinaryExpression(
                    this MemberExpression property,
                    object compareObj, FilterType filter)
        {
            var expression = filter switch
            {
                FilterType.GreaterThan => property.BuildGreaterThanBinaryExpression(compareObj),
                FilterType.GreaterThanOrEquals => property.BuildGreaterThanOrEqualBinaryExpression(compareObj),
                FilterType.LessThan => property.BuildLessThanBinaryExpression(compareObj),
                FilterType.LessThanOrEquals => property.BuildLessThanOrEqualBinaryExpression(compareObj),
                _ => null
            };

            return expression;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Build compare expression by filter.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="property">Member expression property.</param>
        /// <param name="filter">Property filter.</param>
        /// <param name="objectValues">(Optional) Object values.</param>
        /// <param name="compareObjectValues">(Optional) Compare object values.</param>
        /// <param name="parameterExpression">(Optional) Parameter expression.</param>
        /// <returns>An Expression.</returns>
        ///=================================================================================================
        internal static Expression BuildCompareBinaryBodyByFilter<TSource>(
            this MemberExpression property, FilterType filter, object[] objectValues = null, object[] compareObjectValues = null,
            ParameterExpression parameterExpression = null)
        {
            property.ThrowIfArgNull(nameof(property));

            Expression body = null;
            Expression<Func<object>> expressionFilterValue = null;
            var nullConst = Expression.Constant(null);

            var filterObjectValue = objectValues?.FirstOrDefault();
            object compareObjValue = null;

            if (new List<FilterType>()
            {
                FilterType.Equals, FilterType.NotEquals,
                FilterType.StartsWith, FilterType.DoesNotStartWith,
                FilterType.EndsWith, FilterType.DoesNotEndsWith
            }.Contains(filter))
            {
                compareObjValue = Convert.ChangeType(filterObjectValue, property.Type.IsNullablePropType()
                    ? property.Type.GetNonNullableType()
                    : property.Type);

                expressionFilterValue = () => compareObjValue;
            }
            switch (filter)
            {
                case FilterType.GreaterThan:
                    if (property.Type.IsNullablePropType())
                    {
                        body = GetExpressionBody(property, filterObjectValue, filter);
                        if (body.IsNotNull())
                            return body;

                        body = Expression.GreaterThan(property, Expression.Constant(filterObjectValue));
                    }
                    else
                    {
                        body = Expression.GreaterThan(property, Expression.Constant(filterObjectValue));
                    }

                    if (!filter.AllowNullInResult() && (property.Type.IsNullablePropType() || property.Type.IsStringPropType()))
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }
                    break;
                case FilterType.GreaterThanOrEquals:
                    if (property.Type.IsNullablePropType())
                    {
                        body = GetExpressionBody(property, filterObjectValue, filter);
                        if (body.IsNotNull())
                            return body;

                        body = Expression.GreaterThanOrEqual(property, Expression.Constant(filterObjectValue));
                    }
                    else
                    {
                        body = Expression.GreaterThanOrEqual(property, Expression.Constant(filterObjectValue));
                    }

                    if (!filter.AllowNullInResult() && (property.Type.IsNullablePropType() || property.Type.IsStringPropType()))
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }
                    break;
                case FilterType.LessThan:
                    if (property.Type.IsNullablePropType())
                    {
                        body = GetExpressionBody(property, filterObjectValue, filter);
                        if (body.IsNotNull())
                            return body;

                        body = Expression.LessThan(property, Expression.Constant(filterObjectValue));
                    }
                    else
                    {
                        body = Expression.LessThan(property, Expression.Constant(filterObjectValue));
                    }

                    if (!filter.AllowNullInResult() && (property.Type.IsNullablePropType() || property.Type.IsStringPropType()))
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }
                    break;
                case FilterType.LessThanOrEquals:
                    if (property.Type.IsNullablePropType())
                    {
                        body = GetExpressionBody(property, filterObjectValue, filter);
                        if (body.IsNotNull())
                            return body;

                        body = Expression.LessThanOrEqual(property, Expression.Constant(filterObjectValue));
                    }
                    else
                    {
                        body = Expression.LessThanOrEqual(property, Expression.Constant(filterObjectValue));
                    }

                    if (!filter.AllowNullInResult() && (property.Type.IsNullablePropType() || property.Type.IsStringPropType()))
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }
                    break;

                case FilterType.Between:
                    parameterExpression.ThrowIfArgNull(nameof(parameterExpression));
                    if (compareObjectValues.IsNullOrEmptyEnumerable())
                        ThrowHelper.ArgumentException("Compare value must be not null!");

                    var filterCompareObjectValue = compareObjectValues?[0];

                    var leftCompareObj = property.Type.IsNullablePropType()
                        ? filterObjectValue.ChangeToNotNullType(property.Type)
                        : Convert.ChangeType(filterObjectValue, property.Type);
                    var rightCompareObj = property.Type.IsNullablePropType()
                        ? filterCompareObjectValue.ChangeToNotNullType(property.Type)
                        : Convert.ChangeType(filterCompareObjectValue, property.Type);

                    var lBody = GetExpressionBody(property, leftCompareObj, FilterType.GreaterThanOrEquals);
                    var rBody = GetExpressionBody(property, rightCompareObj, FilterType.LessThanOrEquals);

                    body = Expression.And(Expression.Lambda<Func<TSource, bool>>(lBody, parameterExpression).Body,
                        Expression.Lambda<Func<TSource, bool>>(rBody, parameterExpression).Body);
                    break;
                case FilterType.Equals:
                    if (property.Type.IsNullablePropType() && property.IsNotNull())
                    {
                        body = Expression.Call(property, GetMethod(property.Type, MethodInfoNamesHelper.EqualsMethodName), expressionFilterValue?.Body);
                    }
                    else
                    {
                        var constantExp = Expression.Constant(compareObjValue);
                        body = Expression.Call(property, GetMethod(property.Type, MethodInfoNamesHelper.EqualsMethodName), constantExp);
                    }

                    if (property.Type.IsNullablePropType() || property.Type.IsStringPropType())
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }
                    break;
                case FilterType.NotEquals:
                    if (property.Type.IsNullablePropType() && property.IsNotNull())
                    {
                        body = Expression.Call(property, GetMethod(property.Type, MethodInfoNamesHelper.EqualsMethodName), expressionFilterValue?.Body);
                    }
                    else
                    {
                        var constantExp = Expression.Constant(compareObjValue);
                        body = Expression.Call(property, GetMethod(property.Type, MethodInfoNamesHelper.EqualsMethodName), constantExp);
                    }

                    if (property.Type.IsNullablePropType() || property.Type.IsStringPropType())
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }

                    body = Expression.Not(body);
                    break;
                case FilterType.StartsWith:
                    if (property.Type.IsNullablePropType() && property.IsNotNull())
                    {
                        body = Expression.Call(property, GetMethod(property.Type, MethodInfoNamesHelper.StartsWithMethodName), expressionFilterValue?.Body);
                    }
                    else
                    {
                        var constantExp = Expression.Constant(compareObjValue);
                        body = Expression.Call(property, GetMethod(property.Type, MethodInfoNamesHelper.StartsWithMethodName), constantExp);
                    }

                    if (property.Type.IsNullablePropType() || property.Type.IsStringPropType())
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }
                    break;
                case FilterType.DoesNotStartWith:
                    if (property.Type.IsNullablePropType() && property.IsNotNull())
                    {
                        body = Expression.Call(property, GetMethod(property.Type, MethodInfoNamesHelper.StartsWithMethodName), expressionFilterValue?.Body);
                    }
                    else
                    {
                        var constantExp = Expression.Constant(compareObjValue);
                        body = Expression.Call(property, GetMethod(property.Type, MethodInfoNamesHelper.StartsWithMethodName), constantExp);
                    }

                    if (property.Type.IsNullablePropType() || property.Type.IsStringPropType())
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }

                    body = Expression.Not(body);
                    break;
                case FilterType.EndsWith:
                    if (property.Type.IsNullablePropType() && property.IsNotNull())
                    {
                        body = Expression.Call(property, GetMethod(property.Type, MethodInfoNamesHelper.EndsWithMethodName), expressionFilterValue?.Body);
                    }
                    else
                    {
                        var constantExp = Expression.Constant(compareObjValue);
                        body = Expression.Call(property, GetMethod(property.Type, MethodInfoNamesHelper.EndsWithMethodName), constantExp);
                    }

                    if (property.Type.IsNullablePropType() || property.Type.IsStringPropType())
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }
                    break;
                case FilterType.DoesNotEndsWith:
                    if (property.Type.IsNullablePropType() && property.IsNotNull())
                    {
                        body = Expression.Call(property, GetMethod(property.Type, MethodInfoNamesHelper.EndsWithMethodName), expressionFilterValue?.Body);
                    }
                    else
                    {
                        var constantExp = Expression.Constant(compareObjValue);
                        body = Expression.Call(property, GetMethod(property.Type, MethodInfoNamesHelper.EndsWithMethodName), constantExp);
                    }

                    if (property.Type.IsNullablePropType() || property.Type.IsStringPropType())
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }

                    body = Expression.Not(body);
                    break;
                case FilterType.Contains:
                    var leftContains = property.Type.IsStringPropType()
                        ? Expression.Call(property, ExpressionMethodHelper.GetToStringMethod().Response)
                        : ExpressionMethodHelper.GetStringLowerCasePropertyAccess(property).Response;
                    var rightContains = Expression.Call(Expression.Constant(filterObjectValue),
                        ExpressionMethodHelper.GetToStringMethod().Response);

                    body = Expression.Call(leftContains, ExpressionMethodHelper.GetStringContainsMethod().Response, rightContains);

                    if (property.Type.IsNullablePropType() || property.Type.IsStringPropType())
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }
                    break;
                case FilterType.DoesNotContains:
                    var leftDoesNotContains = property.Type.IsStringPropType()
                        ? Expression.Call(property, ExpressionMethodHelper.GetToStringMethod().Response)
                        : ExpressionMethodHelper.GetStringLowerCasePropertyAccess(property).Response;
                    var rightDoesNotContains = Expression.Call(Expression.Constant(filterObjectValue),
                        ExpressionMethodHelper.GetToStringMethod().Response);

                    body = Expression.Call(leftDoesNotContains, ExpressionMethodHelper.GetStringContainsMethod().Response, rightDoesNotContains);

                    if (property.Type.IsNullablePropType() || property.Type.IsStringPropType())
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }

                    body = Expression.Not(body);
                    break;
                case FilterType.SensitiveContains:
                    var leftSensitiveContains = property.Type.IsStringPropType()
                        ? Expression.Call(property, ExpressionMethodHelper.GetStringToLowerMethod().Response)
                        : ExpressionMethodHelper.GetStringLowerCasePropertyAccess(property).Response;
                    var rightSensitiveContains = Expression.Call(Expression.Constant(filterObjectValue),
                        ExpressionMethodHelper.GetStringToLowerMethod().Response);

                    body = Expression.Call(leftSensitiveContains,
                        ExpressionMethodHelper.GetStringContainsMethod().Response,
                        rightSensitiveContains);

                    if (property.Type.IsNullablePropType() || property.Type.IsStringPropType())
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }
                    break;
                case FilterType.SensitiveDoesNotContains:
                    var leftSensitiveDoesNotContains = property.Type.IsStringPropType()
                        ? Expression.Call(property, ExpressionMethodHelper.GetStringToLowerMethod().Response)
                        : ExpressionMethodHelper.GetStringLowerCasePropertyAccess(property).Response;
                    var rightSensitiveDoesNotContains = Expression.Call(Expression.Constant(filterObjectValue),
                        ExpressionMethodHelper.GetStringToLowerMethod().Response);

                    body = Expression.Call(leftSensitiveDoesNotContains,
                        ExpressionMethodHelper.GetStringContainsMethod().Response,
                        rightSensitiveDoesNotContains);

                    if (property.Type.IsNullablePropType() || property.Type.IsStringPropType())
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }

                    body = Expression.Not(body);
                    break;
                case FilterType.IsIn:
                    if (objectValues.IsNullOrEmptyEnumerable())
                        ThrowHelper.ArgumentException($"{nameof(objectValues)}, IN values must have data!");

                    foreach (var inValue in objectValues!)
                    {
                        var objValue = property.Type.IsNullablePropType()
                            ? inValue.ChangeToNotNullType(property.Type)
                            : Convert.ChangeType(inValue, property.Type);

                        var objExpr = Expression.Constant(objValue, property.Type);
                        var eqObj = Expression.Equal(property, objExpr);

                        body = body.IsNull()
                            ? eqObj
                            : Expression.Or(body!, eqObj);
                    }
                    break;
                case FilterType.IsNotIn:
                    if (objectValues.IsNullOrEmptyEnumerable())
                        ThrowHelper.ArgumentException($"{nameof(objectValues)}, NOT IN values must have data!");

                    foreach (var inValue in objectValues!)
                    {
                        var objValue = property.Type.IsNullablePropType()
                            ? inValue.ChangeToNotNullType(property.Type)
                            : Convert.ChangeType(inValue, property.Type);

                        var objExpr = Expression.Constant(objValue, property.Type);
                        var eqObj = Expression.NotEqual(property, objExpr);

                        body = body.IsNull()
                            ? eqObj
                            : Expression.AndAlso(body!, eqObj);
                    }
                    break;
                case FilterType.IsNull:
                    body = property.Type.IsNullablePropType()
                        ? (Expression)Expression.Equal(property, nullConst)
                        : Expression.Call(property, ExpressionMethodHelper.GetEqualsMethod().Response, nullConst);
                    break;
                case FilterType.IsNotNull:
                    body = property.Type.IsNullablePropType()
                        ? (Expression)Expression.NotEqual(property, nullConst)
                        : Expression.Not(Expression.Call(property, ExpressionMethodHelper.GetEqualsMethod().Response, nullConst));

                    if (property.Type.IsNullablePropType() || property.Type.IsStringPropType())
                    {
                        var notNullProp = Expression.NotEqual(property, nullConst);
                        body = Expression.AndAlso(notNullProp, body);
                    }
                    break;
                default:
                    ThrowHelper.ArgumentOutOfRangeException(nameof(filter), filter, "Specified filter is not in range!");
                    break;
            }

            return body;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets expression body.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="property">Member expression property.</param>
        /// <param name="compareObjLeft">The compare object left.</param>
        /// <param name="filter">Property filter.</param>
        /// <returns>The expression body.</returns>
        ///=================================================================================================
        private static Expression GetExpressionBody(MemberExpression property, object compareObjLeft, FilterType filter)
        {
            var exprBuilder = ExpressionBuildHelper.BuildExpressionByFilter(property, compareObjLeft, filter);

            if (exprBuilder.IsSuccess.IsFalse())
                ThrowHelper.Exception(exprBuilder.GetFirstMessage());

            return exprBuilder.Response;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets a method.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>The method.</returns>
        ///=================================================================================================
        private static MethodInfo GetMethod(Type propertyType, string methodName)
        {
            var methodInfo = ExpressionMethodHelper.GetTypeMethodInfo(propertyType, methodName);
            if (methodInfo.IsSuccess.IsFalse())
                ThrowHelper.Exception(methodInfo.GetFirstMessage());

            return methodInfo.Response;
        }
    }
}