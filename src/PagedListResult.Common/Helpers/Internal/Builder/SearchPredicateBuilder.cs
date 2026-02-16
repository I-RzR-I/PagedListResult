// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 15:43
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-02 17:23
// ***********************************************************************
//  <copyright file="SearchPredicateBuilder.cs" company="">
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
using DomainCommonExtensions.ArraysExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Extensions.Internal.Common;
using PagedListResult.Common.Helpers.Internal.Common;

#endregion

namespace PagedListResult.Common.Helpers.Internal.Builder
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Search predicate builder.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    internal static class SearchPredicateBuilder
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Create expression for filter text.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Type source query type.</typeparam>
        /// <param name="text">Search text.</param>
        /// <param name="type">(Optional) Type.</param>
        /// <returns>The new text filter expression.</returns>
        ///=================================================================================================
        internal static IResult<Expression<Func<TSource, bool>>> CreateTextFilterExpression<TSource>(
            string text, Type type = null)
        {
            try
            {
                type ??= typeof(TSource);
                var props = type.GetProperties().Where(x => x.PropertyType.IsStringPropType()).ToList();

                if (props.Any().IsFalse()) 
                    return Result<Expression<Func<TSource, bool>>>.Success();

                var searchPredicate = CreateSearchPredicateForSpecificProperties<TSource>(text, props, type);
                if (searchPredicate.IsSuccess.IsFalse())
                    ThrowHelper.Exception(searchPredicate.GetFirstMessage());

                return Result<Expression<Func<TSource, bool>>>
                    .Success(searchPredicate.Response);
            }
            catch (Exception e)
            {
                return Result<Expression<Func<TSource, bool>>>
                    .Failure()
                    .WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates text filter expression all types.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="text">Search text.</param>
        /// <param name="type">(Optional) Type.</param>
        /// <returns>
        ///     The new text filter expression all types.
        /// </returns>
        /// =================================================================================================
        internal static IResult<Expression<Func<TSource, bool>>> CreateTextFilterExpressionAllTypes<TSource>(
            string text, Type type = null)
        {
            try
            {
                type ??= typeof(TSource);
                var props = type.GetProperties().ToList();

                if (props.Any().IsFalse()) 
                    return Result<Expression<Func<TSource, bool>>>.Success();

                var searchPredicate = CreateSearchPredicateForSpecificAllTypeProperties<TSource>(text, props, type);
                if (searchPredicate.IsSuccess.IsFalse())
                    ThrowHelper.Exception(searchPredicate.GetFirstMessage());

                return Result<Expression<Func<TSource, bool>>>
                    .Success(searchPredicate.Response);
            }
            catch (Exception e)
            {
                return Result<Expression<Func<TSource, bool>>>
                    .Failure()
                    .WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Create expression for filter text.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Type source query type.</typeparam>
        /// <param name="text">Search text.</param>
        /// <param name="properties">Custom search properties list.</param>
        /// <param name="type">(Optional) Type.</param>
        /// <returns>The new text filter expression.</returns>
        ///=================================================================================================
        internal static IResult<Expression<Func<TSource, bool>>> CreateTextFilterExpression<TSource>(
            string text, ICollection<string> properties, Type type = null)
        {
            try
            {
                type ??= typeof(TSource);
                properties = properties.Select(x => x.FirstCharToUpper()).ToList();
                var props = type.GetProperties().Where(x => properties.Contains(x.Name) && x.PropertyType.IsStringPropType()).ToList();

                if (props.IsNullOrEmptyEnumerable())
                    return Result<Expression<Func<TSource, bool>>>.Success();

                var searchPredicate = CreateSearchPredicateForSpecificProperties<TSource>(text, props, type);
                if (searchPredicate.IsSuccess.IsFalse())
                    ThrowHelper.Exception(searchPredicate.GetFirstMessage());

                return Result<Expression<Func<TSource, bool>>>
                    .Success(searchPredicate.Response);
            }
            catch (Exception e)
            {
                return Result<Expression<Func<TSource, bool>>>
                    .Failure()
                    .WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates text filter expression all types.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="text">Search text.</param>
        /// <param name="properties">Custom search properties list.</param>
        /// <param name="type">(Optional) Type.</param>
        /// <returns>
        ///     The new text filter expression all types.
        /// </returns>
        /// =================================================================================================
        internal static IResult<Expression<Func<TSource, bool>>> CreateTextFilterExpressionAllTypes<TSource>(
            string text, ICollection<string> properties, Type type = null)
        {
            try
            {
                type ??= typeof(TSource);
                properties = properties.Select(x => x.FirstCharToUpper()).ToList();
                var props = type.GetProperties().Where(x => properties.Contains(x.Name)).ToList();

                if (props.IsNullOrEmptyEnumerable())
                    return Result<Expression<Func<TSource, bool>>>.Success();

                var searchPredicate = CreateSearchPredicateForSpecificAllTypeProperties<TSource>(text, props, type);
                if (searchPredicate.IsSuccess.IsFalse())
                    ThrowHelper.Exception(searchPredicate.GetFirstMessage());

                return Result<Expression<Func<TSource, bool>>>
                    .Success(searchPredicate.Response);
            }
            catch (Exception e)
            {
                return Result<Expression<Func<TSource, bool>>>
                    .Failure()
                    .WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Create search predicate for specific properties.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Type source query type.</typeparam>
        /// <param name="text">Search text.</param>
        /// <param name="props">Custom search properties list.</param>
        /// <param name="type">Type.</param>
        /// <returns>The new search predicate for specific properties.</returns>
        ///=================================================================================================
        private static IResult<Expression<Func<TSource, bool>>> CreateSearchPredicateForSpecificProperties<TSource>(
            string text, IEnumerable<PropertyInfo> props, Type type)
        {
            try
            {
                Expression<Func<TSource, bool>> predicate = null;
                var parameter = Expression.Parameter(type, "x");

                var toLowerMethod = ExpressionMethodHelper.GetStringToLowerMethod();
                if (toLowerMethod.IsSuccess.IsFalse())
                    ThrowHelper.Exception(toLowerMethod.GetFirstMessage());

                var containsMethod = ExpressionMethodHelper.GetStringContainsMethod();
                if (containsMethod.IsSuccess.IsFalse())
                    ThrowHelper.Exception(containsMethod.GetFirstMessage());

                foreach (var prop in props)
                {
                    var property = Expression.Property(parameter, prop.Name);
                    var notNullProp = Expression.NotEqual(property, Expression.Constant(null, property.Type));

                    var left = Expression.Call(property, toLowerMethod.Response);
                    var right = Expression.Call(Expression.Constant(text), toLowerMethod.Response);
                    var body = Expression.Call(left, containsMethod.Response, right);

                    var predicateExpression = Expression.Lambda<Func<TSource, bool>>(body, parameter);
                    var expressionNotNullAndCheckPropertyValue = Expression.Lambda<Func<TSource, bool>>(
                        Expression.AndAlso(notNullProp, predicateExpression.Body),
                        parameter);

                    predicate = predicate.IsNull()
                        ? expressionNotNullAndCheckPropertyValue
                        : Expression.Lambda<Func<TSource, bool>>(
                            Expression.Or(predicate!.Body, expressionNotNullAndCheckPropertyValue.Body),
                            parameter);
                }

                return Result<Expression<Func<TSource, bool>>>
                    .Success(predicate);
            }
            catch (Exception e)
            {
                return Result<Expression<Func<TSource, bool>>>
                    .Failure()
                    .WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates search predicate for specific all type properties.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="text">Search text.</param>
        /// <param name="props">Custom search properties list.</param>
        /// <param name="type">Type.</param>
        /// <returns>
        ///     The new search predicate for specific all type properties.
        /// </returns>
        /// =================================================================================================
        private static IResult<Expression<Func<TSource, bool>>> CreateSearchPredicateForSpecificAllTypeProperties<TSource>(
            string text, IEnumerable<PropertyInfo> props, Type type)
        {
            try
            {
                Expression<Func<TSource, bool>> predicate = null;
                var parameter = Expression.Parameter(type, "x");

                var toLowerMethod = ExpressionMethodHelper.GetStringToLowerMethod();
                if (toLowerMethod.IsSuccess.IsFalse())
                    ThrowHelper.Exception(toLowerMethod.GetFirstMessage());

                var containsMethod = ExpressionMethodHelper.GetStringContainsMethod();
                if (containsMethod.IsSuccess.IsFalse())
                    ThrowHelper.Exception(containsMethod.GetFirstMessage());

                var toStringMethod = ExpressionMethodHelper.GetToStringMethod();
                if (toStringMethod.IsSuccess.IsFalse())
                    ThrowHelper.Exception(toStringMethod.GetFirstMessage());

                foreach (var prop in props)
                {
                    var property = Expression.Property(parameter, prop.Name);

                    Expression condition;

                    var canBeNull = !property.Type.IsValueType || Nullable.GetUnderlyingType(property.Type).IsNotNull();

                    var leftContains = ExpressionMethodHelper.GetStringLowerCasePropertyAccess(property).Response;
                    var rightContains = Expression.Call(
                        Expression.Call(Expression.Constant(text), toStringMethod.Response), toLowerMethod.Response);
                    var body = Expression.Call(leftContains, containsMethod.Response, rightContains);

                    if (canBeNull)
                    {
                        var notNull = Expression.NotEqual(property, Expression.Constant(null, property.Type));
                        condition = Expression.AndAlso(notNull, body);
                    }
                    else
                    {
                        condition = body;
                    }

                    if (predicate.IsNull())
                        predicate = Expression.Lambda<Func<TSource, bool>>(condition, parameter);
                    else
                    {
                        var combined = Expression.OrElse(predicate!.Body, condition);
                        predicate = Expression.Lambda<Func<TSource, bool>>(combined, parameter);
                    }
                }

                return Result<Expression<Func<TSource, bool>>>.Success(predicate);
            }
            catch (Exception e)
            {
                return Result<Expression<Func<TSource, bool>>>
                    .Failure()
                    .WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Create search for predefined records.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query.</typeparam>
        /// <param name="searchValues">Search values.</param>
        /// <param name="props">Props name.</param>
        /// <param name="type">Entity type.</param>
        /// <param name="isEqualsMethod">(Optional) Use .Equals() is by default.</param>
        /// <returns>The new search predicate for specific properties.</returns>
        ///=================================================================================================
        internal static IResult<Expression<Func<TSource, bool>>> CreateSearchPredicateForSpecificProperties<TSource>(
            IList<string> searchValues, IEnumerable<string> props, Type type, bool isEqualsMethod = true)
        {
            try
            {
                Expression<Func<TSource, bool>> predicate = null;
                var parameter = Expression.Parameter(type, "x");

                var toLowerMethod = ExpressionMethodHelper.GetStringToLowerMethod();
                if (toLowerMethod.IsSuccess.IsFalse())
                    ThrowHelper.Exception(toLowerMethod.GetFirstMessage());

                var equalsMethod = ExpressionMethodHelper.GetEqualsMethod();
                if (equalsMethod.IsSuccess.IsFalse())
                    ThrowHelper.Exception(equalsMethod.GetFirstMessage());

                foreach (var prop in props)
                {
                    foreach (var text in searchValues)
                    {
                        var property = Expression.Property(parameter, prop);

                        var toStringToLower = ExpressionMethodHelper.GetStringLowerCasePropertyAccess(property);
                        if (toStringToLower.IsSuccess.IsFalse())
                            ThrowHelper.Exception(toStringToLower.GetFirstMessage());

                        var right = Expression.Call(Expression.Constant(text), toLowerMethod.Response);

                        if (isEqualsMethod)
                        {
                            var body = Expression.Call(toStringToLower.Response, equalsMethod.Response, right);

                            var predicateExpression = Expression.Lambda<Func<TSource, bool>>(body, parameter);
                            predicate = predicate.IsNull()
                                ? predicateExpression
                                : Expression.Lambda<Func<TSource, bool>>(
                                    Expression.Or(predicate!.Body, predicateExpression.Body),
                                    parameter);
                        }
                        else
                        {
                            var notEqual = Expression.NotEqual(toStringToLower.Response, right);
                            var predicateExpression = Expression.Lambda<Func<TSource, bool>>(notEqual, parameter);
                            predicate = predicate.IsNull()
                                ? predicateExpression
                                : Expression.Lambda<Func<TSource, bool>>(
                                    Expression.And(predicate!.Body, predicateExpression.Body),
                                    parameter);
                        }
                    }
                }

                return Result<Expression<Func<TSource, bool>>>
                    .Success(predicate);
            }
            catch (Exception e)
            {
                return Result<Expression<Func<TSource, bool>>>
                    .Failure()
                    .WithError(e);
            }
        }
    }
}