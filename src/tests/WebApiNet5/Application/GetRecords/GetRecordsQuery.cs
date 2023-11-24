// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.WebApiNet5
//  Author           : RzR
//  Created On       : 2023-11-15 21:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-15 21:10
// ***********************************************************************
//  <copyright file="GetRecordsQuery.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using MediatR;
using PagedListResult.Common.Abstractions;
using PagedListResult.Common.Models.Request.Page;
using PagedListResultNet5Tests.Models;

namespace WebApiNet5.Application.GetRecords
{
    public class GetRecordsQuery : PageRequestWithFilters,
        IRequest<IPagedResult<PostDetail>>
    {

    }
}