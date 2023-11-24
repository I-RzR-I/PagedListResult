// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-26 08:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 09:20
// ***********************************************************************
//  <copyright file="ExpressionFuncBuilder.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using AggregatedGenericResultMessage.Extensions.Result;
using DomainCommonExtensions.CommonExtensions;
using System;
using System.Linq.Expressions;

#endregion

namespace PagedListResult.Common.Helpers.Internal.Builder
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Expression function builder.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    internal static class ExpressionFuncBuilder
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Build expression function.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <param name="objectData">Object data.</param>
        /// <returns>An IResult&lt;Expression&lt;Func&lt;T&gt;&gt;&gt;</returns>
        /// =================================================================================================
        internal static IResult<Expression<Func<TSource>>> BuildExpFunc<TSource>(object objectData) where TSource : struct
        {
            try
            {
                return Result<Expression<Func<TSource>>>
                    .Success(typeof(TSource).IsNullablePropType()
                        ? () => ((TSource?)objectData).Value
                        : () => (TSource)objectData);
            }
            catch (Exception e)
            {
                return Result<Expression<Func<TSource>>>.Failure().WithError(e);
            }
        }
    }
}