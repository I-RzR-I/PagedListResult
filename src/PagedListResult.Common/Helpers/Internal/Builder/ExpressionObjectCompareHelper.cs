// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-30 20:31
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 09:17
// ***********************************************************************
//  <copyright file="ExpressionObjectCompareHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Reflection;
using RzR.Extensions.Domain.Validation;
using RzR.ResultMessage.Abstractions;
using RzR.ResultMessage.Extensions.Result;
using System;
using System.Linq.Expressions;

#endregion

namespace RzR.ResultMessage.Pagination.Core.Helpers.Internal.Builder
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Expression object comparer helper.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    internal static class ExpressionObjectCompareHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Generate object comparer.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="property">Current property.</param>
        /// <param name="value">Property value.</param>
        /// <returns>The object compare.</returns>
        /// =================================================================================================
        internal static IResult<object> GenerateObjectCompare(MemberExpression property, object value)
        {
            property.ThrowIfArgNull(nameof(property));

            try
            {
                object compareObj = null;
                if (property.Type.IsNullablePropType())
                {
                    if (property.IsNotNull())
                        compareObj = Convert.ChangeType(value, property.Type.GetNonNullableType());
                }
                else
                {
                    compareObj = Convert.ChangeType(value, property.Type);
                }

                return Result<object>.Success(compareObj);
            }
            catch (Exception e)
            {
                return Result<object>.Failure().WithError(e);
            }
        }
    }
}