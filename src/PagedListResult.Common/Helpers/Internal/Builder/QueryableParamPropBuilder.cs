// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-31 16:19
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-13 16:37
// ***********************************************************************
//  <copyright file="QueryableParamPropBuilder.cs" company="">
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
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Models.Internal;
using System;
using System.Linq.Expressions;

#endregion

namespace PagedListResult.Common.Helpers.Internal.Builder
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Queryable parameter and property builder.</summary>
    /// <remarks>RzR, 13-Nov-23.</remarks>
    /// =================================================================================================
    internal static class QueryableParamPropBuilder
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Get Expression parameter/property details.</summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query type.</typeparam>
        /// <param name="propertyName">Required. Property name.</param>
        /// <param name="queryType">(Optional) Optional. The default value is null.</param>
        /// <returns>The queryable parameter property.</returns>
        /// =================================================================================================
        internal static IResult<QueryableParamPropDto> GetQueryableParamProp<TSource>(string propertyName, Type queryType = null)
        {
            propertyName.ThrowIfArgNullOrEmpty(nameof(propertyName));

            try
            {
                queryType ??= typeof(TSource);
                const string parameterX = "x";

                var parameter = Expression.Parameter(queryType, parameterX);
                var property = Expression.Property(parameter, propertyName);

                return Result<QueryableParamPropDto>
                    .Success(
                        new QueryableParamPropDto
                        {
                            ParamName = parameterX, 
                            QueryType = queryType, 
                            ParameterExpression = parameter, 
                            PropertyExpression = property
                        });
            }
            catch (Exception e)
            {
                return Result<QueryableParamPropDto>.Failure().WithError(e);
            }
        }
    }
}