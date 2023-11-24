// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-10 21:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-13 16:30
// ***********************************************************************
//  <copyright file="DataSearchDefinition.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

#endregion

namespace PagedListResult.Common.Models.Request
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>A data search definition.</summary>
    /// <remarks>RzR, 10-Nov-23.</remarks>
    /// =================================================================================================
    public class DataSearchDefinition
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Text to search.</summary>
        /// <value>The search.</value>
        /// =================================================================================================
        public virtual string Search { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     If Search contains a value and SearchInAllTextFields is specified with the value true
        ///     then the search will be done in all text properties, otherwise it will be done in the
        ///     specified properties.
        /// </summary>
        /// <value>True if search in all text fields, false if not.</value>
        /// =================================================================================================
        [DefaultValue(true)]
        public virtual bool SearchInAllTextFields { get; set; } = true;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     If SearchInAllTextFields is set to false, then search will be done on your custom fields.
        /// </summary>
        /// <value>The custom search text properties.</value>
        /// =================================================================================================
        public virtual ICollection<string> CustomSearchTextProperties { get; set; } = new HashSet<string>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Force set custom search fields.</summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <param name="customSearchFields">Custom fields used in search.</param>
        /// =================================================================================================
        public void SetCustomSearchProperties(IEnumerable<string> customSearchFields)
        {
            SearchInAllTextFields = false;
            CustomSearchTextProperties = customSearchFields.ToList();
        }
    }
}