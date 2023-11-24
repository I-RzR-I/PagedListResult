// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-26 09:00
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 00:44
// ***********************************************************************
//  <copyright file="QueryableParamPropDto.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Linq.Expressions;

#endregion

namespace PagedListResult.Common.Models.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Queryable parameter property DTO.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    internal class QueryableParamPropDto
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Property name.</summary>
        /// <value>The name of the parameter.</value>
        /// =================================================================================================
        internal string ParamName { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Query type.</summary>
        /// <value>The type of the query.</value>
        /// =================================================================================================
        internal Type QueryType { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Parameter expression.</summary>
        /// <value>The parameter expression.</value>
        /// =================================================================================================
        internal ParameterExpression ParameterExpression { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Property expression.</summary>
        /// <value>The property expression.</value>
        /// =================================================================================================
        internal MemberExpression PropertyExpression { get; set; }
    }
}