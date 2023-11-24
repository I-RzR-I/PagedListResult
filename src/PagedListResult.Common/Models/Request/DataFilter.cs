// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-24 14:56
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-13 16:32
// ***********************************************************************
//  <copyright file="DataFilter.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions;
using PagedListResult.Common.Extensions.Internal.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

#endregion

namespace PagedListResult.Common.Models.Request
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Data filter.</summary>
    /// <remarks>RzR, 13-Nov-23.</remarks>
    /// <seealso cref="System.ComponentModel.DataAnnotations.IValidatableObject" />
    /// =================================================================================================
    public class DataFilter : IValidatableObject
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Filter value.</summary>
        /// <value>The filter value.</value>
        /// =================================================================================================
        [Required]
        public DataFilterValue FilterValue { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Filter apply order.</summary>
        /// <value>The filter apply order.</value>
        /// =================================================================================================
        [Required]
        public virtual int FilterApplyOrder { get; set; } = 0;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Data filter dependencies.</summary>
        /// <value>The dependencies.</value>
        /// =================================================================================================
        public virtual ICollection<DataFilterDependence> Dependencies { get; set; } = new HashSet<DataFilterDependence>();

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FilterValue.IsNullOrDefault())
            {
                yield return new ValidationResult(
                    $"On filter data, {nameof(FilterValue)} must be not null", new[] { nameof(FilterValue) });
            }
            else
            {
                var validationResult = FilterValue.Validate(validationContext).ToList();
                if (!validationResult.IsNullOrEmptyEnumerable()) yield return validationResult.First();
            }

            if (FilterApplyOrder.IsNull())
                yield return new ValidationResult(
                    $"On filter data, {nameof(FilterApplyOrder)} must be not null", new[] { nameof(FilterApplyOrder) });
        }
    }
}