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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResultWebTests.Stubs;
using RzR.ResultMessage.Pagination.Abstractions.Models.Request.Page;
using RzR.ResultMessage.Pagination.AspNetCore.Abstractions;
using RzR.ResultMessage.Pagination.AspNetCore.Builders;
using RzR.ResultMessage.Pagination.AspNetCore.Configuration;
using RzR.ResultMessage.Pagination.AspNetCore.Query;
using RzR.ResultMessage.Pagination.AspNetCore.ModelBinding;
using RzR.ResultMessage.Pagination.AspNetCore.Models;
using RzR.ResultMessage.Pagination.AspNetCore.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class PagedRequestModelBinderTests
    {
        private static IServiceProvider BuildServices(PageableMetadata allow = null)
        {
            var sc = new ServiceCollection();
            sc.AddOptions();
            var registry = new PageableMetadataRegistry();
            if (allow != null) 
                registry.Register(typeof(SampleItem), allow);

            sc.AddSingleton<IPageableMetadataRegistry>(registry);

            return sc.BuildServiceProvider();
        }

        private static ModelBindingContext MakeCtx(Type modelType, IServiceProvider sp, params (string k, string v)[] qs)
        {
            var http = new DefaultHttpContext { RequestServices = sp };
            var dict = new Dictionary<string, StringValues>();
            foreach (var (k, v) in qs) 
                dict[k] = v;
            http.Request.Query = new QueryCollection(dict);

            var metaProvider = new EmptyModelMetadataProvider();

            return new DefaultModelBindingContext
            {
                ActionContext = new Microsoft.AspNetCore.Mvc.ActionContext
                {
                    HttpContext = http,
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                    ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
                },
                ModelMetadata = metaProvider.GetMetadataForType(modelType),
                ModelName = "req",
                ModelState = new ModelStateDictionary(),
                ValidationState = new ValidationStateDictionary()
            };
        }

        [TestMethod]
        public async Task Binder_PopulatesPagedRequest_FromQuery()
        {
            var sp = BuildServices();
            var ctx = MakeCtx(typeof(PagedRequest), sp, ("page", "2"), ("pageSize", "5"), ("order", "name:desc"));

            var binder = new PagedRequestModelBinder(typeof(PagedRequest), entityType: null);
            await binder.BindModelAsync(ctx);

            Assert.IsTrue(ctx.Result.IsModelSet);
            var req = (PagedRequest)ctx.Result.Model;
            Assert.AreEqual(2, req.Page);
            Assert.AreEqual(5, req.PageSize);
            Assert.AreEqual("name", req.Order.OrderByProperty);
        }

        [TestMethod]
        public async Task Binder_OnInvalidInput_FailsAndPopulatesModelState()
        {
            var sp = BuildServices();
            var ctx = MakeCtx(typeof(PagedRequest), sp, ("page", "abc"));

            var binder = new PagedRequestModelBinder(typeof(PagedRequest), entityType: null);
            await binder.BindModelAsync(ctx);

            Assert.IsFalse(ctx.Result.IsModelSet);
            Assert.IsTrue(ctx.ModelState.ContainsKey("page"));
        }

        [TestMethod]
        public async Task Binder_AppliesAllowList_WhenEntityTypeProvided()
        {
            var allow = new PageableMetadataBuilder<SampleItem>().AllowSort("name").Build();
            var sp = BuildServices(allow);
            var ctx = MakeCtx(typeof(PagedRequest), sp, ("order", "secret:asc"));

            var binder = new PagedRequestModelBinder(typeof(PagedRequest), entityType: typeof(SampleItem));
            await binder.BindModelAsync(ctx);

            Assert.IsFalse(ctx.Result.IsModelSet);
            Assert.IsTrue(ctx.ModelState.ContainsKey("order"));
        }

        [TestMethod]
        public async Task Binder_BindsPageRequestWithFilters_IncludingFilters()
        {
            var sp = BuildServices();
            var ctx = MakeCtx(
                typeof(PageRequestWithFilters),
                sp,
                ("filter", "name:Equals:foo"));

            var binder = new PagedRequestModelBinder(typeof(PageRequestWithFilters), entityType: null);
            await binder.BindModelAsync(ctx);

            Assert.IsTrue(ctx.Result.IsModelSet);
            var req = (PageRequestWithFilters)ctx.Result.Model;
            Assert.AreEqual(1, req.Filters.Count);
        }

#if NET7_0_OR_GREATER

        [TestMethod]
        public async Task Binder_OnInvalidInput_ForPagedQuery_SetsModelAndPopulatesInstanceErrors()
        {
            var sp = BuildServices();
            var ctx = MakeCtx(typeof(PagedQuery<SampleItem>), sp, ("page", "abc"));

            var binder = new PagedRequestModelBinder(typeof(PagedQuery<SampleItem>), entityType: typeof(SampleItem));
            await binder.BindModelAsync(ctx);

            // (a) Model must still be set so the caller receives an instance to inspect.
            Assert.IsTrue(ctx.Result.IsModelSet);

            // (a) ModelState still carries the error, same as the legacy PagedRequest path.
            Assert.IsTrue(ctx.ModelState.ContainsKey("page"));

            var model = (PagedQuery<SampleItem>)ctx.Result.Model;

            // (a) The instance's own IPagedQueryValidation.Errors dictionary is populated too.
            Assert.IsTrue(model.Errors.ContainsKey("page"));
            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public async Task Binder_OnValidInput_ForPagedQuery_BindsAndInstanceIsValid()
        {
            var sp = BuildServices();
            var ctx = MakeCtx(typeof(PagedQuery<SampleItem>), sp, ("page", "2"), ("pageSize", "5"));

            var binder = new PagedRequestModelBinder(typeof(PagedQuery<SampleItem>), entityType: typeof(SampleItem));
            await binder.BindModelAsync(ctx);

            Assert.IsTrue(ctx.Result.IsModelSet);

            var model = (PagedQuery<SampleItem>)ctx.Result.Model;

            // (b) No errors surfaced, instance reports valid, values bound normally.
            Assert.AreEqual(0, model.Errors.Count);
            Assert.IsTrue(model.IsValid);
            Assert.AreEqual(2, model.Page);
            Assert.AreEqual(5, model.PageSize);
        }

        [TestMethod]
        public async Task Binder_BindsPagedQueryWithFilters_IncludingFilters()
        {
            var sp = BuildServices();
            var ctx = MakeCtx(
                typeof(PagedQueryWithFilters<SampleItem>),
                sp,
                ("filter", "name:Equals:foo"));

            var binder = new PagedRequestModelBinder(
                typeof(PagedQueryWithFilters<SampleItem>), entityType: typeof(SampleItem));
            await binder.BindModelAsync(ctx);

            Assert.IsTrue(ctx.Result.IsModelSet);

            var model = (PagedQueryWithFilters<SampleItem>)ctx.Result.Model;

            Assert.AreEqual(1, model.Filters.Count);
            Assert.AreEqual(0, model.Errors.Count);
            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public async Task Binder_OnInvalidInput_ForPagedQueryWithFilters_PopulatesInstanceErrors()
        {
            var allow = new PageableMetadataBuilder<SampleItem>().AllowFilter("name").Build();
            var sp = BuildServices(allow);
            var ctx = MakeCtx(
                typeof(PagedQueryWithFilters<SampleItem>),
                sp,
                ("filter", "secret:Equals:foo"));

            var binder = new PagedRequestModelBinder(
                typeof(PagedQueryWithFilters<SampleItem>), entityType: typeof(SampleItem));
            await binder.BindModelAsync(ctx);

            // (a) Same footgun guard applies to the filters-flavored wrapper.
            Assert.IsTrue(ctx.Result.IsModelSet);
            Assert.IsTrue(ctx.ModelState.ContainsKey("filter[0]"));

            var model = (PagedQueryWithFilters<SampleItem>)ctx.Result.Model;

            Assert.IsTrue(model.Errors.ContainsKey("filter[0]"));
            Assert.IsFalse(model.IsValid);
        }

#endif

        [TestMethod]
        public async Task Binder_OnInvalidInput_ForPlainPagedRequest_KeepsOldBehavior_NoModelSet()
        {
            // (c) Regression guard: plain PagedRequest does NOT implement IPagedQueryValidation,
            // so the old contract (no model set, only ModelState populated) must be unchanged.
            var sp = BuildServices();
            var ctx = MakeCtx(typeof(PagedRequest), sp, ("page", "abc"));

            var binder = new PagedRequestModelBinder(typeof(PagedRequest), entityType: null);
            await binder.BindModelAsync(ctx);

            Assert.IsFalse(ctx.Result.IsModelSet);
            Assert.IsTrue(ctx.ModelState.ContainsKey("page"));
        }

        [TestMethod]
        public async Task Binder_OnInvalidInput_ForPlainPageRequestWithFilters_KeepsOldBehavior_NoModelSet()
        {
            // (c) Regression guard for the filters-flavored plain type as well.
            var sp = BuildServices();
            var ctx = MakeCtx(typeof(PageRequestWithFilters), sp, ("filter", "bad"));

            var binder = new PagedRequestModelBinder(typeof(PageRequestWithFilters), entityType: null);
            await binder.BindModelAsync(ctx);

            Assert.IsFalse(ctx.Result.IsModelSet);
            Assert.IsTrue(ctx.ModelState.ContainsKey("filter[0]"));
        }

        [TestMethod]
        public void Provider_IsRegistered_ByAddPagedListResultWeb()
        {
            var sc = new ServiceCollection();
            sc.AddOptions();
            sc.AddPagedListResultWeb();

            var sp = sc.BuildServiceProvider();
            var mvc = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.Mvc.MvcOptions>>().Value;

            Assert.IsTrue(
                mvc.ModelBinderProviders.OfType<PagedRequestModelBinderProvider>().Any(),
                "PagedRequestModelBinderProvider should be inserted into MvcOptions.ModelBinderProviders.");
        }
    }
}
