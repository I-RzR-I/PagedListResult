// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 13:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="PagedRequest.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#endregion

namespace PagedListResult.DataModels.Models.Request.Page
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Page request settings.</summary>
    /// <remarks>RzR, 10-Nov-23.</remarks>
    /// =================================================================================================
    public class PagedRequest
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Page number.</summary>
        /// <value>The page.</value>
        /// =================================================================================================
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        [DefaultValue(1)]
        public virtual int Page { get; set; } = 1;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Number of items per page.</summary>
        /// <value>The size of the page.</value>
        /// =================================================================================================
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        [DefaultValue(10)]
        public virtual int PageSize { get; set; } = 10;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the data search settings.</summary>
        /// <value>The search.</value>
        /// =================================================================================================
        public virtual DataSearchDefinition Search { get; set; } = new DataSearchDefinition();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the data/result order settings.</summary>
        /// <value>The order.</value>
        /// =================================================================================================
        public virtual DataOrderDefinition Order { get; set; } = new DataOrderDefinition();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Property used for select/return in UI provided column (in special on execution SP).
        /// </summary>
        /// <value>The fields.</value>
        /// =================================================================================================
        public virtual ICollection<string> Fields { get; set; } = new HashSet<string>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>List of record/s id/s witch will be injected in top of grid result.</summary>
        /// <value>The predefined records.</value>
        /// =================================================================================================
        public virtual DataPredefinedFilterDefinition PredefinedRecord { get; set; } = new DataPredefinedFilterDefinition();
    }
}