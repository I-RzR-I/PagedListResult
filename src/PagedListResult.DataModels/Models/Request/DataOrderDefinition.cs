﻿// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common.DataModel
//  Author           : RzR
//  Created On       : 2024-12-22 13:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-22 19:46
// ***********************************************************************
//  <copyright file="DataOrderDefinition.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using PagedListResult.DataModels.Enums;
using System.ComponentModel;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

#endregion

namespace PagedListResult.DataModels.Models.Request
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>A data order definition.</summary>
    /// <remarks>RzR, 10-Nov-23.</remarks>
    /// =================================================================================================
    public class DataOrderDefinition
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Property name to order.</summary>
        /// <value>The order by property.</value>
        /// =================================================================================================
        public virtual string OrderByProperty { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Ordering direction, ordering will be made by selected property.</summary>
        /// <value>The order direction.</value>
        /// =================================================================================================
        public virtual OrderDirection OrderDirection { get; set; } = OrderDirection.Asc;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     If no order property is provided, enable to order by default property if exists.
        /// </summary>
        /// <value>True if order by default property, false if not.</value>
        /// =================================================================================================
        [DefaultValue(false)]
        public virtual bool OrderByDefaultProperty { get; set; } = false;
    }
}