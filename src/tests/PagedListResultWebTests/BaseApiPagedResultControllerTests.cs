// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultWebTests
//  Author           : RzR
//  Created On       : 2026-04-26 20:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 20:49
// ***********************************************************************
//  <copyright file="BaseApiPagedResultControllerTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResultWebTests.Stubs;
using RzR.ResultMessage.Pagination.Abstractions.Abstractions;
using RzR.ResultMessage.Pagination.Abstractions.Models.Result;
using RzR.ResultMessage.Pagination.AspNetCore;
using System.Collections.Generic;
using System.Net;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class BaseApiPagedResultControllerTests
    {
        private static TestController CreateController()
        {
            var services = new ServiceCollection().BuildServiceProvider();

            return new TestController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = services
                    }
                }
            };
        }

        private static PagedResult<SampleItem> SuccessPagedResult()
            => new()
            {
                IsSuccess = true,
                Response = new List<SampleItem> { new() { Id = 1, Name = "A" } },
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 10,
                RowCount = 1
            };

        [TestMethod]
        public void PagedOkResult_OnSuccess_ReturnsJsonResult()
        {
            var controller = CreateController();
            var paged = SuccessPagedResult();

            var result = controller.InvokePagedOk(paged);

            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult, "Expected JsonResult on success.");
            Assert.AreSame(paged, jsonResult.Value);
        }

        [TestMethod]
        public void PagedXmlResult_OnSuccess_ReturnsXmlContentResult()
        {
            var controller = CreateController();
            var paged = SuccessPagedResult();

            var result = controller.InvokePagedXml(paged);

            var content = result as ContentResult;
            Assert.IsNotNull(content, "Expected ContentResult on success.");
            Assert.AreEqual("text/xml", content.ContentType);
            Assert.AreEqual((int)HttpStatusCode.OK, content.StatusCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(content.Content), "Serialized XML must not be empty.");
        }

        private sealed class TestController : BaseApiPagedResultController
        {
            public IActionResult InvokePagedOk<T>(IPagedResult<T> response) where T : class
                => PagedOkResult(response);

            public IActionResult InvokePagedXml<T>(IPagedResult<T> response) where T : class
                => PagedXmlResult(response);
        }
    }
}