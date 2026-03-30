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

using AggregatedGenericResultMessage.Models;
using Microsoft.AspNetCore.Mvc;
using PagedListResult;
using PagedListResult.DataModels.Enums;
using PagedListResult.DataModels.Models.Result;
using PagedListResult.Web;
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
        [ProducesResponseType(typeof(IEnumerable<MessageModel>), StatusCodes.Status400BadRequest)]
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

                return JsonResult(dataList);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499); // Client Closed Request (optional)
            }
        }
    }
}