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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResultWebTests.Stubs;
using RzR.ResultMessage.Pagination.DataModels.Models.Result;
using RzR.ResultMessage.Pagination.Web.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class PagedResultsHttpExtensionsTests
    {
        private static PagedResult<SampleItem> SuccessPaged()
            => new()
            {
                IsSuccess = true,
                Response = new List<SampleItem> { new() { Id = 1, Name = "x" } },
                CurrentPage = 2,
                PageCount = 5,
                PageSize = 10,
                RowCount = 47
            };

        private static PagedResult<SampleItem> FailurePaged()
        {
            var p = new PagedResult<SampleItem> { IsSuccess = false };

            return p;
        }

        [TestMethod]
        public void NullPagedResult_ReturnsNoContent()
        {
            IPagedResult_Null_Helper(out var http);
            Assert.AreEqual((int)HttpStatusCode.NoContent, http.Response.StatusCode);
        }

        private static void IPagedResult_Null_Helper(out HttpContext http)
        {
            PagedResult<SampleItem> nullPaged = null;
            var result = nullPaged.ToPagedHttpResult();
            http = new DefaultHttpContext { RequestServices = TestServiceProvider.Empty };

            // Execute to materialize the status code.
            result.ExecuteAsync(http).GetAwaiter().GetResult();
        }

        [TestMethod]
        public async Task SuccessPath_ReturnsOkOfPagedResult_PreservingMetadata()
        {
            var paged = SuccessPaged();
            var result = paged.ToPagedHttpResult();

            // Assert it's TypedResults.Ok<IPagedResult<SampleItem>> (or similar Ok<T>).
            var t = result.GetType();
            StringAssert.StartsWith(t.Name, "Ok");
            var valueProp = t.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
            Assert.IsNotNull(valueProp, "Result type should expose a 'Value' property.");
            var value = valueProp!.GetValue(result);
            Assert.AreSame(paged, value, "Body should be the entire paged envelope, not just .Response.");

            // And executing it should serialize the WHOLE envelope to JSON (so paging
            // metadata reaches the client).
            var http = new DefaultHttpContext
            {
                RequestServices = TestServiceProvider.Empty,
                Response = { Body = new MemoryStream() }
            };
            await result.ExecuteAsync(http);

            http.Response.Body.Position = 0;
            using var doc = JsonDocument.Parse(http.Response.Body);
            Assert.AreEqual(2, doc.RootElement.GetProperty("currentPage").GetInt32());
            Assert.AreEqual(5, doc.RootElement.GetProperty("pageCount").GetInt32());
            Assert.AreEqual(47, doc.RootElement.GetProperty("rowCount").GetInt32());
            Assert.IsTrue(doc.RootElement.TryGetProperty("response", out var arr));
            Assert.AreEqual(JsonValueKind.Array, arr.ValueKind);
            Assert.AreEqual(1, arr.GetArrayLength());
        }

        [TestMethod]
        public async Task FailurePath_ReturnsProblemDetails()
        {
            var paged = FailurePaged();
            var result = paged.ToPagedHttpResult();

            var http = new DefaultHttpContext
            {
                RequestServices = TestServiceProvider.Empty,
                Response = { Body = new MemoryStream() }
            };
            await result.ExecuteAsync(http);

            // Failure should be a non-2xx status with application/problem+json.
            Assert.IsTrue(http.Response.StatusCode >= 400, $"Expected 4xx, got {http.Response.StatusCode}.");
            StringAssert.Contains(http.Response.ContentType ?? string.Empty, "problem+json");
        }

        private sealed class TestServiceProvider : IServiceProvider
        {
            private static readonly IServiceProvider Inner =
                new ServiceCollection()
                    .AddLogging()
                    .BuildServiceProvider();

            public static readonly IServiceProvider Empty = new TestServiceProvider();

            public object GetService(Type serviceType) => Inner.GetService(serviceType);
        }
    }
}
