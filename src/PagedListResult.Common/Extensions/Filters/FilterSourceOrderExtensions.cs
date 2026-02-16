// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 16:45
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-02 17:07
// ***********************************************************************
//  <copyright file="FilterSourceOrderExtensions.cs" company="">
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
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Utilities.Ensure;
using PagedListResult.Common.Extensions.Internal;
using PagedListResult.Common.Helpers;
using PagedListResult.Common.Helpers.Internal.Common;
using PagedListResult.Common.Helpers.Internal.ConstNamesHelper;
using PagedListResult.DataModels.Enums;

#endregion

namespace PagedListResult.Common.Extensions.Filters
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Filter query source ordering.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    public static class FilterSourceOrderExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Order by property name.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="query">.</param>
        /// <param name="propertyName">.</param>
        /// <param name="direction">.</param>
        /// <param name="comparer">(Optional)</param>
        /// <param name="type">(Optional)</param>
        /// <param name="orderByDefaultProperty">(Optional) Order by default defined property.</param>
        /// <returns>An IOrderedQueryable&lt;TSource&gt;</returns>
        ///=================================================================================================
        public static IOrderedQueryable<TSource> OrderByWithDirection<TSource>(
            this IQueryable<TSource> query, string propertyName,
            OrderDirection direction, IComparer<object> comparer = null, Type type = null,
            bool orderByDefaultProperty = false) where TSource : class
        {
            query.ThrowIfNull("Current query can not be null!");
            direction.ThrowIfArgNull(nameof(direction));

            if (propertyName.IsNullOrEmpty() || orderByDefaultProperty.IsTrue())
            {
                var defaultProperty = ReflectionTypeStorage.GetDefaultOrderByPropertyInfo(type ?? typeof(TSource));
                if (defaultProperty.IsSuccess.IsFalse())
                    ThrowHelper.Exception(defaultProperty.GetFirstMessage());

                propertyName = defaultProperty.Response.Name;
            }

            return direction == OrderDirection.Desc
                ? query.OrderByDescending(propertyName, comparer, type)
                : query.OrderBy(propertyName, comparer, type);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Order by default defined property in attribute.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="query">.</param>
        /// <param name="direction">.</param>
        /// <param name="comparer">(Optional)</param>
        /// <param name="type">(Optional)</param>
        /// <returns>An IOrderedQueryable&lt;TSource&gt;</returns>
        ///=================================================================================================
        public static IOrderedQueryable<TSource> OrderByWithDirectionByDefaultProperty<TSource>(
            this IQueryable<TSource> query, OrderDirection direction,
            IComparer<object> comparer = null, Type type = null) where TSource : class
        {
            query.ThrowIfNull("Current query can not be null!");
            direction.ThrowIfArgNull(nameof(direction));

            var defaultProperty = ReflectionTypeStorage.GetDefaultOrderByPropertyInfo(type ?? typeof(TSource));
            if (defaultProperty.IsSuccess.IsFalse())
                ThrowHelper.Exception(defaultProperty.GetFirstMessage());

            var propertyName = defaultProperty.Response.Name;

            if (propertyName.IsNullOrEmpty())
                return query.OrderBy(x => 0);

            return direction == OrderDirection.Desc
                ? query.OrderByDescending(propertyName, comparer, type)
                : query.OrderBy(propertyName, comparer, type);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Order by property.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="query">.</param>
        /// <param name="propertyName">.</param>
        /// <param name="comparer">(Optional)</param>
        /// <param name="type">(Optional)</param>
        /// <returns>An IOrderedQueryable&lt;TSource&gt;</returns>
        ///=================================================================================================
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query,
            string propertyName, IComparer<object> comparer = null, Type type = null) where TSource : class
        {
            query.ThrowIfNull("Current query can not be null!");
            propertyName.ThrowIfArgNullOrEmpty(nameof(propertyName));

            return query.CallOrderedQueryable(OrderInfoNamesHelper.OrderBy, propertyName, comparer, type);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Order by descending.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="query">.</param>
        /// <param name="propertyName">.</param>
        /// <param name="comparer">(Optional)</param>
        /// <param name="type">(Optional)</param>
        /// <returns>An IOrderedQueryable&lt;TSource&gt;</returns>
        ///=================================================================================================
        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> query,
            string propertyName, IComparer<object> comparer = null, Type type = null) where TSource : class
        {
            query.ThrowIfNull("Current query can not be null!");
            propertyName.ThrowIfArgNullOrEmpty(nameof(propertyName));

            return query.CallOrderedQueryable(OrderInfoNamesHelper.OrderByDescending, propertyName, comparer, type);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>The order.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="query">.</param>
        /// <param name="propertyName">.</param>
        /// <param name="comparer">(Optional)</param>
        /// <param name="type">(Optional)</param>
        /// <returns>An IOrderedQueryable&lt;TSource&gt;</returns>
        ///=================================================================================================
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> query,
            string propertyName, IComparer<object> comparer = null, Type type = null) where TSource : class
        {
            query.ThrowIfNull("Current query can not be null!");
            propertyName.ThrowIfArgNullOrEmpty(nameof(propertyName));

            return query.CallOrderedQueryable(OrderInfoNamesHelper.ThenBy, propertyName, comparer, type);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Then by descending.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="query">.</param>
        /// <param name="propertyName">.</param>
        /// <param name="comparer">(Optional)</param>
        /// <param name="type">(Optional)</param>
        /// <returns>An IOrderedQueryable&lt;TSource&gt;</returns>
        ///=================================================================================================
        public static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IOrderedQueryable<TSource> query,
            string propertyName, IComparer<object> comparer = null, Type type = null) where TSource : class
        {
            query.ThrowIfNull("Current query can not be null!");
            propertyName.ThrowIfArgNullOrEmpty(nameof(propertyName));

            return query.CallOrderedQueryable(OrderInfoNamesHelper.ThenByDescending, propertyName, comparer, type);
        }
    }
}