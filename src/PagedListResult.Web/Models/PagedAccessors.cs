// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 08:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 08:08
// ***********************************************************************
//  <copyright file="PagedAccessors.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.Domain.Primitives;
using RzR.ResultMessage.Pagination.Abstractions.Abstractions;
using System;
using System.Reflection;

#endregion

namespace RzR.ResultMessage.Pagination.AspNetCore.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A paged accessors. This class cannot be inherited.
    /// </summary>
    /// =================================================================================================
    internal sealed class PagedAccessors
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the get current page.
        /// </summary>
        /// <value>
        ///     A function delegate that yields an int.
        /// </value>
        /// =================================================================================================
        public Func<object, int> GetCurrentPage { get; private set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the number of get pages.
        /// </summary>
        /// <value>
        ///     A function delegate that yields an int.
        /// </value>
        /// =================================================================================================
        public Func<object, int> GetPageCount { get; private set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the size of the get page.
        /// </summary>
        /// <value>
        ///     A function delegate that yields an int.
        /// </value>
        /// =================================================================================================
        public Func<object, int> GetPageSize { get; private set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the number of get rows.
        /// </summary>
        /// <value>
        ///     A function delegate that yields an int.
        /// </value>
        /// =================================================================================================
        public Func<object, int> GetRowCount { get; private set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the get execution time milliseconds.
        /// </summary>
        /// <value>
        ///     A function delegate that yields a long.
        /// </value>
        /// =================================================================================================
        public Func<object, long> GetExecutionTimeMs { get; private set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies an operation to all items in this collection.
        /// </summary>
        /// <param name="pagedType">Type of the paged.</param>
        /// <returns>
        ///     The PagedAccessors.
        /// </returns>
        /// =================================================================================================
        public static PagedAccessors For(Type pagedType)
        {
            var a = new PagedAccessors
            {
                GetCurrentPage = MakeIntGetter(pagedType, nameof(IPagedResult<object>.CurrentPage)),
                GetPageCount = MakeIntGetter(pagedType, nameof(IPagedResult<object>.PageCount)),
                GetPageSize = MakeIntGetter(pagedType, nameof(IPagedResult<object>.PageSize)),
                GetRowCount = MakeIntGetter(pagedType, nameof(IPagedResult<object>.RowCount))
            };

            var execProp = pagedType.GetProperty("ExecutionDetails", BindingFlags.Public | BindingFlags.Instance);
            var msProp = execProp?.PropertyType.GetProperty("ExecutionTimeMs", BindingFlags.Public | BindingFlags.Instance);
            if (execProp.IsNotNull() && msProp.IsNotNull() && msProp!.PropertyType == typeof(long))
            {
                a.GetExecutionTimeMs = obj =>
                {
                    var details = execProp!.GetValue(obj);

                    return details.IsNull() ? -1L : (long)msProp.GetValue(details)!;
                };
            }

            return a;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Makes int getter.
        /// </summary>
        /// <param name="pagedType">Type of the paged.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///     A function delegate that yields an int.
        /// </returns>
        /// =================================================================================================
        private static Func<object, int> MakeIntGetter(Type pagedType, string propertyName)
        {
            var prop = pagedType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop.IsNull() || prop!.PropertyType != typeof(int))
                return _ => 0;

            return obj => (int)prop.GetValue(obj)!;
        }
    }
}