// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-11-02 16:24
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-02 16:24
// ***********************************************************************
//  <copyright file="SourceFilterTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using FilterTests.Data;
using FilterTests.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResult.Common.Extensions.Filters;
using System.Linq;

namespace FilterTests.Filters
{
    [TestClass]
    public class SourceTextSearchFilterTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        [DataRow("temp", null, 0)]
        [DataRow("test", null, 3)]
        [DataRow("1", null, 1)]
        [DataRow("test", new string[] { "" }, 4)]
        [DataRow("test1", new string[] { "name" }, 1)]
        [DataRow("test1", new string[] { "value" }, 1)]
        public void FilterSourceByText_Test(string searchText, string[] searchProperties, int expected)
        {
            //Act
            var filtered = _fakeItems.FilterSourceByText(true, false, searchText, searchProperties);

            //Assert
            filtered.Count().Should().Be(expected);
        }

        [TestMethod]
        [DataRow("temp", null, 0)]
        [DataRow("test", null, 3)]
        [DataRow("1", null, 4)]
        [DataRow("ZeroTwo", null, 1)]
        [DataRow("test", new string[] { "" }, 4)]
        [DataRow("test1", new string[] { "name" }, 1)]
        [DataRow("test1", new string[] { "value" }, 1)]
        [DataRow("1", new string[] { "price" }, 1)]
        [DataRow("1", new string[] { "price", "count" }, 4)]
        [DataRow("1", new string[] { "date", "endDate" }, 4)]
        //[DataRow("111", new string[] { "date", "endDate" }, 1)] > 111 should be found, depending on the current culture
        [DataRow("21", new string[] { "date", "endDate" }, 1)]
        public void FilterSourceByText_InAllFields_Test(string searchText, string[] searchProperties, int expected)
        {
            //Act
            var filtered = _fakeItems.FilterSourceByText(false, true, searchText, searchProperties);

            //Assert
            filtered.Count().Should().Be(expected);
        }
    }
}