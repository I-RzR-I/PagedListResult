// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultWebTests
//  Author           : RzR
//  Created On       : 2026-04-26 20:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 20:48
// ***********************************************************************
//  <copyright file="SampleController.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Mvc;
using RzR.ResultMessage.Pagination.Abstractions.Abstractions;
using RzR.ResultMessage.Pagination.Abstractions.Models.Result;
using System.Threading.Tasks;

#endregion

namespace PagedListResultWebTests.Stubs
{
    public sealed class SampleItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SampleController : ControllerBase
    {
        public PagedResult<SampleItem> ReturnsConcretePagedResult() => new();

        public IPagedResult<SampleItem> ReturnsInterfacePagedResult() => new PagedResult<SampleItem>();

        public Task<PagedResult<SampleItem>> ReturnsTaskPagedResult()
            => Task.FromResult(new PagedResult<SampleItem>());

        public Task<IPagedResult<SampleItem>> ReturnsTaskInterfacePagedResult()
            => Task.FromResult<IPagedResult<SampleItem>>(new PagedResult<SampleItem>());

        public ValueTask<PagedResult<SampleItem>> ReturnsValueTaskPagedResult()
            => new(new PagedResult<SampleItem>());

        public ActionResult<PagedResult<SampleItem>> ReturnsActionResultPagedResult()
            => new PagedResult<SampleItem>();

        public Task<ActionResult<PagedResult<SampleItem>>> ReturnsTaskActionResultPagedResult()
            => Task.FromResult<ActionResult<PagedResult<SampleItem>>>(new PagedResult<SampleItem>());

        public IActionResult ReturnsIActionResult() => new OkResult();

        public string ReturnsString() => string.Empty;

        public void ReturnsVoid() { }

        [ProducesResponseType(typeof(PagedResult<SampleItem>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public PagedResult<SampleItem> AlreadyAnnotatedPagedResult() => new();

        [Produces("application/xml")]
        public PagedResult<SampleItem> ProducesXmlPagedResult() => new();
    }

    [Produces("application/xml")]
    public class SampleControllerWithProducesAtControllerLevel : ControllerBase
    {
        public PagedResult<SampleItem> ReturnsPagedResult() => new();
    }
}