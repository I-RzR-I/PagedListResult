// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-10-30 08:50
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-31 15:00
// ***********************************************************************
//  <copyright file="FilterInNotInTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Linq;
using FilterTests.Data;
using FilterTests.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResult.Common.Extensions.Filters.PropertyFilterQuery;

#endregion

namespace FilterTests.Filters.PropertyFilters
{
    [TestClass]
    public class FilterInNotInTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        [DataRow("count", 2, new string[] { "5", "10", "4" })]
        [DataRow("price", 3, new string[] { "-10", "10", "2", "3" })]
        [DataRow("name", 2, new string[] { "Test", "10", "TrustValue", "3" })]
        [DataRow("minQuantity", 2, new string[] { "1", "10", "32", "3" })]
        [DataRow("minQuantity", 4, new string[] { "1", "10", "32", null })]
        [DataRow("retailPrice", 1, new string[] { "1", "10", "32", "1.2" })]
        [DataRow("retailPrice", 4, new string[] { "1", "10", "32", null })]
        [DataRow("retailPrice", 4, new string[] { "1", "10", "32", "" })]
        [DataRow("isBlocked", 1, new string[] { "true" })]
        [DataRow("isActive", 2, new string[] { "false" })]
        [DataRow("date", 1, new string[] { "2010/07/01" })]
        [DataRow("date", 2, new string[] { "2010/07/01", "2010/05/01" })]
        [DataRow("date", 0, new string[] { "2111/07/01", "2010/05/10" })]
        [DataRow("endDate", 2, new string[] { "2111/07/01", "2010/05/10" })]
        public void IsIn_Test(string propertyName, int expected, string[] values)
        {
            //Act
            var filtered = _fakeItems.PropertyIsIn(propertyName, values);

            //Assert
            filtered.Count().Should().Be(expected);
        }

        [TestMethod]
        [DataRow("count", 2, new string[] { "5", "10", "4" })]
        [DataRow("price", 1, new string[] { "-10", "10", "2", "3" })]
        [DataRow("name", 2, new string[] { "Test", "10", "TrustValue", "3" })]
        [DataRow("minQuantity", 2, new string[] { "1", "10", "32", "3" })]
        [DataRow("minQuantity", 4, new string[] { "1", "10", "32", null })]
        [DataRow("retailPrice", 3, new string[] { "1", "10", "32", "1.2" })]
        [DataRow("retailPrice", 4, new string[] { "1", "10", "32", null })]
        [DataRow("retailPrice", 4, new string[] { "1", "10", "32", "" })]
        [DataRow("isBlocked", 3, new string[] { "true" })]
        [DataRow("isActive", 2, new string[] { "false" })]
        [DataRow("date", 3, new string[] { "2010/07/01" })]
        [DataRow("date", 2, new string[] { "2010/07/01", "2010/05/01" })]
        [DataRow("date", 4, new string[] { "2111/07/01", "2010/05/10" })]
        [DataRow("endDate", 2, new string[] { "2111/07/01", "2010/05/10" })]
        public void IsNotIn_Test(string propertyName, int expected, string[] values)
        {
            //Act
            var filtered = _fakeItems.PropertyIsNotIn(propertyName, values);

            //Assert
            filtered.Count().Should().Be(expected);
        }
    }
}