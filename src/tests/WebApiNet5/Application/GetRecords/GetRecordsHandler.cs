// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.WebApiNet5
//  Author           : RzR
//  Created On       : 2023-11-15 21:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-15 21:10
// ***********************************************************************
//  <copyright file="GetRecordsHandler.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using MediatR;
using Microsoft.EntityFrameworkCore;
using PagedListResult;
using PagedListResult.Common.Abstractions;
using PagedListResult.Common.Models.Request;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiNet5.Data;
using WebApiNet5.Models;

namespace WebApiNet5.Application.GetRecords
{
    public class GetRecordsHandler : IRequestHandler<GetRecordsQuery, IPagedResult<PostDetail>>
    {
        private readonly AppDbContext _db;

        public GetRecordsHandler(AppDbContext db) => _db = db;

        /// <inheritdoc />
        public async Task<IPagedResult<PostDetail>> Handle(GetRecordsQuery request, CancellationToken cancellationToken)
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

            return await data
                .GetPagedWithFiltersAsync(request, 
                    new DefaultPrimaryKeyDefinition("Id"), 
                    cancellationToken: cancellationToken);
        }
    }
}