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
using System.Net;

#endregion

namespace RzR.ResultMessage.Pagination.AspNetCore
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A controller for handling base API paged results.
    /// </summary>
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
        ///     <c>RzR.ResultMessage.Web</c>).
        /// </summary>
        /// <typeparam name="TType">.</typeparam>
        /// <param name="response">.</param>
        /// <returns>
        ///     A response to return to the caller. On <c>netstandard2.1</c> this is an <c>OkObjectResult</c>
        ///     with <c>ContentTypes</c> pinned to <c>application/json</c>; on net5.0+
        ///     it is a <c>JsonResult</c>. Both carry the whole paged envelope.
        ///     A <see langword="null" /> <paramref name="response" /> returns <c>204 No Content</c>.
        /// </returns>
        /// =================================================================================================
        protected virtual IActionResult PagedOkResult<TType>(IPagedResult<TType> response)
            where TType : class
        {
            if (response.IsNull())
                return NoContent();

            if (response.IsSuccess.IsTrue())
            {
#if NETSTANDARD2_1
                return JsonWholeResult<System.Collections.Generic.IList<TType>>(response);
#else
                return new Microsoft.AspNetCore.Mvc.JsonResult(response);
#endif
            }

            return response.AsProblemDetails(HttpStatusCode.BadRequest);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     XML result.
        /// </summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <param name="response">.</param>
        /// <returns>
        ///     A response to return to the caller.
        ///     A <see langword="null" /> <paramref name="response" /> returns <c>204 No Content</c>.
        /// </returns>
        /// =================================================================================================
        protected virtual IActionResult PagedXmlResult<TType>(IPagedResult<TType> response)
            where TType : class
        {
            if (response.IsNull())
                return NoContent();

            if (response.IsSuccess.IsTrue())
            {
                var xml = response.ToSoapXmlPagedResult();

                return new ContentResult
                {
                    Content = xml.SerializeToString(), 
                    ContentType = "text/xml", 
                    StatusCode = (int)HttpStatusCode.OK
                };
            }

            return response.AsProblemDetails(HttpStatusCode.BadRequest);
        }
    }
}