// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 13:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="DataFilterValue.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using PagedListResult.DataModels.Enums;
using PagedListResult.DataModels.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

#endregion

namespace PagedListResult.DataModels.Models.Request
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Data filter value.</summary>
    /// <remarks>RzR, 13-Nov-23.</remarks>
    /// <seealso cref="System.ComponentModel.DataAnnotations.IValidatableObject" />
    /// =================================================================================================
    public class DataFilterValue : IValidatableObject
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the condition.</summary>
        /// <value>The condition. Default value: Equals</value>
        /// =================================================================================================
        [Required]
        public virtual FilterType Condition { get; set; } = FilterType.Equals;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets property name for filter.</summary>
        /// <value>The name of the property.</value>
        /// =================================================================================================
        [Required]
        public virtual string PropertyName { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets property filter value.</summary>
        /// <value>The values.</value>
        /// =================================================================================================
        [Required]
        public virtual ICollection<string> Values { get; set; } = new HashSet<string>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Optionally. Used in compare conditions with 2 values.</summary>
        /// <value>The compare value.</value>
        /// =================================================================================================
        public virtual string CompareValue { get; set; }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PropertyName.IsNullOrEmpty())
                yield return new ValidationResult(
                    $"On filter data, {nameof(PropertyName)} must be not null", new[] { nameof(PropertyName) });

            if (Condition.IsNull())
                yield return new ValidationResult(
                    $"On filter data, {nameof(Condition)} must be not null", new[] { nameof(Condition) });

            if (Condition == FilterType.Between && CompareValue.IsNull())
                yield return new ValidationResult(
                    $"On filter type: {Condition}, {nameof(CompareValue)} must be not null", new[] { nameof(CompareValue) });

            if (new List<FilterType> { FilterType.IsNull, FilterType.IsNotNull }.Contains(Condition).IsFalse() && Values.IsNullOrEmptyEnumerable())
                yield return new ValidationResult("Filter value must have a value to initiate filter!");

            if (Values.Count > 1 && new List<FilterType> { FilterType.IsIn, FilterType.IsNotIn }.Contains(Condition).IsFalse())
                yield return new ValidationResult(
                    $"Multiple values {nameof(Values)} are allowed only for filter: {FilterType.IsIn} or {FilterType.IsNotIn}",
                    new[] { nameof(Values) });
        }
    }
}