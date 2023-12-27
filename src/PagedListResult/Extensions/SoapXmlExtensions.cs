// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2023-12-26 23:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-27 00:05
// ***********************************************************************
//  <copyright file="SoapXmlExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage.Models;
using DomainCommonExtensions.CommonExtensions;
using PagedListResult.Common.Models.Result;
using PagedListResult.Models;
using System.Linq;

#endregion

namespace PagedListResult.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>A SOAP/XML extensions.</summary>
    /// <remarks></remarks>
    /// =================================================================================================
    public static class SoapXmlExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A PagedResult&lt;TSource&gt; extension method that converts a source to a SOAP/XML
        ///     result.
        /// </summary>
        /// <remarks></remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <returns>Source as a SoapXmlPagedResult.</returns>
        /// =================================================================================================
        public static SoapXmlPagedResult ToSoapXmlPagedResult<TSource>(this PagedResult<TSource> source) where TSource : class
            => new SoapXmlPagedResult()
            {
                CurrentPage = source.CurrentPage,
                IsSuccess = source.IsSuccess,
                PageCount = source.PageCount,
                PageSize = source.PageSize,
                RowCount = source.RowCount,
                Messages = source.Messages.Select(x => new MessageModel
                {
                    Key = x.Key,
                    Message = x.Message,
                    MessageType = x.MessageType
                }).ToList(),
                Response = source.Response.IsNotNull() ? source.Response.CastToSoapXmlResponse() : null,
                ExecutionDetails = new SoapXmlPagedExecDetailsResult()
                {
                    ExecutionDate = source.ExecutionDetails.ExecutionDate,
                    ExecutionTimeMs = source.ExecutionDetails.ExecutionTimeMs
                }
            };
    }
}