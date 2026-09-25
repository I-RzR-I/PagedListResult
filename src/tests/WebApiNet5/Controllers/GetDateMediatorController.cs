// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.WebApiNet5
//  Author           : RzR
//  Created On       : 2023-11-15 21:08
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-15 21:08
// ***********************************************************************
//  <copyright file="GetDateMediatorController.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RzR.ResultMessage.Pagination.Abstractions.Models.Result;
using RzR.ResultMessage.Pagination.AspNetCore;
using RzR.ResultMessage.Web.Models;
using System.Threading.Tasks;
using System.Threading;
using WebApiNet5.Application.GetRecords;
using WebApiNet5.Models;

namespace WebApiNet5.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class GetDateMediatorController : BaseApiPagedResultController
    {
        private readonly IMediator _mediator;

        public GetDateMediatorController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(PagedResult<PostDetail>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultMessageProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRecords(
            [FromBody] GetRecordsQuery query,
            CancellationToken cancellationToken)
        {
            var queryResponse = await _mediator.Send(query, cancellationToken);

            return PagedOkResult(queryResponse);
        }
    }
}