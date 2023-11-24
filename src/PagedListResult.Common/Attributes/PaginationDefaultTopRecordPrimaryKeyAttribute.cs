// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 21:31
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 00:40
// ***********************************************************************
//  <copyright file="PaginationDefaultTopRecordPrimaryKeyAttribute.cs" company="">
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
    /// <summary>Default pagination primary key for top predefined records show.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// <seealso cref="Attribute" />
    /// =================================================================================================
    [AttributeUsage(AttributeTargets.Property)]
    public class PaginationDefaultTopRecordPrimaryKeyAttribute : Attribute
    {
    }
}