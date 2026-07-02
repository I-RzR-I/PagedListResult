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
using RzR.ResultMessage.Pagination.Abstractions.Enums;
using RzR.ResultMessage.Pagination.AspNetCore.Abstractions;
using RzR.ResultMessage.Pagination.AspNetCore.Builders;
using RzR.ResultMessage.Pagination.AspNetCore.Query;
using RzR.ResultMessage.Pagination.AspNetCore.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class PagedQueryWithFiltersBindAsyncTests
    {
        private static HttpContext Http(IServiceProvider sp = null, params (string key, string value)[] qs)
        {
            var http = new DefaultHttpContext
            {
                RequestServices = sp ?? new ServiceCollection().AddOptions().BuildServiceProvider()
            };

            var d = new Dictionary<string, StringValues>();
            foreach (var grp in qs.GroupBy(x => x.key))
                d[grp.Key] = new StringValues(grp.Select(x => x.value).ToArray());

            http.Request.Query = new QueryCollection(d);

            return http;
        }

        private static IServiceProvider WithAllowList(Action<PageableMetadataBuilder<SampleItem>> configure)
        {
            var sc = new ServiceCollection().AddOptions();
            var registry = new PageableMetadataRegistry();
            var builder = new PageableMetadataBuilder<SampleItem>();
            configure(builder);
            registry.Register(typeof(SampleItem), builder.Build());
            sc.AddSingleton<IPageableMetadataRegistry>(registry);

            return sc.BuildServiceProvider();
        }


        [TestMethod]
        public async Task BindAsync_EmptyQuery_ProducesValidEmptyFilters()
        {
            var http = Http();

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsTrue(pq.IsValid);
            Assert.IsNotNull(pq.Filters);
            Assert.AreEqual(0, pq.Filters.Count);
        }

        [TestMethod]
        public async Task BindAsync_SingleFilter_PopulatesValuesAndCondition()
        {
            var http = Http(qs: new[] { ("filter", "name:Equals:foo") });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsTrue(pq.IsValid);
            Assert.AreEqual(1, pq.Filters.Count);

            var f = pq.Filters.Single().FilterValue;
            Assert.AreEqual("name", f.PropertyName);
            Assert.AreEqual(FilterType.Equals, f.Condition);
            CollectionAssert.AreEquivalent(new[] { "foo" }, f.Values.ToArray());
            Assert.IsNull(f.CompareValue);
        }

        [TestMethod]
        public async Task BindAsync_MultipleFilters_AreAndCombined_AndPreserveApplyOrder()
        {
            var http = Http(qs: new[]
            {
                ("filter", "name:Equals:foo"),
                ("filter", "id:GreaterThan:5"),
                ("filter", "name:Contains:bar")
            });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsTrue(pq.IsValid);
            Assert.AreEqual(3, pq.Filters.Count);

            var ordered = pq.Filters.OrderBy(f => f.FilterApplyOrder).ToList();
            Assert.AreEqual(0, ordered[0].FilterApplyOrder);
            Assert.AreEqual(1, ordered[1].FilterApplyOrder);
            Assert.AreEqual(2, ordered[2].FilterApplyOrder);

            Assert.AreEqual(FilterType.Equals, ordered[0].FilterValue.Condition);
            Assert.AreEqual(FilterType.GreaterThan, ordered[1].FilterValue.Condition);
            Assert.AreEqual(FilterType.Contains, ordered[2].FilterValue.Condition);
        }

        [TestMethod]
        public async Task BindAsync_IsInFilter_CsvTokensFlowIntoValues()
        {
            var http = Http(qs: new[] { ("filter", "name:IsIn:a,b,c") });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsTrue(pq.IsValid);

            var f = pq.Filters.Single().FilterValue;
            Assert.AreEqual(FilterType.IsIn, f.Condition);
            CollectionAssert.AreEquivalent(new[] { "a", "b", "c" }, f.Values.ToArray());
            Assert.IsNull(f.CompareValue);
        }

        [TestMethod]
        public async Task BindAsync_BetweenFilter_SplitsLeftIntoValuesAndRightIntoCompareValue()
        {
            var http = Http(qs: new[] { ("filter", "id:Between:10,20") });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsTrue(pq.IsValid);

            var f = pq.Filters.Single().FilterValue;
            Assert.AreEqual(FilterType.Between, f.Condition);
            CollectionAssert.AreEquivalent(new[] { "10" }, f.Values.ToArray());
            Assert.AreEqual("20", f.CompareValue);
        }

        [TestMethod]
        public async Task BindAsync_IsNullFilter_LeavesValuesAndCompareValueEmpty()
        {
            var http = Http(qs: new[] { ("filter", "name:IsNull:") });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsTrue(pq.IsValid);

            var f = pq.Filters.Single().FilterValue;
            Assert.AreEqual(FilterType.IsNull, f.Condition);
            Assert.AreEqual(0, f.Values.Count);
            Assert.IsNull(f.CompareValue);
        }

        [TestMethod]
        public async Task BindAsync_FilterCondition_IsCaseInsensitive()
        {
            var http = Http(qs: new[] { ("filter", "name:EQUALS:foo") });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsTrue(pq.IsValid);
            Assert.AreEqual(FilterType.Equals, pq.Filters.Single().FilterValue.Condition);
        }

        [TestMethod]
        public async Task BindAsync_CombinedWithPagingAndOrder_FillsAllSections()
        {
            var http = Http(qs: new[]
            {
                ("page", "3"),
                ("pageSize", "15"),
                ("order", "name:asc"),
                ("filter", "name:Equals:foo")
            });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsTrue(pq.IsValid);
            Assert.AreEqual(3, pq.Page);
            Assert.AreEqual(15, pq.PageSize);
            Assert.AreEqual(1, pq.Filters.Count);
            Assert.IsNotNull(pq.Order);
            Assert.AreEqual("name", pq.Order.OrderByProperty);
        }

        [TestMethod]
        public async Task BindAsync_BadFilterShape_ReportsErrorOnFirstRow()
        {
            var http = Http(qs: new[] { ("filter", "no-colons-here") });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsFalse(pq.IsValid);
            Assert.IsTrue(pq.Errors.ContainsKey("filter[0]"));
            Assert.AreEqual(0, pq.Filters.Count);
        }

        [TestMethod]
        public async Task BindAsync_UnknownCondition_ReportsErrorOnFirstRow()
        {
            var http = Http(qs: new[] { ("filter", "name:bogus:x") });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsFalse(pq.IsValid);
            Assert.IsTrue(pq.Errors.ContainsKey("filter[0]"));
        }

        [TestMethod]
        public async Task BindAsync_BetweenWithSingleValue_ReportsError()
        {
            var http = Http(qs: new[] { ("filter", "id:Between:10") });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsFalse(pq.IsValid);
            Assert.IsTrue(pq.Errors.ContainsKey("filter[0]"));
            StringAssert.Contains(pq.Errors["filter[0]"], "Between");
        }

        [TestMethod]
        public async Task BindAsync_TwoBadFilters_EachGetsItsOwnErrorKey()
        {
            var http = Http(qs: new[]
            {
                ("filter", "name:bogus:x"),
                ("filter", "no-colons-here")
            });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsFalse(pq.IsValid);
            Assert.IsTrue(pq.Errors.ContainsKey("filter[0]"));
            Assert.IsTrue(pq.Errors.ContainsKey("filter[1]"));
            Assert.AreNotEqual(pq.Errors["filter[0]"], pq.Errors["filter[1]"]);
        }

        [TestMethod]
        public async Task BindAsync_OneValidAndOneBadFilter_ValidIsKeptBadIsReported()
        {
            var http = Http(qs: new[]
            {
                ("filter", "name:Equals:foo"),     // row 0 - valid
                ("filter", "name:bogus:x")         // row 1 - bad condition
            });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsFalse(pq.IsValid);
            Assert.AreEqual(1, pq.Filters.Count);
            Assert.AreEqual("foo", pq.Filters.Single().FilterValue.Values.First());
            Assert.IsFalse(pq.Errors.ContainsKey("filter[0]"));
            Assert.IsTrue(pq.Errors.ContainsKey("filter[1]"));
        }

        [TestMethod]
        public async Task BindAsync_EmptyFilterValueAmongOthers_IsSkippedWithoutError()
        {
            var http = Http(qs: new[]
            {
                ("filter", "name:Equals:foo"),
                ("filter", ""),
                ("filter", "id:GreaterThan:5")
            });

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsTrue(pq.IsValid);
            Assert.AreEqual(2, pq.Filters.Count);
        }

        [TestMethod]
        public async Task BindAsync_AllowList_PermitsFilteredProperty()
        {
            var sp = WithAllowList(b => b.AllowFilter("name"));
            var http = Http(sp, ("filter", "name:Equals:foo"));

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsTrue(pq.IsValid);
            Assert.AreEqual(1, pq.Filters.Count);
        }

        [TestMethod]
        public async Task BindAsync_AllowList_RejectsForbiddenProperty()
        {
            var sp = WithAllowList(b => b.AllowFilter("name"));
            var http = Http(sp, ("filter", "secret:Equals:x"));

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsFalse(pq.IsValid);
            Assert.IsTrue(pq.Errors.ContainsKey("filter[0]"));
            StringAssert.Contains(pq.Errors["filter[0]"], "allow-list");
        }

        [TestMethod]
        public async Task BindAsync_AllowList_RejectsOnlyForbiddenRow_KeepsAllowed()
        {
            var sp = WithAllowList(b => b.AllowFilter("name"));
            var http = Http(sp,
                ("filter", "name:Equals:foo"),
                ("filter", "secret:Equals:x"));

            var pq = await PagedQueryWithFilters<SampleItem>.BindAsync(http, parameter: null);

            Assert.IsFalse(pq.IsValid);
            Assert.AreEqual(1, pq.Filters.Count);
            Assert.IsTrue(pq.Errors.ContainsKey("filter[1]"));
            Assert.IsFalse(pq.Errors.ContainsKey("filter[0]"));
        }
    }
}
