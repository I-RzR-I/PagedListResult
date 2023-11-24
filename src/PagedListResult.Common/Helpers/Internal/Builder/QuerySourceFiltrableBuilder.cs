// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-03 01:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 21:46
// ***********************************************************************
//  <copyright file="QuerySourceFiltrableBuilder.cs" company="">
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
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Enums;
using PagedListResult.Common.Extensions.Filters.PropertyFilterQuery;
using PagedListResult.Common.Extensions.Internal;
using PagedListResult.Common.Helpers.Internal.Common;
using PagedListResult.Common.Models.Internal;
using PagedListResult.Common.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace PagedListResult.Common.Helpers.Internal.Builder
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Filtrable source query builder.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    internal static class QuerySourceFiltrableBuilder
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Builds filterable query.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="filterLink">(Optional) The filter link.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        internal static IResult<IQueryable<TSource>> BuildFilterableQuery<TSource>(
            IQueryable<TSource> query,
            IEnumerable<DataFilter> filters, FilterConditionType filterLink = FilterConditionType.And) where TSource : class
        {
            try
            {
                var queryFilters = (filters ?? new List<DataFilter>()).ToList();
                if (queryFilters.IsNullOrEmptyEnumerable())
                    return Result<IQueryable<TSource>>.Success(query);

                var filterLinkCriteria = new List<Expression<Func<TSource, bool>>>();
                foreach (var filter in queryFilters.OrderBy(x => x.FilterApplyOrder))
                {
                    var propParamData = QueryableParamPropBuilder.GetQueryableParamProp<TSource>(filter.FilterValue.PropertyName, typeof(TSource));
                    var parameter = propParamData.Response.ParameterExpression;

                    var propFilter = BuildExpressionPropFilterQuery<TSource>(
                        filter.FilterValue.Condition,
                        filter.FilterValue.PropertyName,
                        filter.FilterValue.Values,
                        new[] { filter.FilterValue.CompareValue },
                        propParamData);

                    if (propFilter.IsSuccess.IsFalse())
                        ThrowHelper.Exception(propFilter.GetFirstMessage());

                    var propFilterFunc = Expression.Lambda<Func<TSource, bool>>(propFilter.Response, parameter);

                    if (filter.Dependencies.Count.IsZero())
                        filterLinkCriteria.Add(propFilterFunc);

                    if (filter.Dependencies.Count.IsGreaterThanZero())
                    {
                        var linkCriteriaOr = new List<Expression<Func<TSource, bool>>>();
                        var linkCriteriaAnd = new List<Expression<Func<TSource, bool>>>();

                        foreach (var dependency in filter.Dependencies)
                        {
                            var depend = BuildExpressionPropFilterQuery<TSource>(
                                dependency.FilterValue.Condition,
                                dependency.FilterValue.PropertyName,
                                dependency.FilterValue.Values,
                                new[] { dependency.FilterValue.CompareValue });

                            if (depend.IsSuccess.IsFalse())
                                ThrowHelper.Exception(depend.GetFirstMessage());
                            var dependFunc = Expression.Lambda<Func<TSource, bool>>(depend.Response, parameter);

                            switch (dependency.ParentFilterLinkType)
                            {
                                case FilterConditionType.And:
                                    var and = Expression.AndAlso(propFilterFunc.Body, dependFunc.Body);
                                    var andBody = (BinaryExpression)new ParameterReplacer(parameter).Visit(and);

                                    linkCriteriaAnd.Add(Expression.Lambda<Func<TSource, bool>>(andBody!, parameter));
                                    break;
                                case FilterConditionType.Or:
                                    var or = Expression.Or(propFilterFunc.Body, dependFunc.Body);
                                    var orBody = (BinaryExpression)new ParameterReplacer(parameter).Visit(or);

                                    linkCriteriaOr.Add(Expression.Lambda<Func<TSource, bool>>(orBody!, parameter));
                                    break;
                                default:
                                    ThrowHelper.ArgumentOutOfRangeException(
                                        nameof(dependency.ParentFilterLinkType), dependency.ParentFilterLinkType, "Specified filter is not in range!");
                                    break;
                            }
                        }

                        var dependFilter = BuildLambdaDependenciesFilters(linkCriteriaOr, linkCriteriaAnd, parameter);
                        if (dependFilter.IsSuccess.IsFalse())
                            ThrowHelper.Exception(dependFilter.GetFirstMessage());

                        filterLinkCriteria.Add(dependFilter.Response);
                    }
                }

                var filteredQuery = filterLink == FilterConditionType.And
                    ? filterLinkCriteria.All()
                    : filterLinkCriteria.Any();

                return Result<IQueryable<TSource>>.Success(query.Where(filteredQuery));
            }
            catch (Exception e)
            {
                return Result<IQueryable<TSource>>.Failure().WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Builds simple property filter query.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValues">The property values.</param>
        /// <param name="compareValues">The compare values.</param>
        /// <returns>An IQueryable&lt;TSource&gt;</returns>
        /// =================================================================================================
        internal static IResult<IQueryable<TSource>> BuildSimplePropFilterQuery<TSource>(
            IQueryable<TSource> query, FilterType filterType, string propertyName,
            IEnumerable<object> propertyValues, IEnumerable<object> compareValues) where TSource : class
        {
            try
            {
                switch (filterType)
                {
                    case FilterType.Equals:
                        query = query.PropertyEquals(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.NotEquals:
                        query = query.PropertyNotEquals(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.StartsWith:
                        query = query.PropertyStartsWith(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.DoesNotStartWith:
                        query = query.PropertyDoesNotStartWith(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.EndsWith:
                        query = query.PropertyEndsWith(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.DoesNotEndsWith:
                        query = query.PropertyDoesNotEndsWith(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.Contains:
                        query = query.PropertyContains(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.SensitiveContains:
                        query = query.PropertySensitiveContains(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.DoesNotContains:
                        query = query.PropertyDoesNotContains(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.SensitiveDoesNotContains:
                        query = query.PropertySensitiveDoesNotContains(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.GreaterThan:
                        query = query.PropertyGreaterThan(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.GreaterThanOrEquals:
                        query = query.PropertyGreaterThanOrEquals(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.LessThan:
                        query = query.PropertyLessThan(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.LessThanOrEquals:
                        query = query.PropertyLessThanOrEquals(propertyName, propertyValues.FirstOrDefault());
                        break;
                    case FilterType.Between:
                        query = query.PropertyBetween(propertyName, propertyValues.FirstOrDefault(), compareValues.FirstOrDefault());
                        break;
                    case FilterType.IsIn:
                        query = query.PropertyIsIn(propertyName, propertyValues);
                        break;
                    case FilterType.IsNotIn:
                        query = query.PropertyIsNotIn(propertyName, propertyValues);
                        break;
                    case FilterType.IsNull:
                        query = query.PropertyIsNull(propertyName);
                        break;
                    case FilterType.IsNotNull:
                        query = query.PropertyIsNotNull(propertyName);
                        break;
                    default:
                        ThrowHelper.ArgumentOutOfRangeException(nameof(filterType), filterType, "Specified filter is not defined!");
                        break;
                }

                return Result<IQueryable<TSource>>.Success(query);
            }
            catch (Exception e)
            {
                return Result<IQueryable<TSource>>.Failure().WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Builds expression property filter query.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValues">The property values.</param>
        /// <param name="compareValues">The compare values.</param>
        /// <param name="propParamData">(Optional) Information describing the property parameter.</param>
        /// <returns>An IResult&lt;Expression&gt;</returns>
        /// =================================================================================================
        private static IResult<Expression> BuildExpressionPropFilterQuery<TSource>(
            FilterType filterType, string propertyName, IEnumerable<object> propertyValues, IEnumerable<object> compareValues,
            IResult<QueryableParamPropDto> propParamData = null)
        {
            try
            {
                if (propParamData.IsNull())
                {
                    propParamData = QueryableParamPropBuilder.GetQueryableParamProp<TSource>(propertyName, typeof(TSource));
                    if (propParamData.IsSuccess.IsFalse())
                        ThrowHelper.Exception(propParamData.GetFirstMessage());
                }

                var propValues = propertyValues as object[] ?? propertyValues.ToArray();
                var propCompValues = compareValues as object[] ?? compareValues.ToArray();

                var expr = propParamData!.Response.PropertyExpression
                    .BuildCompareBinaryBodyByFilter<TSource>(filterType, propValues, propCompValues, propParamData.Response.ParameterExpression);

                return Result<Expression>.Success(expr);
            }
            catch (Exception e)
            {
                return Result<Expression>.Failure().WithError(e);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Builds lambda dependencies filters.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="linkCriteriaOr">The link criteria or.</param>
        /// <param name="linkCriteriaAnd">The link criteria and.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>An IResult&lt;Expression&lt;Func&lt;TSource,bool&gt;&gt;&gt;</returns>
        /// =================================================================================================
        private static IResult<Expression<Func<TSource, bool>>> BuildLambdaDependenciesFilters<TSource>(
            IReadOnlyCollection<Expression<Func<TSource, bool>>> linkCriteriaOr,
            IReadOnlyCollection<Expression<Func<TSource, bool>>> linkCriteriaAnd, ParameterExpression parameter)
        {
            try
            {
                Expression<Func<TSource, bool>> expressionResult = null;

                if (linkCriteriaOr.Count > 0 && linkCriteriaAnd.Count > 0)
                {
                    var orBinExpr = Expression.Or(linkCriteriaOr.Any().Body, linkCriteriaAnd.All().Body);
                    var binExpr = (BinaryExpression)new ParameterReplacer(parameter).Visit(orBinExpr);
                    expressionResult = Expression.Lambda<Func<TSource, bool>>(binExpr!, parameter);
                }
                else if (linkCriteriaOr.Count > 0)
                {
                    expressionResult = linkCriteriaOr.Any();
                }
                else if (linkCriteriaAnd.Count > 0)
                {
                    expressionResult = linkCriteriaAnd.All();
                }

                return Result<Expression<Func<TSource, bool>>>.Success(expressionResult);
            }
            catch (Exception e)
            {
                return Result<Expression<Func<TSource, bool>>>.Failure().WithError(e);
            }
        }
    }
}