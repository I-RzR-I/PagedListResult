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
using RzR.ResultMessage.Pagination.DataModels.Abstractions;
using RzR.ResultMessage.Pagination.DataModels.Models.Result;
using RzR.ResultMessage.Pagination.Extensions;
using RzR.ResultMessage.Web;
using System.Net;

#endregion

namespace RzR.ResultMessage.Pagination.Web
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
        ///     Status code 400 with errors collection if IsSuccess is false.
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
        protected virtual IActionResult JsonResult<TType>(IPagedResult<TType> response)
            where TType : class
        {
            if (response.IsSuccess.IsTrue())
                return Json(response);

            return BadRequest(response.Messages);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Return api response on json format. Status code 200 with data if IsSuccess is true.
        ///     Status code 400 with errors collection if IsSuccess is false.
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
        protected virtual IActionResult JsonResult<TType>(PagedResult<TType> response)
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
        protected virtual IActionResult XmlResult<TType>(IPagedResult<TType> response)
            where TType : class
        {
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
        protected virtual IActionResult XmlResult<TType>(PagedResult<TType> response)
            where TType : class
        {
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

            return BadRequest(response.Messages);
        }
    }
}