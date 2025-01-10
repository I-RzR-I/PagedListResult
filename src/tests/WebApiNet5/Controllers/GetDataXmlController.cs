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
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedListResult;
using PagedListResult.DataModels.Enums;
using PagedListResult.Extensions;
using PagedListResult.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebApiNet5.Application;
using WebApiNet5.Data;
using WebApiNet5.Models;

namespace WebApiNet5.Controllers
{
    [Produces("application/xml")]
    [Route("api/[controller]/[action]")]
    public class GetDataXmlController : BaseApiPagedResultController
    {
        private readonly AppDbContext _db;

        public GetDataXmlController(AppDbContext db) => _db = db;

        [HttpPost]
        [Produces("application/xml")]
        [ProducesResponseType(typeof(IEnumerable<MessageModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetXmlAllRecords(
            [FromBody] GetAllRecordsRequest query, CancellationToken cancellationToken)
        {
            var data = _db.Posts
                .Include(x => x.Author)
                .Select(x => new PostDetail
                {
                    AuthorId = x.AuthorId,
                    AuthorName = x.Author.Name,
                    Contents = x.Contents,
                    CreatedOn = x.CreatedOn,
                    Id = x.Id,
                    Title = x.Title,
                    ModifiedOn = x.ModifiedOn
                });

            var dataList = await data.GetPagedWithFiltersAsync(query, null, FilterConditionType.And, cancellationToken);

            var xml = dataList.ToSoapXmlPagedResult();

            return new ContentResult
            {
                Content = xml.SerializeToString(),
                ContentType = "text/xml",
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        [HttpPost]
        [Produces("application/xml")]
        [ProducesResponseType(typeof(IEnumerable<MessageModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetXmlExtAllRecords(
            [FromBody] GetAllRecordsRequest query, CancellationToken cancellationToken)
        {
            var data = _db.Posts
                .Include(x => x.Author)
                .Select(x => new PostDetail
                {
                    AuthorId = x.AuthorId,
                    AuthorName = x.Author.Name,
                    Contents = x.Contents,
                    CreatedOn = x.CreatedOn,
                    Id = x.Id,
                    Title = x.Title,
                    ModifiedOn = x.ModifiedOn
                });

            var dataList = await data.GetPagedWithFiltersAsync(query, null, FilterConditionType.And, cancellationToken);

            return XmlResult(dataList);
        }
    }
}