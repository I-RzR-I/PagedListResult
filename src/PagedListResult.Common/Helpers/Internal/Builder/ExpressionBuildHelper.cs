// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-26 08:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 20:58
// ***********************************************************************
//  <copyright file="ExpressionBuildHelper.cs" company="">
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
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.CommonExtensions.Reflection;
using PagedListResult.Common.Extensions.Internal;
using PagedListResult.Common.Extensions.Internal.Common;
using PagedListResult.DataModels.Enums;
using System;
using System.Linq.Expressions;

#endregion

namespace PagedListResult.Common.Helpers.Internal.Builder
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Expression builder search in properties.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    internal static class ExpressionBuildHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build expression in dependence of current property type.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="property">Current property.</param>
        /// <param name="propertyType">Current property type.</param>
        /// <param name="sourceExpression">Source expression with is provided as proof.</param>
        /// <returns>Return built expression in dependence of type (nullable or not)</returns>
        /// =================================================================================================
        internal static IResult<Expression> BuildExpressionByPropertyType(
            MemberExpression property, Type propertyType,
            Expression sourceExpression)
        {
            try
            {
                if (propertyType.IsNullablePropType() || propertyType.IsStringPropType())
                {
                    var notNullProp = Expression.NotEqual(property, Expression.Constant(null, propertyType));

                    return Result<Expression>
                        .Success(Expression.AndAlso(notNullProp, Expression.LessThanOrEqual(property, sourceExpression)));
                }

                return Result<Expression>.Success(sourceExpression);
            }
            catch (Exception e)
            {
                return Result<Expression>.Failure().WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build expression in dependence of current property type.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Current type of entity.</typeparam>
        /// <param name="property">Current property.</param>
        /// <param name="propertyType">Current property type.</param>
        /// <param name="sourceExpression">Source expression with is provided as proof.</param>
        /// <param name="parameter">Parameter expression (x)</param>
        /// <returns>Return built expression in dependence of type (nullable or not)</returns>
        /// =================================================================================================
        internal static IResult<Expression<Func<TSource, bool>>> BuildExpressionByPropertyType<TSource>(
            MemberExpression property, Type propertyType,
            Expression<Func<TSource, bool>> sourceExpression, ParameterExpression parameter)
        {
            try
            {
                if (propertyType.IsNullablePropType() || propertyType.IsStringPropType())
                {
                    var notNullProp = Expression.NotEqual(property, Expression.Constant(null, propertyType));

                    return Result<Expression<Func<TSource, bool>>>
                        .Success(Expression.Lambda<Func<TSource, bool>>(
                            Expression.AndAlso(notNullProp, sourceExpression.Body), parameter));
                }

                return Result<Expression<Func<TSource, bool>>>.Success(sourceExpression);
            }
            catch (Exception e)
            {
                return Result<Expression<Func<TSource, bool>>>.Failure().WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build expression with property null value check.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Current type of entity.</typeparam>
        /// <param name="property">Current property.</param>
        /// <param name="propertyType">Current property type.</param>
        /// <param name="sourceExpression">Source expression with is provided as proof.</param>
        /// <param name="parameter">Parameter expression (x)</param>
        /// <returns>Return built expression with null property value check.</returns>
        /// =================================================================================================
        internal static IResult<Expression<Func<TSource, bool>>> BuildExpressionWithNotNull<TSource>(
            MemberExpression property, Type propertyType,
            Expression<Func<TSource, bool>> sourceExpression, ParameterExpression parameter)
        {
            try
            {
                var notNullProp = Expression.NotEqual(property, Expression.Constant(null, propertyType));

                return Result<Expression<Func<TSource, bool>>>
                    .Success(Expression.Lambda<Func<TSource, bool>>(
                        Expression.AndAlso(notNullProp, sourceExpression.Body), parameter));
            }
            catch (Exception e)
            {
                return Result<Expression<Func<TSource, bool>>>.Failure().WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build expression function by filter type.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Current type of entity.</typeparam>
        /// <param name="property">Current property.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="compareObjLeft">Compare object left side.</param>
        /// <param name="filter">Request filter.</param>
        /// <returns>An IResult&lt;Expression&lt;Func&lt;TSource,bool&gt;&gt;&gt;</returns>
        /// =================================================================================================
        internal static IResult<Expression<Func<TSource, bool>>> BuildExpressionFunctionByFilterType<TSource>(
            MemberExpression property, ParameterExpression parameter, object compareObjLeft,
            FilterType filter)
        {
            try
            {
                var binaryExpr = property.BuildBinaryExpression(compareObjLeft, filter);
                if (binaryExpr.IsNull())
                    return Result<Expression<Func<TSource, bool>>>.Success();

                if (property.Type.IsNullablePropType() && filter.AllowNullInResult())
                {
                    var nullBinary = Expression.Equal(property, Expression.Constant(null, property.Type));
                    var body = Expression.Or(binaryExpr, nullBinary);

                    return Result<Expression<Func<TSource, bool>>>
                        .Success(Expression.Lambda<Func<TSource, bool>>(body, parameter));
                }

                return Result<Expression<Func<TSource, bool>>>
                    .Success(Expression.Lambda<Func<TSource, bool>>(binaryExpr, parameter));
            }
            catch (Exception e)
            {
                return Result<Expression<Func<TSource, bool>>>.Failure().WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Builds expression by filter.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="property">Current property.</param>
        /// <param name="compareObjLeft">Compare object left side.</param>
        /// <param name="filter">Request filter.</param>
        /// <returns>An IResult&lt;Expression&gt;</returns>
        /// =================================================================================================
        internal static IResult<Expression> BuildExpressionByFilter(MemberExpression property, object compareObjLeft, FilterType filter)
        {
            try
            {
                var binaryExpr = property.BuildBinaryExpression(compareObjLeft, filter);
                if (binaryExpr.IsNull())
                    return Result<Expression>.Success();

                if (property.Type.IsNullablePropType() && filter.AllowNullInResult())
                {
                    var nullBinary = Expression.Equal(property, Expression.Constant(null, property.Type));

                    return Result<Expression>.Success(Expression.Or(binaryExpr, nullBinary));
                }

                return Result<Expression>.Success(binaryExpr);
            }
            catch (Exception e)
            {
                return Result<Expression>.Failure().WithError(e);
            }
        }
    }
}