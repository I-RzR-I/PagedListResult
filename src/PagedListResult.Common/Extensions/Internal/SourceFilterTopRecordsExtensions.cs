// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 20:36
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-02 20:49
// ***********************************************************************
//  <copyright file="SourceFilterTopRecordsExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;
using RzR.ResultMessage.Pagination.Core.Extensions.Internal.Common;
using RzR.ResultMessage.Pagination.Core.Helpers.Internal;
using RzR.ResultMessage.Pagination.Core.Helpers.Internal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace RzR.ResultMessage.Pagination.Core.Extensions.Internal
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Source query filter top predefined records.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    internal static class SourceFilterTopRecordsExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get predefined record query.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query.</typeparam>
        /// <param name="source">Required. Source query.</param>
        /// <param name="ids">Optional. Record ids(values), The default value is null.</param>
        /// <param name="defaultPrimaryKeys">Default primary keys name.</param>
        /// <param name="isInclude">(Optional) Include specified record ids or not.</param>
        /// <returns>The predefined records in top.</returns>
        ///=================================================================================================
        internal static IQueryable<TSource> GetPredefinedRecordsInTop<TSource>(
            this IQueryable<TSource> source,
            ICollection<string> ids, ICollection<string> defaultPrimaryKeys, bool isInclude = true)
        {
            if (source.IsNull() || defaultPrimaryKeys.IsNullOrEmptyEnumerable())
                return Enumerable.Empty<TSource>().AsQueryable();

            if (ids.IsNotNull() && ids!.Any() && ids.All(x => !string.IsNullOrEmpty(x)))
                return source.Where(CreateSearchPredicateForSpecificProperties<TSource>(ids.ToList(),
                    defaultPrimaryKeys, typeof(TSource), isInclude));

            return Enumerable.Empty<TSource>().AsQueryable();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Create search for predefined records.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source query.</typeparam>
        /// <param name="searchValues">Search values.</param>
        /// <param name="props">Props name.</param>
        /// <param name="type">Entity type.</param>
        /// <param name="isEqualsMethod">(Optional) Use .Equals() is by default.</param>
        /// <returns>An expression that evaluates to a Func&lt;TSource,bool&gt;</returns>
        ///=================================================================================================
        private static Expression<Func<TSource, bool>> CreateSearchPredicateForSpecificProperties<TSource>(
            IList<string> searchValues, IEnumerable<string> props, Type type, bool isEqualsMethod = true)
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
                if(prop.IsNullOrEmpty()) continue;
                foreach (var text in searchValues)
                {
                    var property = Expression.Property(parameter, prop);
                    Expression toStringToLower;
                    if (property.Type.IsStringPropType())
                    {
                        toStringToLower = Expression.Call(property, toLowerMethod.Response);
                    }
                    else
                    {
                        var toStringToLowerResult = ExpressionMethodHelper.GetStringLowerCasePropertyAccess(property);
                        if (toStringToLowerResult.IsSuccess.IsFalse())
                            ThrowHelper.Exception(toStringToLowerResult.GetFirstMessage());

                        toStringToLower = toStringToLowerResult.Response;
                    }

                    var right = Expression.Call(Expression.Constant(text), toLowerMethod.Response);

                    if (isEqualsMethod)
                    {
                        var body = Expression.Call(toStringToLower, equalsMethod.Response, right);

                        var predicateExpression = Expression.Lambda<Func<TSource, bool>>(body, parameter);
                        predicate = predicate == null
                            ? predicateExpression
                            : Expression.Lambda<Func<TSource, bool>>(
                                Expression.Or(predicate.Body, predicateExpression.Body),
                                parameter);
                    }
                    else
                    {
                        var notEqual = Expression.NotEqual(toStringToLower, right);
                        var predicateExpression = Expression.Lambda<Func<TSource, bool>>(notEqual, parameter);
                        predicate = predicate == null
                            ? predicateExpression
                            : Expression.Lambda<Func<TSource, bool>>(
                                Expression.And(predicate.Body, predicateExpression.Body),
                                parameter);
                    }
                }
            }

            return predicate;
        }
    }
}