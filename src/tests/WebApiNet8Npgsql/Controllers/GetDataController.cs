// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.WebApiNet5
//  Author           : RzR
//  Created On       : 2023-11-15 01:56
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-15 01:56
// ***********************************************************************
//  <copyright file="GetDataController.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.AspNetCore.Mvc;
using RzR.ResultMessage.Pagination;
using RzR.ResultMessage.Pagination.Abstractions.Enums;
using RzR.ResultMessage.Pagination.Abstractions.Models.Result;
using RzR.ResultMessage.Pagination.AspNetCore;
using RzR.ResultMessage.Pagination.EntityFrameworkCore;
using RzR.ResultMessage.Web.Models;
using WebApiNet8Npgsql.Data;
using WebApiNet8Npgsql.Data.Models;
using WebApiNet8Npgsql.Models;
using WebApiNet8Npgsql.Operations;

namespace WebApiNet8Npgsql.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class GetDataController : BaseApiPagedResultController
    {
        private readonly AppDbContext _db;

        public GetDataController(AppDbContext db) => _db = db;

        [HttpPost]
        [ProducesResponseType(typeof(PagedResult<PagedDocumentResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultMessageProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllRecords(
            [FromBody] PagedDocumentQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = _db.Set<DocumentDataModel>()
                    .Join(_db.Set<UserDataModel>(),
                        doc => doc.UserId,
                        user => user.Id,
                        (doc, user) => new PagedDocumentResult()
                        {
                            Id = doc.Id,
                            Title = doc.Title,
                            AuthorName = user.UserName,
                            AuthorEmail = user.Email,
                            CreatedAt = doc.CreatedAt
                        });

                var dataList = await result.GetPagedWithFiltersAsync(query, null, FilterConditionType.And, cancellationToken);

                return PagedOkResult(dataList);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499); // Client Closed Request (optional)
            }
        }
    }
}