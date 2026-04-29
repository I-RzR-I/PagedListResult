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
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResultWebTests.Stubs;
using RzR.ResultMessage.Pagination.Web.Abstractions;
using RzR.ResultMessage.Pagination.Web.Builders;
using RzR.ResultMessage.Pagination.Web.MinimalApi;
using RzR.ResultMessage.Pagination.Web.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class PagedQueryBindAsyncTests
    {
        private static HttpContext Http(IServiceProvider sp = null, params (string, string)[] qs)
        {
            var http = new DefaultHttpContext { RequestServices = sp ?? new ServiceCollection().AddOptions().BuildServiceProvider() };
            var d = new Dictionary<string, StringValues>();
            foreach (var (k, v) in qs) 
                d[k] = v;

            http.Request.Query = new QueryCollection(d);

            return http;
        }

        [TestMethod]
        public async Task BindAsync_PagedQuery_PopulatesFromQuery()
        {
            var http = Http(qs: new[] { ("page", "4"), ("pageSize", "7") });
            var pq = await PagedQuery<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsTrue(pq.IsValid);
            Assert.AreEqual(4, pq.Page);
            Assert.AreEqual(7, pq.PageSize);
        }

        [TestMethod]
        public async Task BindAsync_OnInvalid_ExposesErrors()
        {
            var http = Http(qs: new[] { ("pageSize", "999999") });
            var pq = await PagedQuery<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsFalse(pq.IsValid);
            Assert.IsTrue(pq.Errors.ContainsKey("pageSize"));
        }

        [TestMethod]
        public async Task BindAsync_AppliesAllowList_FromRegistry()
        {
            var sc = new ServiceCollection().AddOptions();
            var registry = new PageableMetadataRegistry();
            registry.Register(typeof(SampleItem),
                new PageableMetadataBuilder<SampleItem>().AllowSort("name").Build());
            sc.AddSingleton<IPageableMetadataRegistry>(registry);
            var sp = sc.BuildServiceProvider();

            var http = Http(sp, ("order", "secret:asc"));
            var pq = await PagedQuery<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsFalse(pq.IsValid);
            Assert.IsTrue(pq.Errors.ContainsKey("order"));
        }

        [TestMethod]
        public async Task BindAsync_PagedQueryWithFilters_ParsesFilter()
        {
            var http = Http(qs: new[] { ("filter", "name:Equals:foo") });
            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsTrue(pq.IsValid);
            Assert.AreEqual(1, pq.Filters.Count);
            Assert.AreEqual("name", pq.Filters.First().FilterValue.PropertyName);
        }
    }
}
