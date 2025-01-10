// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 13:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="DataFilter.cs" company="RzR SOFT & TECH">
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

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

#endregion

namespace PagedListResult.DataModels.Models.Request
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
                if (validationResult.IsNullOrEmptyEnumerable().IsFalse()) yield return validationResult.First();
            }

            if (FilterApplyOrder.IsNull())
                yield return new ValidationResult(
                    $"On filter data, {nameof(FilterApplyOrder)} must be not null", new[] { nameof(FilterApplyOrder) });
        }
    }
}