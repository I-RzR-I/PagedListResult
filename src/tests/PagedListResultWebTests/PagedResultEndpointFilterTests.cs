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
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResultWebTests.Stubs;
using RzR.ResultMessage.Pagination.Abstractions.Models.Result;
using RzR.ResultMessage.Pagination.AspNetCore.Filters;
using RzR.ResultMessage.Pagination.AspNetCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class PagedResultEndpointFilterTests
    {
        private static PagedResultEndpointFilter Filter(PagedListResultWebOptions opts = null)
            => new(Options.Create(opts ?? new PagedListResultWebOptions()));

        private static (HttpContext Http, EndpointFilterInvocationContext Ctx) NewContext(
            string queryString = "",
            string path = "/items")
        {
            var http = new DefaultHttpContext();
            http.Request.Scheme = "https";
            http.Request.Host = new HostString("api.example.com");
            http.Request.Path = path;
            http.Request.QueryString = new QueryString(queryString);
            http.Request.Method = "GET";

            var ctx = EndpointFilterInvocationContext.Create(http);

            return (http, ctx);
        }

        private static EndpointFilterDelegate Returning(object value)
            => _ => ValueTask.FromResult(value);

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
        public async Task NullPayload_DoesNotEmitHeaders()
        {
            var (http, ctx) = NewContext();
            await Filter().InvokeAsync(ctx, Returning(null));

            Assert.IsFalse(http.Response.Headers.ContainsKey("X-Total-Count"));
            Assert.IsFalse(http.Response.Headers.ContainsKey("Link"));
        }

        [TestMethod]
        public async Task NonPagedPayload_DoesNotEmitHeaders()
        {
            var (http, ctx) = NewContext();
            await Filter().InvokeAsync(ctx, Returning(new SampleItem { Id = 1, Name = "n" }));

            Assert.IsFalse(http.Response.Headers.ContainsKey("X-Total-Count"));
        }

        [TestMethod]
        public async Task PagedPayload_DirectReturn_EmitsTotalCountHeaders()
        {
            var (http, ctx) = NewContext();
            await Filter().InvokeAsync(ctx, Returning(SamplePaged()));

            var h = http.Response.Headers;
            Assert.AreEqual("47", h["X-Total-Count"].ToString());
            Assert.AreEqual("5", h["X-Page-Count"].ToString());
            Assert.AreEqual("10", h["X-Page-Size"].ToString());
            Assert.AreEqual("2", h["X-Current-Page"].ToString());
        }

        [TestMethod]
        public async Task PagedPayload_WrappedInOk_EmitsHeaders()
        {
            var (http, ctx) = NewContext();
            var ok = TypedResults.Ok(SamplePaged());
            await Filter().InvokeAsync(ctx, Returning(ok));

            Assert.AreEqual("47", http.Response.Headers["X-Total-Count"].ToString());
        }

        [TestMethod]
        public async Task PagedPayload_WrappedInJsonHttpResult_EmitsHeaders()
        {
            var (http, ctx) = NewContext();
            var json = TypedResults.Json(SamplePaged());
            await Filter().InvokeAsync(ctx, Returning(json));

            Assert.AreEqual("47", http.Response.Headers["X-Total-Count"].ToString());
        }

        [TestMethod]
        public async Task TotalCountHeader_DisabledByOption()
        {
            var (http, ctx) = NewContext();
            await Filter(new PagedListResultWebOptions { EmitTotalCountHeader = false })
                .InvokeAsync(ctx, Returning(SamplePaged()));

            Assert.IsFalse(http.Response.Headers.ContainsKey("X-Total-Count"));
        }

        [TestMethod]
        public async Task LinkHeader_FirstPrevNextLast()
        {
            var (http, ctx) = NewContext();
            await Filter().InvokeAsync(ctx, Returning(SamplePaged(currentPage: 3, pageCount: 5, pageSize: 10)));

            var link = http.Response.Headers["Link"].ToString();
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
            var (http, ctx) = NewContext();
            await Filter().InvokeAsync(ctx, Returning(SamplePaged(currentPage: 1, pageCount: 5)));

            var link = http.Response.Headers["Link"].ToString();
            Assert.IsFalse(link.Contains("rel=\"prev\""));
            StringAssert.Contains(link, "rel=\"next\"");
        }

        [TestMethod]
        public async Task LinkHeader_LastPage_OmitsNext()
        {
            var (http, ctx) = NewContext();
            await Filter().InvokeAsync(ctx, Returning(SamplePaged(currentPage: 5, pageCount: 5)));

            var link = http.Response.Headers["Link"].ToString();
            Assert.IsFalse(link.Contains("rel=\"next\""));
            StringAssert.Contains(link, "rel=\"prev\"");
        }

        [TestMethod]
        public async Task LinkHeader_PreservesOtherQueryParameters()
        {
            var (http, ctx) = NewContext(
                queryString: "?filter=status:eq:active&order=name:asc&page=99&pageSize=99");

            await Filter().InvokeAsync(ctx, Returning(SamplePaged()));

            var link = http.Response.Headers["Link"].ToString();
            StringAssert.Contains(link, "filter=status%3Aeq%3Aactive");
            StringAssert.Contains(link, "order=name%3Aasc");
            Assert.IsFalse(link.Contains("page=99"));
            Assert.IsFalse(link.Contains("pageSize=99"));
        }

        [TestMethod]
        public async Task LinkHeader_DisabledByOption()
        {
            var (http, ctx) = NewContext();
            await Filter(new PagedListResultWebOptions { EmitLinkHeader = false })
                .InvokeAsync(ctx, Returning(SamplePaged()));

            Assert.IsFalse(http.Response.Headers.ContainsKey("Link"));
        }

        [TestMethod]
        public async Task LinkHeader_NotEmitted_WhenPageCountZero()
        {
            var (http, ctx) = NewContext();
            await Filter().InvokeAsync(ctx, Returning(SamplePaged(currentPage: 0, pageCount: 0, rowCount: 0)));

            Assert.IsFalse(http.Response.Headers.ContainsKey("Link"));
        }

        [TestMethod]
        public async Task ServerTiming_NotEmitted_ByDefault()
        {
            var (http, ctx) = NewContext();
            await Filter().InvokeAsync(ctx, Returning(SamplePaged()));

            Assert.IsFalse(http.Response.Headers.ContainsKey("Server-Timing"));
        }

        [TestMethod]
        public async Task ServerTiming_Emitted_WhenEnabled()
        {
            var (http, ctx) = NewContext();
            await Filter(new PagedListResultWebOptions { EmitServerTimingHeader = true })
                .InvokeAsync(ctx, Returning(SamplePaged(execMs: 123)));

            Assert.AreEqual("paged;dur=123", http.Response.Headers["Server-Timing"].ToString());
        }

        [TestMethod]
        public async Task Filter_ReturnsNextResult()
        {
            var (_, ctx) = NewContext();
            var paged = SamplePaged();

            var actual = await Filter().InvokeAsync(ctx, Returning(paged));

            Assert.AreSame(paged, actual);
        }

        [TestMethod]
        public async Task Filter_PropagatesWrappedResult()
        {
            var (_, ctx) = NewContext();
            var ok = TypedResults.Ok(SamplePaged());

            var actual = await Filter().InvokeAsync(ctx, Returning(ok));

            Assert.AreSame(ok, actual);
        }

        [TestMethod]
        public void Ctor_NullOptions_Throws()
            => Assert.ThrowsException<ArgumentNullException>(() => new PagedResultEndpointFilter(null));
    }
}
