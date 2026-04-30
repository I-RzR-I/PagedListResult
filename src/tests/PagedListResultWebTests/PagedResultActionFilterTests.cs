// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultWebTests
//  Author           : RzR
//  Created On       : 2026-04-26 20:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 20:49
// ***********************************************************************
//  <copyright file="PagedListResultMvcBuilderExtensionsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResultWebTests.Stubs;
using RzR.ResultMessage.Pagination.Abstractions.Models.Result;
using RzR.ResultMessage.Pagination.AspNetCore.Filters;
using RzR.ResultMessage.Pagination.AspNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class PagedResultActionFilterTests
    {
        private static PagedResultActionFilter Filter(PagedListResultWebOptions opts = null)
            => new(Options.Create(opts ?? new PagedListResultWebOptions()));

        private static ResultExecutingContext Context(
            IActionResult result,
            string queryString = "",
            string path = "/items")
        {
            var http = new DefaultHttpContext();
            http.Request.Scheme = "https";
            http.Request.Host = new HostString("api.example.com");
            http.Request.Path = path;
            http.Request.QueryString = new QueryString(queryString);
            http.Request.Method = "GET";

            var actionContext = new ActionContext(http, new RouteData(), new ActionDescriptor());

            return new ResultExecutingContext(actionContext, new List<IFilterMetadata>(), result, controller: null);
        }

        private static Task NoopNext(out ResultExecutedContext captured, ResultExecutingContext ctx)
        {
            var local = new ResultExecutedContext(ctx, ctx.Filters, ctx.Result, controller: null);
            captured = local;

            return Task.FromResult(local);
        }

        private static PagedResult<SampleItem> SamplePaged(
            int currentPage = 2, int pageCount = 5, int pageSize = 10, int rowCount = 47, long execMs = 42)
        {
            var p = new PagedResult<SampleItem>
            {
                IsSuccess = true,
                Response = new List<SampleItem> { new() { Id = 1, Name = "x" } },
                CurrentPage = currentPage,
                PageCount = pageCount,
                PageSize = pageSize,
                RowCount = rowCount
            };
            p.ExecutionDetails.SetExecutionTimeMs(execMs, DateTime.UtcNow);

            return p;
        }

        [TestMethod]
        public async Task NonObjectResult_DoesNotEmitHeaders()
        {
            var ctx = Context(new OkResult());
            await Filter().OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("X-Total-Count"));
            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("Link"));
        }

        [TestMethod]
        public async Task NonPagedObjectResult_DoesNotEmitHeaders()
        {
            var ctx = Context(new ObjectResult(new SampleItem()));
            await Filter().OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("X-Total-Count"));
        }

        [TestMethod]
        public async Task NullObjectResultValue_DoesNotThrow()
        {
            var ctx = Context(new ObjectResult(null));
            await Filter().OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));
        }

        [TestMethod]
        public async Task PagedResult_EmitsTotalCountHeaders_ByDefault()
        {
            var ctx = Context(new ObjectResult(SamplePaged()));
            await Filter().OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            var h = ctx.HttpContext.Response.Headers;
            Assert.AreEqual("47", h["X-Total-Count"].ToString());
            Assert.AreEqual("5", h["X-Page-Count"].ToString());
            Assert.AreEqual("10", h["X-Page-Size"].ToString());
            Assert.AreEqual("2", h["X-Current-Page"].ToString());
        }

        [TestMethod]
        public async Task PagedResult_TotalCountHeader_DisabledByOption()
        {
            var ctx = Context(new ObjectResult(SamplePaged()));
            await Filter(new PagedListResultWebOptions { EmitTotalCountHeader = false })
                .OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("X-Total-Count"));
        }

        [TestMethod]
        public async Task PagedResult_EmitsLinkHeader_WithFirstPrevNextLast()
        {
            var ctx = Context(new ObjectResult(SamplePaged(currentPage: 3, pageCount: 5, pageSize: 10)));
            await Filter().OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            var link = ctx.HttpContext.Response.Headers["Link"].ToString();
            StringAssert.Contains(link, "rel=\"first\"");
            StringAssert.Contains(link, "rel=\"prev\"");
            StringAssert.Contains(link, "rel=\"next\"");
            StringAssert.Contains(link, "rel=\"last\"");
            StringAssert.Contains(link, "page=1");
            StringAssert.Contains(link, "page=2");
            StringAssert.Contains(link, "page=4");
            StringAssert.Contains(link, "page=5");
            StringAssert.Contains(link, "pageSize=10");
            StringAssert.Contains(link, "https://api.example.com/items");
        }

        [TestMethod]
        public async Task LinkHeader_FirstPage_OmitsPrev()
        {
            var ctx = Context(new ObjectResult(SamplePaged(currentPage: 1, pageCount: 5)));
            await Filter().OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            var link = ctx.HttpContext.Response.Headers["Link"].ToString();
            Assert.IsFalse(link.Contains("rel=\"prev\""));
            StringAssert.Contains(link, "rel=\"next\"");
        }

        [TestMethod]
        public async Task LinkHeader_LastPage_OmitsNext()
        {
            var ctx = Context(new ObjectResult(SamplePaged(currentPage: 5, pageCount: 5)));
            await Filter().OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            var link = ctx.HttpContext.Response.Headers["Link"].ToString();
            Assert.IsFalse(link.Contains("rel=\"next\""));
            StringAssert.Contains(link, "rel=\"prev\"");
        }

        [TestMethod]
        public async Task LinkHeader_PreservesOtherQueryParameters()
        {
            var ctx = Context(
                new ObjectResult(SamplePaged()),
                queryString: "?filter=status:eq:active&order=name:asc&page=99&pageSize=99");

            await Filter().OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            var link = ctx.HttpContext.Response.Headers["Link"].ToString();
            StringAssert.Contains(link, "filter=status%3Aeq%3Aactive");
            StringAssert.Contains(link, "order=name%3Aasc");

            // The user's page=99/pageSize=99 must NOT appear: convention overrides them.
            Assert.IsFalse(link.Contains("page=99"));
            Assert.IsFalse(link.Contains("pageSize=99"));
        }

        [TestMethod]
        public async Task LinkHeader_DisabledByOption()
        {
            var ctx = Context(new ObjectResult(SamplePaged()));
            await Filter(new PagedListResultWebOptions { EmitLinkHeader = false })
                .OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("Link"));
        }

        [TestMethod]
        public async Task LinkHeader_NotEmitted_WhenPageCountZero()
        {
            var ctx = Context(new ObjectResult(SamplePaged(currentPage: 0, pageCount: 0, rowCount: 0)));
            await Filter().OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("Link"));
        }

        [TestMethod]
        public async Task ServerTiming_NotEmitted_ByDefault()
        {
            var ctx = Context(new ObjectResult(SamplePaged()));
            await Filter().OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("Server-Timing"));
        }

        [TestMethod]
        public async Task ServerTiming_Emitted_WhenEnabled()
        {
            var ctx = Context(new ObjectResult(SamplePaged(execMs: 123)));
            await Filter(new PagedListResultWebOptions { EmitServerTimingHeader = true })
                .OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            var st = ctx.HttpContext.Response.Headers["Server-Timing"].ToString();
            Assert.AreEqual("paged;dur=123", st);
        }

        [TestMethod]
        public async Task Filter_CallsNext()
        {
            var ctx = Context(new ObjectResult(SamplePaged()));
            var nextCalled = false;

            await Filter().OnResultExecutionAsync(ctx, () =>
            {
                nextCalled = true;
                return Task.FromResult<ResultExecutedContext>(null);
            });

            Assert.IsTrue(nextCalled);
        }

        [TestMethod]
        public void Ctor_NullOptions_Throws()
            => Assert.ThrowsException<ArgumentNullException>(() => new PagedResultActionFilter(null));
    }
}
