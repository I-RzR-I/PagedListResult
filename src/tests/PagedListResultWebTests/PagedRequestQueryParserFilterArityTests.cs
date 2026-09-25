#region U S A G E S

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.ResultMessage.Pagination.Abstractions.Enums;
using RzR.ResultMessage.Pagination.Abstractions.Models.Request.Page;
using RzR.ResultMessage.Pagination.AspNetCore.Helpers;
using RzR.ResultMessage.Pagination.AspNetCore.Models;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class PagedRequestQueryParserFilterArityTests
    {
        private const string ShapeHint = "Expected '<property>:<condition>:<value[,value2,...]>'.";

        private const string FirstFilterErrorKey = "filter[0]";

        private static readonly PagedListResultWebOptions Options = new()
        {
            MaxPageSize = 100,
            DefaultPageSize = 10
        };

        private static IQueryCollection FilterQuery(string filter)
            => new QueryCollection(new Dictionary<string, StringValues> { ["filter"] = filter });

        private static PagedParseResult ParseFilter(PageRequestWithFilters request, string filter)
            => PagedRequestQueryParser.Populate(request, FilterQuery(filter), Options);

        private static string Describe(PagedParseResult result)
            => string.Join(";", result.Errors.Select(e => e.Key + "=" + e.Value));

        [TestMethod]
        public void Populate_ValuelessIsNullFilter_ParsesWithEmptyValues_Test()
        {
            var request = new PageRequestWithFilters();

            var result = ParseFilter(request, "name:IsNull");

            Assert.IsTrue(result.IsValid, Describe(result));
            var filter = request.Filters.Single().FilterValue;
            Assert.AreEqual(FilterType.IsNull, filter.Condition);
            Assert.AreEqual("name", filter.PropertyName);
            Assert.AreEqual(0, filter.Values.Count, "A value-less condition must carry no values.");
            Assert.IsNull(filter.CompareValue, "A value-less condition must carry no compare value.");
        }

        [TestMethod]
        public void Populate_ValuelessIsNotNullFilter_ParsesWithEmptyValues_Test()
        {
            var request = new PageRequestWithFilters();

            var result = ParseFilter(request, "name:IsNotNull");

            Assert.IsTrue(result.IsValid, Describe(result));
            var filter = request.Filters.Single().FilterValue;
            Assert.AreEqual(FilterType.IsNotNull, filter.Condition);
            Assert.AreEqual("name", filter.PropertyName);
            Assert.AreEqual(0, filter.Values.Count, "A value-less condition must carry no values.");
            Assert.IsNull(filter.CompareValue, "A value-less condition must carry no compare value.");
        }

        [TestMethod]
        public void Populate_LowercaseValuelessCondition_ParsesCaseInsensitively_Test()
        {
            var request = new PageRequestWithFilters();

            var result = ParseFilter(request, "name:isnull");

            Assert.IsTrue(result.IsValid, Describe(result));
            Assert.AreEqual(FilterType.IsNull, request.Filters.Single().FilterValue.Condition);
        }

        [TestMethod]
        public void Populate_TrailingColonValuelessFilter_StillParses_Test()
        {
            var request = new PageRequestWithFilters();

            var result = ParseFilter(request, "name:IsNull:");

            Assert.IsTrue(result.IsValid, Describe(result));
            var filter = request.Filters.Single().FilterValue;
            Assert.AreEqual(FilterType.IsNull, filter.Condition);
            Assert.AreEqual(0, filter.Values.Count, "The legacy trailing-colon form must stay value-less.");
        }

        [TestMethod]
        public void Populate_EqualsWithoutValueSegment_ReportsShapeError_Test()
        {
            var request = new PageRequestWithFilters();

            var result = ParseFilter(request, "name:Equals");

            Assert.IsFalse(result.IsValid, "A value-requiring condition must still reject the two-part form.");
            Assert.AreEqual(0, request.Filters.Count);
            StringAssert.Contains(result.Errors[FirstFilterErrorKey], "Bad filter 'name:Equals'.");
            StringAssert.Contains(result.Errors[FirstFilterErrorKey], ShapeHint);
        }

        [TestMethod]
        public void Populate_BetweenWithoutValueSegment_ReportsShapeError_Test()
        {
            var request = new PageRequestWithFilters();

            var result = ParseFilter(request, "price:Between");

            Assert.IsFalse(result.IsValid, "A value-requiring condition must still reject the two-part form.");
            Assert.AreEqual(0, request.Filters.Count);
            StringAssert.Contains(result.Errors[FirstFilterErrorKey], "Bad filter 'price:Between'.");
            StringAssert.Contains(result.Errors[FirstFilterErrorKey], ShapeHint);
        }

        [TestMethod]
        public void Populate_ValuelessFilterWithoutProperty_ReportsPropertyRequired_Test()
        {
            var request = new PageRequestWithFilters();

            var result = ParseFilter(request, ":IsNull");

            Assert.IsFalse(result.IsValid);
            StringAssert.Contains(result.Errors[FirstFilterErrorKey], "Property name is required.");
            Assert.IsFalse(
                result.Errors[FirstFilterErrorKey].Contains(ShapeHint),
                "The property check now runs before the value-segment check, so the shape hint must not be reported here.");
        }

        [TestMethod]
        public void Populate_UnknownConditionWithoutValue_ReportsUnknownCondition_Test()
        {
            var request = new PageRequestWithFilters();

            var result = ParseFilter(request, "name:bogus");

            Assert.IsFalse(result.IsValid);
            StringAssert.Contains(result.Errors[FirstFilterErrorKey], "Unknown filter condition 'bogus'.");
            Assert.IsFalse(
                result.Errors[FirstFilterErrorKey].Contains(ShapeHint),
                "The condition is now parsed before the value-segment check, so the shape hint must not be reported here.");
        }
    }
}
