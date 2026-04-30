// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2023-11-15 18:53
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-03 16:57
// ***********************************************************************
//  <copyright file="BaseApiPagedResultController.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Mvc;
using RzR.Extensions.Domain.Primitives;
using RzR.ResultMessage.Pagination.Abstractions.Abstractions;
using RzR.ResultMessage.Pagination.EntityFrameworkCore.Extensions;
using RzR.ResultMessage.Web;
using RzR.ResultMessage.Web.Extensions.ProblemDetail;
using System;
using System.Net;

#endregion

namespace RzR.ResultMessage.Pagination.AspNetCore
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A controller for handling base API paged results.
    /// </summary>
    /// <remarks>
    ///     RzR, 15-Nov-23.
    /// </remarks>
    /// <seealso cref="ResultBaseApiController" />
    /// =================================================================================================
    public abstract class BaseApiPagedResultController : ResultBaseApiController
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Return api response on json format. Status code 200 with data if IsSuccess is true.
        ///     On failure returns an RFC 9457 <c>application/problem+json</c> response built by the
        ///     ambient <see cref="RzR.ResultMessage.Web.Abstractions.IProblemDetailsResultFactory"/>
        ///     (defaults to 400; customize via
        ///     <c>services.AddProblemDetailsResultFactory&lt;TFactory&gt;()</c> from
        ///     <c>AggregatedGenericResultMessage.Web</c>).
        /// </summary>
        /// <remarks>
        ///     RzR, 15-Nov-23.
        /// </remarks>
        /// <typeparam name="TType">.</typeparam>
        /// <param name="response">.</param>
        /// <returns>
        ///     A response to return to the caller.
        /// </returns>
        /// =================================================================================================
        protected virtual IActionResult PagedOkResult<TType>(IPagedResult<TType> response)
            where TType : class
        {
            if (response.IsSuccess.IsTrue())
                return Json(response);

            return response.AsProblemDetails(HttpStatusCode.BadRequest);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Obsolete. Use <see cref="PagedOkResult{TType}(IPagedResult{TType})"/> instead. Kept to
        ///     preserve binary/source compatibility for one release; collides with
        ///     <see cref="Microsoft.AspNetCore.Mvc.JsonResult"/>.
        /// </summary>
        /// <typeparam name="TType">.</typeparam>
        /// <param name="response">.</param>
        /// <returns>A response to return to the caller.</returns>
        /// =================================================================================================
        [Obsolete("Use PagedOk<TType> instead. JsonResult will be removed in the next major version because may cause collides with Microsoft.AspNetCore.Mvc.JsonResult.")]
        protected virtual IActionResult JsonResult<TType>(IPagedResult<TType> response)
            where TType : class
        {
            if (response.IsSuccess.IsTrue())
                return Json(response);

            return BadRequest(response.Messages);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     XML result.
        /// </summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <param name="response">.</param>
        /// <returns>
        ///     A response to return to the caller.
        /// </returns>
        /// =================================================================================================
        protected virtual IActionResult PagedXmlResult<TType>(IPagedResult<TType> response)
            where TType : class
        {
            if (response.IsSuccess.IsTrue())
            {
                var xml = response.ToSoapXmlPagedResult();

                return new ContentResult
                {
                    Content = ObjectExtensions.SerializeToString(xml), 
                    ContentType = "text/xml", 
                    StatusCode = (int)HttpStatusCode.OK
                };
            }

            return response.AsProblemDetails(HttpStatusCode.BadRequest);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Obsolete. Use <see cref="PagedXmlResult{TType}(IPagedResult{TType})"/> instead. Kept for one
        ///     release; will be removed in the next major version.
        /// </summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <param name="response">.</param>
        /// <returns>A response to return to the caller.</returns>
        /// =================================================================================================
        [Obsolete("Use PagedXmlResult<TType> instead. XmlResult will be removed in the next major version.")]
        protected virtual IActionResult XmlResult<TType>(IPagedResult<TType> response)
            where TType : class
        {

            if (response.IsSuccess.IsTrue())
            {
                var xml = response.ToSoapXmlPagedResult();

                return new ContentResult
                {
                    Content = ObjectExtensions.SerializeToString(xml),
                    ContentType = "text/xml",
                    StatusCode = (int)HttpStatusCode.OK
                };
            }

            return BadRequest(response.Messages);
        }
    }
}