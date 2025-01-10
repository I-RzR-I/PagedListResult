// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 13:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="PageRequestWithFilters.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using PagedListResult.DataModels.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable MemberCanBePrivate.Global

#endregion

namespace PagedListResult.DataModels.Models.Request.Page
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Filtered page request.</summary>
    /// <remarks>RzR, 13-Nov-23.</remarks>
    /// <seealso cref="PagedRequest" />
    /// <seealso cref="IValidatableObject" />
    /// =================================================================================================
    public class PageRequestWithFilters : PagedRequest, IValidatableObject
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Data filters.</summary>
        /// <value>The filters.</value>
        /// =================================================================================================
        public ICollection<DataFilter> Filters { get; set; } = new HashSet<DataFilter>();

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Filters.IsNullOrEmptyEnumerable())
                yield break;

            foreach (var filter in Filters)
            {
                var validationResult = filter.Validate(validationContext).ToList();
                if (!validationResult.IsNullOrEmptyEnumerable())
                    yield return validationResult.First();
            }
        }
    }
}