// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-05 23:40
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 02:07
// ***********************************************************************
//  <copyright file="ParameterReplacer.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Linq.Expressions;

#endregion

namespace PagedListResult.Common.Helpers.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>A parameter replacer.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// <seealso cref="System.Linq.Expressions.ExpressionVisitor" />
    /// =================================================================================================
    internal class ParameterReplacer : ExpressionVisitor
    {
        /// <summary>(Immutable) the parameter.</summary>
        private readonly ParameterExpression _parameter;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="parameter">The parameter.</param>
        /// =================================================================================================
        internal ParameterReplacer(ParameterExpression parameter)
            => _parameter = parameter;

        /// <inheritdoc />
        protected override Expression VisitParameter(ParameterExpression node)
            => base.VisitParameter(_parameter);
    }
}