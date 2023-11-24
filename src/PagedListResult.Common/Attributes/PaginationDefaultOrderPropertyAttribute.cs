// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-24 14:56
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 00:40
// ***********************************************************************
//  <copyright file="PaginationDefaultOrderPropertyAttribute.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;

#endregion

namespace PagedListResult.Common.Attributes
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Default pagination order property attribute.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// <seealso cref="Attribute" />
    /// =================================================================================================
    [AttributeUsage(AttributeTargets.Property)]
    public class PaginationDefaultOrderPropertyAttribute : Attribute
    {
    }
}