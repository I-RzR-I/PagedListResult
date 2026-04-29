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
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResultWebTests.Stubs;
using RzR.ResultMessage.Pagination.Abstractions.Enums;
using RzR.ResultMessage.Pagination.Abstractions.Models.Request.Page;
using RzR.ResultMessage.Pagination.AspNetCore.Builders;
using RzR.ResultMessage.Pagination.AspNetCore.Helpers;
using RzR.ResultMessage.Pagination.AspNetCore.Models;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class PagedRequestQueryParserTests
    {
        private static readonly PagedListResultWebOptions Options = new()
        {
            MaxPageSize = 100,
            DefaultPageSize = 10
        };

        private static IQueryCollection QueryCollection(params (string k, string v)[] pairs)
        {
            var d = new Dictionary<string, StringValues>();
            foreach (var (k, v) in pairs)
            {
                if (d.TryGetValue(k, out var existing))
                    d[k] = StringValues.Concat(existing, v);
                else
                    d[k] = v;
            }
            return new QueryCollection(d);
        }

        [TestMethod]
        public void Page_PageSize_Parsed()
        {
            var req = new PagedRequest();
            var result = PagedRequestQueryParser.Populate(req, QueryCollection(("page", "3"), ("pageSize", "20")), Options);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(3, req.Page);
            Assert.AreEqual(20, req.PageSize);
        }

        [TestMethod]
        public void PageSize_OverMax_ProducesError()
        {
            var req = new PagedRequest();
            var result = PagedRequestQueryParser.Populate(req, QueryCollection(("pageSize", "9999")), Options);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.ContainsKey("pageSize"));
            StringAssert.Contains(result.Errors["pageSize"], "MaxPageSize");
        }

        [TestMethod]
        public void Order_Parsed_AscDescCaseInsensitive()
        {
            var req = new PagedRequest();
            var result = PagedRequestQueryParser.Populate(req, QueryCollection(("order", "Name:DESC")), Options);

            Assert.IsTrue(result.IsValid, string.Join(";", result.Errors.Select(e => e.Key + "=" + e.Value)));
            Assert.AreEqual("Name", req.Order.OrderByProperty);
            Assert.AreEqual(OrderDirection.Desc, req.Order.OrderDirection);
        }

        [TestMethod]
        public void Order_MultiKey_FirstHonored_ExtrasWarn()
        {
            var req = new PagedRequest();
            var result = PagedRequestQueryParser.Populate(req, QueryCollection(("order", "name:asc,createdOn:desc")), Options);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual("name", req.Order.OrderByProperty);
            Assert.AreEqual(1, result.Warnings.Count);
            StringAssert.Contains(result.Warnings[0], "createdOn");
        }

        [TestMethod]
        public void Order_NotInAllowList_Errors()
        {
            var req = new PagedRequest();
            var allow = new PageableMetadataBuilder<SampleItem>().AllowSort("id", "name").Build();

            var result = PagedRequestQueryParser.Populate(req, QueryCollection(("order", "secret:asc")), Options, allow);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.ContainsKey("order"));
        }

        [TestMethod]
        public void Filter_Parsed_RepeatableAndCsvValues()
        {
            var req = new PageRequestWithFilters();
            var result = PagedRequestQueryParser.Populate(
                req,
                QueryCollection(("filter", "status:Equals:active"), ("filter", "tag:IsIn:a,b,c")),
                Options);

            Assert.IsTrue(result.IsValid, string.Join(";", result.Errors.Select(e => e.Key + "=" + e.Value)));
            Assert.AreEqual(2, req.Filters.Count);

            var statusFilter = req.Filters.First(f => f.FilterValue.PropertyName == "status");
            Assert.AreEqual(FilterType.Equals, statusFilter.FilterValue.Condition);
            CollectionAssert.AreEquivalent(new[] { "active" }, statusFilter.FilterValue.Values.ToArray());

            var tagFilter = req.Filters.First(f => f.FilterValue.PropertyName == "tag");
            Assert.AreEqual(FilterType.IsIn, tagFilter.FilterValue.Condition);
            CollectionAssert.AreEquivalent(new[] { "a", "b", "c" }, tagFilter.FilterValue.Values.ToArray());
        }

        [TestMethod]
        public void Filter_UnknownCondition_Errors()
        {
            var req = new PageRequestWithFilters();
            var result = PagedRequestQueryParser.Populate(req, QueryCollection(("filter", "name:bogus:x")), Options);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.ContainsKey("filter[0]"));
        }

        [TestMethod]
        public void Filter_BadShape_Errors()
        {
            var req = new PageRequestWithFilters();
            var result = PagedRequestQueryParser.Populate(req, QueryCollection(("filter", "no-colons-here")), Options);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.ContainsKey("filter[0]"));
        }

        [TestMethod]
        public void Filter_PropertyNotInAllowList_Errors()
        {
            var req = new PageRequestWithFilters();
            var allow = new PageableMetadataBuilder<SampleItem>().AllowFilter("name").Build();
            var result = PagedRequestQueryParser.Populate(req, QueryCollection(("filter", "secret:Equals:x")), Options, allow);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.ContainsKey("filter[0]"));
        }

        [TestMethod]
        public void Filter_Between_LeftAndRightSplitAcrossValuesAndCompareValue()
        {
            var req = new PageRequestWithFilters();
            var result = PagedRequestQueryParser.Populate(req, QueryCollection(("filter", "price:Between:10,20")), Options);

            Assert.IsTrue(result.IsValid, string.Join(";", result.Errors.Select(e => e.Key + "=" + e.Value)));
            Assert.AreEqual(1, req.Filters.Count);

            var f = req.Filters.Single().FilterValue;
            Assert.AreEqual(FilterType.Between, f.Condition);
            CollectionAssert.AreEquivalent(new[] { "10" }, f.Values.ToArray());
            Assert.AreEqual("20", f.CompareValue);
        }

        [TestMethod]
        public void Filter_Between_MissingRightValue_Errors()
        {
            var req = new PageRequestWithFilters();
            var result = PagedRequestQueryParser.Populate(req, QueryCollection(("filter", "price:Between:10")), Options);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.ContainsKey("filter[0]"));
        }

        [TestMethod]
        public void Filter_TwoBadFilters_ProducesTwoDistinctErrorKeys()
        {
            var req = new PageRequestWithFilters();
            var result = PagedRequestQueryParser.Populate(
                req,
                QueryCollection(("filter", "name:bogus:x"), ("filter", "no-colons-here")),
                Options);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.ContainsKey("filter[0]"));
            Assert.IsTrue(result.Errors.ContainsKey("filter[1]"));
            Assert.AreNotEqual(result.Errors["filter[0]"], result.Errors["filter[1]"]);
        }

        [TestMethod]
        public void Filter_NonBetween_LeavesCompareValueNull()
        {
            var req = new PageRequestWithFilters();
            var result = PagedRequestQueryParser.Populate(
                req,
                QueryCollection(("filter", "price:GreaterThan:50"), ("filter", "tag:IsIn:a,b")),
                Options);

            Assert.IsTrue(result.IsValid);

            var gt = req.Filters.First(f => f.FilterValue.PropertyName == "price").FilterValue;
            Assert.IsNull(gt.CompareValue);
            CollectionAssert.AreEquivalent(new[] { "50" }, gt.Values.ToArray());

            var isIn = req.Filters.First(f => f.FilterValue.PropertyName == "tag").FilterValue;
            Assert.IsNull(isIn.CompareValue);
            CollectionAssert.AreEquivalent(new[] { "a", "b" }, isIn.Values.ToArray());
        }

        [TestMethod]
        public void Filter_IsNull_TakesNoValue()
        {
            var req = new PageRequestWithFilters();
            var result = PagedRequestQueryParser.Populate(req, QueryCollection(("filter", "name:IsNull:")), Options);

            Assert.IsTrue(result.IsValid, string.Join(";", result.Errors.Select(e => e.Key + "=" + e.Value)));
            var f = req.Filters.Single().FilterValue;
            Assert.AreEqual(FilterType.IsNull, f.Condition);
            Assert.AreEqual(0, f.Values.Count);
            Assert.IsNull(f.CompareValue);
        }

        [TestMethod]
        public void Search_AllVariants_Parsed()
        {
            var req = new PagedRequest();
            var r1 = PagedRequestQueryParser.Populate(req, QueryCollection(("search", "abc"), ("searchAll", "fields")), Options);
            Assert.IsTrue(r1.IsValid);
            Assert.AreEqual("abc", req.Search.Search);
            Assert.IsFalse(req.Search.SearchInAllTextFields);
            Assert.IsTrue(req.Search.SearchInAllFields);

            var r2 = PagedRequestQueryParser.Populate(new PagedRequest(), QueryCollection(("searchAll", "weird")), Options);
            Assert.IsFalse(r2.IsValid);
            Assert.IsTrue(r2.Errors.ContainsKey("searchAll"));
        }

        [TestMethod]
        public void Fields_Predefined_Parsed()
        {
            var req = new PagedRequest();
            var result = PagedRequestQueryParser.Populate(
                req,
                QueryCollection(("fields", "id,name"),
                  ("predefinedField", "Id"),
                  ("predefinedRecords", "1,2,3")),
                Options);

            Assert.IsTrue(result.IsValid);
            CollectionAssert.AreEquivalent(new[] { "id", "name" }, req.Fields.ToArray());
            Assert.AreEqual("Id", req.PredefinedRecord.PredefinedFieldName);
            CollectionAssert.AreEquivalent(new[] { "1", "2", "3" }, req.PredefinedRecord.PredefinedRecords.ToArray());
        }

        [TestMethod]
        public void NonIntPage_Errors()
        {
            var req = new PagedRequest();
            var result = PagedRequestQueryParser.Populate(req, QueryCollection(("page", "abc")), Options);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.ContainsKey("page"));
        }

        [TestMethod]
        public void EmptyQuery_LeavesDefaults()
        {
            var req = new PagedRequest();
            var result = PagedRequestQueryParser.Populate(req, QueryCollection(), Options);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(1, req.Page);
            Assert.AreEqual(10, req.PageSize);
        }
    }
}
