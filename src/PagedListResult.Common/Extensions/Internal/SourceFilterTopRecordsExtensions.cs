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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Helpers.Internal;
using PagedListResult.Common.Helpers.Internal.Common;

#endregion

namespace PagedListResult.Common.Extensions.Internal
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
                    var toStringToLower = ExpressionMethodHelper.GetStringLowerCasePropertyAccess(property);
                    if(toStringToLower.IsSuccess.IsFalse())
                        ThrowHelper.Exception(toStringToLower.GetFirstMessage());

                    var right = Expression.Call(Expression.Constant(text), toLowerMethod.Response);

                    if (isEqualsMethod)
                    {
                        var body = Expression.Call(toStringToLower.Response, equalsMethod.Response, right);

                        var predicateExpression = Expression.Lambda<Func<TSource, bool>>(body, parameter);
                        predicate = predicate == null
                            ? predicateExpression
                            : Expression.Lambda<Func<TSource, bool>>(
                                Expression.Or(predicate.Body, predicateExpression.Body),
                                parameter);
                    }
                    else
                    {
                        var notEqual = Expression.NotEqual(toStringToLower.Response, right);
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