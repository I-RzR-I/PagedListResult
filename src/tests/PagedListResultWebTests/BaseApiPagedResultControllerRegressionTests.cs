#region U S A G E S

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResultWebTests.Stubs;
using RzR.ResultMessage.Extensions.Result.Messages;
using RzR.ResultMessage.Pagination.Abstractions.Abstractions;
using RzR.ResultMessage.Pagination.Abstractions.Models.Result;
using RzR.ResultMessage.Pagination.AspNetCore;
using RzR.ResultMessage.Pagination.AspNetCore.Extensions;
using RzR.ResultMessage.Web.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class BaseApiPagedResultControllerRegressionTests
    {
        private const string FailureMessageKey = "paged.query.invalid";

        private static readonly IServiceProvider MinimalApiServices =
            new ServiceCollection().AddLogging().BuildServiceProvider();

        private static TestController CreateController()
            => new()
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = new ServiceCollection().BuildServiceProvider()
                    }
                }
            };

        private static PagedResult<SampleItem> SuccessPagedResult()
            => new()
            {
                IsSuccess = true,
                Response = new List<SampleItem> { new() { Id = 1, Name = "A" } },
                CurrentPage = 2,
                PageCount = 5,
                PageSize = 10,
                RowCount = 47
            };

        private static PagedResult<SampleItem> FailedPagedResult()
            => new() { IsSuccess = false };

        private static PagedResult<SampleItem> FailedPagedResultWithKeyedMessage()
        {
            var paged = new PagedResult<SampleItem> { IsSuccess = false };
            paged.AddError(FailureMessageKey, "The paged query is invalid.");

            return paged;
        }

        private static ObjectResult AssertBadRequestProblem(IActionResult result)
        {
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult, "The failure path must produce an ObjectResult.");
            Assert.AreEqual(
                (int)HttpStatusCode.BadRequest,
                objectResult.StatusCode.GetValueOrDefault(),
                "The failure path must pin 400, never a 5xx or an unset status.");
            Assert.IsInstanceOfType(
                objectResult.Value, typeof(ResultMessageProblemDetails),
                "The failure body must be an RFC 9457 problem-details object.");
            Assert.IsNotInstanceOfType(
                objectResult.Value, typeof(System.Collections.IEnumerable),
                "The failure body must not be the raw result-message collection.");

            return objectResult;
        }

        private static void AssertCarriesEnvelope(IActionResult result, IPagedResult<SampleItem> expected)
        {
            Assert.IsFalse(
                result is StatusCodeResult,
                "Overload resolution bound the non-generic JsonWholeResult(IResult), which returns a bodiless 204 "
                + "and silently drops the paged envelope.");

            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult, $"Expected a body-carrying JsonResult, got '{result.GetType().FullName}'.");
            Assert.AreSame(expected, jsonResult.Value, "The body must be the whole paged envelope.");
            Assert.AreEqual(
                (int)HttpStatusCode.OK,
                jsonResult.StatusCode.GetValueOrDefault((int)HttpStatusCode.OK),
                "The generic overload must keep the default 200, never the bodiless 204.");
        }

        [TestMethod]
        public void PagedOkResult_NullResponse_ReturnsNoContent_Test()
        {
            IPagedResult<SampleItem> nothing = null;

            var result = CreateController().InvokePagedOk(nothing);

            var noContent = result as NoContentResult;
            Assert.IsNotNull(noContent, $"A null response must return 204, got '{result?.GetType().FullName}'.");
            Assert.AreEqual((int)HttpStatusCode.NoContent, noContent.StatusCode);
        }

        [TestMethod]
        public void PagedXmlResult_NullResponse_ReturnsNoContent_Test()
        {
            IPagedResult<SampleItem> nothing = null;

            var result = CreateController().InvokePagedXml(nothing);

            var noContent = result as NoContentResult;
            Assert.IsNotNull(noContent, $"A null response must return 204, got '{result?.GetType().FullName}'.");
            Assert.AreEqual((int)HttpStatusCode.NoContent, noContent.StatusCode);
        }

        [TestMethod]
        public async Task PagedOkResult_NullResponse_AgreesWithMinimalApiTwin_Test()
        {
            IPagedResult<SampleItem> nothing = null;

            var mvcResult = CreateController().InvokePagedOk(nothing) as StatusCodeResult;
            var minimalApiResult = nothing.ToPagedHttpResult();
            var http = new DefaultHttpContext { RequestServices = MinimalApiServices };
            await minimalApiResult.ExecuteAsync(http);

            Assert.IsNotNull(mvcResult, "The MVC path must return a status-only result for a null response.");
            Assert.AreEqual(
                (int)HttpStatusCode.NoContent, mvcResult.StatusCode,
                "The MVC and minimal-API hosting models must not drift on the null response.");
            Assert.AreEqual(
                (int)HttpStatusCode.NoContent, http.Response.StatusCode,
                "The MVC and minimal-API hosting models must not drift on the null response.");
        }

        [TestMethod]
        public void JsonWholeResult_InferredOverload_ReturnsEnvelopeCarryingOk_Test()
        {
            var paged = SuccessPagedResult();

            var result = CreateController().InvokeJsonWholeResultInferred(paged);

            AssertCarriesEnvelope(result, paged);
        }

        [TestMethod]
        public void JsonWholeResult_ExplicitListTypeArgument_ReturnsEnvelopeCarryingOk_Test()
        {
            var paged = SuccessPagedResult();

            var result = CreateController().InvokeJsonWholeResultExplicit(paged);

            AssertCarriesEnvelope(result, paged);
        }

        [TestMethod]
        public void PagedOkResult_FailedResult_ReturnsBadRequestProblemDetails_Test()
        {
            var result = CreateController().InvokePagedOk(FailedPagedResult());

            AssertBadRequestProblem(result);
        }

        [TestMethod]
        public void PagedXmlResult_FailedResult_ReturnsBadRequestProblemDetails_Test()
        {
            var result = CreateController().InvokePagedXml(FailedPagedResult());

            AssertBadRequestProblem(result);
        }

        [TestMethod]
        public void PagedOkResult_FailedResultWithKeyedMessage_EmitsKeyAsProblemCode_Test()
        {
            var result = CreateController().InvokePagedOk(FailedPagedResultWithKeyedMessage());

            var problem = AssertBadRequestProblem(result).Value as ResultMessageProblemDetails;
            Assert.IsNotNull(problem);
            Assert.AreEqual(
                FailureMessageKey, problem.Code,
                "The top-level problem-details 'code' must carry the key of the first non-exception message.");
        }

        private sealed class TestController : BaseApiPagedResultController
        {
            public IActionResult InvokePagedOk<T>(IPagedResult<T> response) where T : class
                => PagedOkResult(response);

            public IActionResult InvokePagedXml<T>(IPagedResult<T> response) where T : class
                => PagedXmlResult(response);

            public IActionResult InvokeJsonWholeResultInferred<T>(IPagedResult<T> response) where T : class
                => JsonWholeResult(response);

            public IActionResult InvokeJsonWholeResultExplicit<T>(IPagedResult<T> response) where T : class
                => JsonWholeResult<IList<T>>(response);
        }
    }
}
