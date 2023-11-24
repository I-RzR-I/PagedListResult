// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2023-11-15 18:53
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-15 19:43
// ***********************************************************************
//  <copyright file="BaseApiPagedResultController.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage.Web;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.AspNetCore.Mvc;
using PagedListResult.Common.Abstractions;
using PagedListResult.Common.Models.Result;

#endregion

namespace PagedListResult.Web
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>A controller for handling base API paged results.</summary>
    /// <remarks>RzR, 15-Nov-23.</remarks>
    /// <seealso cref="AggregatedGenericResultMessage.Web.ResultBaseApiController" />
    /// =================================================================================================
    public abstract class BaseApiPagedResultController : ResultBaseApiController
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Return api response on json format. Status code 200 with data if IsSuccess is true.
        ///     Status code 400 with errors collection if IsSuccess is false.
        /// </summary>
        /// <remarks>RzR, 15-Nov-23.</remarks>
        /// <typeparam name="TType">.</typeparam>
        /// <param name="response">.</param>
        /// <returns>A response to return to the caller.</returns>
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
        /// <remarks>RzR, 15-Nov-23.</remarks>
        /// <typeparam name="TType">.</typeparam>
        /// <param name="response">.</param>
        /// <returns>A response to return to the caller.</returns>
        /// =================================================================================================
        protected virtual IActionResult JsonResult<TType>(PagedResult<TType> response)
            where TType : class
        {
            if (response.IsSuccess.IsTrue())
                return Json(response);

            return BadRequest(response.Messages);
        }
    }
}