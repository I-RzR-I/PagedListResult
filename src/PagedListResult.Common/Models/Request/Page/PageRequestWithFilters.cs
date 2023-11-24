// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-14 00:46
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 00:47
// ***********************************************************************
//  <copyright file="PageRequestWithFilters.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#endregion

namespace PagedListResult.Common.Models.Request.Page
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
            if (Filters.IsNotNull() && Filters.Any())
                foreach (var filter in Filters)
                {
                    var validationResult = filter.Validate(validationContext).ToList();
                    if (!validationResult.IsNullOrEmptyEnumerable())
                        yield return validationResult.First();
                }
        }
    }
}