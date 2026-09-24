#region U S A G E S

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResultWebTests.Stubs;
using RzR.ResultMessage.Pagination.Abstractions.Abstractions;
using RzR.ResultMessage.Pagination.Abstractions.Models.Result;
using RzR.ResultMessage.Pagination.AspNetCore;
using RzR.ResultMessage.Pagination.AspNetCore.Filters;
using RzR.ResultMessage.Pagination.AspNetCore.Helpers;
using RzR.ResultMessage.Pagination.AspNetCore.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class PagedResultUnwrapRegressionTests
    {
        private static PagedResultActionFilter ActionFilter(PagedListResultWebOptions opts = null)
            => new(Options.Create(opts ?? new PagedListResultWebOptions()));

        private static PagedResultEndpointFilter EndpointFilter(PagedListResultWebOptions opts = null)
            => new(Options.Create(opts ?? new PagedListResultWebOptions()));

        private static ResultExecutingContext MvcContext(IActionResult result, string path = "/items")
        {
            var http = new DefaultHttpContext();
            http.Request.Scheme = "https";
            http.Request.Host = new HostString("api.example.com");
            http.Request.Path = path;
            http.Request.QueryString = new QueryString(string.Empty);
            http.Request.Method = "GET";

            var actionContext = new ActionContext(http, new RouteData(), new ActionDescriptor());

            return new ResultExecutingContext(actionContext, new List<IFilterMetadata>(), result, controller: null);
        }

        private static (HttpContext Http, EndpointFilterInvocationContext Ctx) MinimalApiContext(string path = "/items")
        {
            var http = new DefaultHttpContext();
            http.Request.Scheme = "https";
            http.Request.Host = new HostString("api.example.com");
            http.Request.Path = path;
            http.Request.QueryString = new QueryString(string.Empty);
            http.Request.Method = "GET";

            return (http, EndpointFilterInvocationContext.Create(http));
        }

        private static EndpointFilterDelegate Returning(object value)
            => _ => ValueTask.FromResult(value);

        private static PagedResult<SampleItem> SamplePaged(
            int currentPage = 2, int pageCount = 5, int pageSize = 10, int rowCount = 47, long execMs = 42)
        {
            var p = new PagedResult<SampleItem>
            {
                IsSuccess = true,
                Response = new List<SampleItem> { new()
                {
                    Id = 1, 
                    Name = "x"
                } },
                CurrentPage = currentPage,
                PageCount = pageCount,
                PageSize = pageSize,
                RowCount = rowCount
            };
            p.ExecutionDetails.SetExecutionTimeMs(execMs, DateTime.UtcNow);

            return p;
        }

        private static void AssertPaginationHeaders(HttpContext http, string because)
        {
            var h = http.Response.Headers;
            Assert.AreEqual("47", h["X-Total-Count"].ToString(), because);
            Assert.AreEqual("5", h["X-Page-Count"].ToString(), because);
            Assert.AreEqual("10", h["X-Page-Size"].ToString(), because);
            Assert.AreEqual("2", h["X-Current-Page"].ToString(), because);
        }

        private static async Task<ResultExecutingContext> RunMvcAsync(
            IActionResult result, PagedListResultWebOptions opts = null)
        {
            var ctx = MvcContext(result);
            await ActionFilter(opts).OnResultExecutionAsync(ctx, () => Task.FromResult<ResultExecutedContext>(null));

            return ctx;
        }

        [TestMethod]
        public async Task Mvc_ObjectResultPayload_EmitsPaginationHeaders()
        {
            var ctx = await RunMvcAsync(new ObjectResult(SamplePaged()));

            AssertPaginationHeaders(ctx.HttpContext, "ObjectResult.Value must still be unwrapped.");
        }

        [TestMethod]
        public async Task Mvc_ObjectResultSubclasses_EmitPaginationHeaders()
        {
            var results = new IActionResult[]
            {
                new OkObjectResult(SamplePaged()),
                new CreatedResult("/items/1", SamplePaged()),
                new BadRequestObjectResult(SamplePaged())
            };

            foreach (var result in results)
            {
                var ctx = await RunMvcAsync(result);
                AssertPaginationHeaders(ctx.HttpContext, $"{result.GetType().Name} must still be unwrapped.");
            }
        }

        [TestMethod]
        public async Task Mvc_JsonResultPayload_EmitsPaginationHeaders()
        {
            var ctx = await RunMvcAsync(new JsonResult(SamplePaged()));

            AssertPaginationHeaders(ctx.HttpContext, "JsonResult.Value must still be unwrapped.");
        }

        [TestMethod]
        public void Mvc_JsonResult_IsNotAnObjectResult()
        {
            Assert.IsFalse(
                typeof(ObjectResult).IsAssignableFrom(typeof(JsonResult)),
                "If JsonResult ever derives from ObjectResult the NT-2 guard stops being independent.");
        }

        [TestMethod]
        public void Controller_PagedOkResult_ValueIsReferenceIdenticalToEnvelope()
        {
            var paged = SamplePaged();
            var result = CreateController().InvokePagedOk(paged);
            var value = ReadValueProperty(result);

            Assert.IsNotNull(value, "The returned result must expose a Value.");
            Assert.AreSame(
                paged, value,
                $"PagedOkResult must carry the whole envelope; got '{value.GetType().FullName}'.");
        }

        [TestMethod]
        public void Controller_PagedOkResult_ValueIsNotTheItemsCollection()
        {
            var paged = SamplePaged();
            var result = CreateController().InvokePagedOk(paged);
            var value = ReadValueProperty(result);

            Assert.AreNotSame(paged.Response, value, "Value must be the envelope, not the items.");
            Assert.IsTrue(
                value is not null && PagedResponseHeaderWriter.ImplementsIPagedResult(value.GetType()),
                "Value must implement IPagedResult<> or the action filter emits no headers.");
        }

        [TestMethod]
        public async Task Mvc_ContentResult_EmitsNoHeadersAndDoesNotThrow()
        {
            var ctx = await RunMvcAsync(new ContentResult { Content = "<x/>", ContentType = "text/xml" });

            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("X-Total-Count"));
            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("Link"));
        }

        [TestMethod]
        public async Task Mvc_EmptyResult_EmitsNoHeadersAndDoesNotThrow()
        {
            var ctx = await RunMvcAsync(new EmptyResult());

            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("X-Total-Count"));
            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("Link"));
        }

        [TestMethod]
        public async Task Mvc_CustomResultWithPagedValue_EmitsHeaders_DeclaredWidening()
        {
            var ctx = await RunMvcAsync(new ValueCarryingResult { Value = SamplePaged() });

            AssertPaginationHeaders(ctx.HttpContext, "Row 12 widening is declared and accepted.");
        }

        [TestMethod]
        public async Task Mvc_JsonResultWithNonPagedValue_EmitsNoHeaders()
        {
            var ctx = await RunMvcAsync(new JsonResult(new SampleItem { Id = 1, Name = "n" }));

            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("X-Total-Count"));
            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("Link"));
        }

        [TestMethod]
        public async Task Mvc_JsonResultWithNullValue_DoesNotThrowAndEmitsNoHeaders()
        {
            var ctx = await RunMvcAsync(new JsonResult(null));

            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("X-Total-Count"));
            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("Link"));
        }

        [TestMethod]
        public async Task Mvc_ResultWithThrowingValueGetter_DoesNotFaultTheResponse()
        {
            var ctx = await RunMvcAsync(new ThrowingValueResult());

            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("X-Total-Count"));
            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("Link"));
        }

        [TestMethod]
        public async Task Mvc_CustomResultWithNonPagedValue_EmitsNoHeaders()
        {
            var ctx = await RunMvcAsync(new ValueCarryingResult
            {
                Value = new SampleItem { Id = 1, Name = "n" }
            });

            Assert.IsFalse(ctx.HttpContext.Response.Headers.ContainsKey("X-Total-Count"));
        }

        [TestMethod]
        public async Task MinimalApi_JsonHttpResultPayload_StillEmitsHeaders()
        {
            var (http, ctx) = MinimalApiContext();
            await EndpointFilter().InvokeAsync(ctx, Returning(TypedResults.Json(SamplePaged())));

            AssertPaginationHeaders(http, "JsonHttpResult<T> must still be unwrapped by the shared helper.");
        }

        [TestMethod]
        public async Task MinimalApi_OkTypedResultPayload_StillEmitsHeaders()
        {
            var (http, ctx) = MinimalApiContext();
            await EndpointFilter().InvokeAsync(ctx, Returning(TypedResults.Ok(SamplePaged())));

            AssertPaginationHeaders(http, "Ok<T> must still be unwrapped by the shared helper.");
        }

        [TestMethod]
        public async Task MinimalApi_MvcJsonResultPayload_EmitsHeaders_UnwrapIsNotNamespaceScoped()
        {
            var (http, ctx) = MinimalApiContext();
            await EndpointFilter().InvokeAsync(ctx, Returning(new JsonResult(SamplePaged())));

            AssertPaginationHeaders(http, "UnwrapResultValue must not be scoped to one results namespace.");
        }

        private static object ReadValueProperty(object result)
        {
            Assert.IsNotNull(result, "The controller must return a result.");

            var prop = result.GetType().GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
            Assert.IsNotNull(prop, $"'{result.GetType().FullName}' exposes no public Value property.");

            return prop.GetValue(result);
        }

        private static TestController CreateController()
        {
            var services = new ServiceCollection().BuildServiceProvider();

            return new TestController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { RequestServices = services }
                }
            };
        }

        private sealed class TestController : BaseApiPagedResultController
        {
            public IActionResult InvokePagedOk<T>(IPagedResult<T> response) where T : class
                => PagedOkResult(response);

            public IActionResult InvokeObsoleteJsonResult<T>(IPagedResult<T> response) where T : class
            {
#pragma warning disable CS0618
                return JsonResult(response);
#pragma warning restore CS0618
            }
        }

        private sealed class ValueCarryingResult : IActionResult
        {
            public object Value { get; set; }

            public Task ExecuteResultAsync(ActionContext context) => Task.CompletedTask;
        }

        private sealed class ThrowingValueResult : IActionResult
        {
            public object Value => throw new InvalidOperationException("Value getter is broken.");

            public Task ExecuteResultAsync(ActionContext context) => Task.CompletedTask;
        }
    }
}
