// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2025-02-23 21:25
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-02-23 23:11
// ***********************************************************************
//  <copyright file="PredefinedRecordsHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.DataModels.Models.Request;
using PagedListResult.Extensions;
using PagedListResult.Models.Internal;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace PagedListResult.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A predefined records helper.
    /// </summary>
    /// =================================================================================================
    internal static class PredefinedRecordsHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds predefined filter.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="query">The query to act on.</param>
        /// <param name="defaultPrimaryKey">(Optional) The default primary key.</param>
        /// <param name="preSelectedFilter">(Optional) A filter specifying the predefined.</param>
        /// <returns>
        ///     A BuildPredefinedFilterDto.
        /// </returns>
        /// =================================================================================================
        internal static BuildPredefinedFilterDto BuildPredefinedFilter<TSource>(
            IQueryable<TSource> query,
            DefaultPrimaryKeyDefinition defaultPrimaryKey = null,
            DataPredefinedFilterDefinition preSelectedFilter = null) where TSource : class
        {
            var keyFieldNames = GetDefaultPrimaryKeyProp(query, defaultPrimaryKey, preSelectedFilter?.PredefinedFieldName);
            var hasIds = preSelectedFilter!.PredefinedRecords.IsNotNull()
                         && preSelectedFilter.PredefinedRecords.Any()
                         && preSelectedFilter.PredefinedRecords.All(x => !string.IsNullOrEmpty(x));
            var idCount = hasIds.Equals(true) ? preSelectedFilter.PredefinedRecords?.Count ?? 0 : 0;

            return new BuildPredefinedFilterDto { HasIds = hasIds, IdsCount = idCount, PredefinedFieldNames = keyFieldNames, PredefinedFieldIds = preSelectedFilter.PredefinedRecords };
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;TSource&gt; extension method that gets default primary key property.
        /// </summary>
        /// <remarks>
        ///     RzR, 10-Nov-23.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="query">The query to act on.</param>
        /// <param name="defaultPrimaryKey">(Optional) The default primary key.</param>
        /// <param name="preSelectedFieldName">(Optional) The preselected request filter.</param>
        /// <returns>
        ///     The default primary key property.
        /// </returns>
        /// =================================================================================================
        private static ICollection<string> GetDefaultPrimaryKeyProp<TSource>(
            IQueryable<TSource> query,
            DefaultPrimaryKeyDefinition defaultPrimaryKey = null,
            string preSelectedFieldName = null) where TSource : class
        {
            if (preSelectedFieldName.IsPresent())
                return new[] { preSelectedFieldName };

            if (defaultPrimaryKey.IsNull())
                return null;

            if (defaultPrimaryKey!.DefaultPrimaryKey.IsPresent())
                return new[] { defaultPrimaryKey.DefaultPrimaryKey };

            if (defaultPrimaryKey.FindByEntity.IsTrue())
                return query.GetPrimaryKeysNameList();

            if (defaultPrimaryKey.FindByAttribute.IsTrue())
                return null;

            return null;
        }
    }
}