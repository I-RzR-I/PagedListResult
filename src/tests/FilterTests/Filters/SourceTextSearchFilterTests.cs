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

using System.Linq;
using FilterTests.Data;
using FilterTests.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResult.Common.Extensions.Filters;

namespace FilterTests.Filters
{
    [TestClass]
    public class SourceTextSearchFilterTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        [DataRow(true, "test", null, 3)]
        [DataRow(false, "test", new string[] { "" }, 4)]
        [DataRow(false, "test1", new string[] { "name" }, 1)]
        [DataRow(false, "test1", new string[] { "value" }, 1)]
        public void FilterSourceByText_Test(bool searchInAllTextFields, string searchText, string[] searchProperties, int expected)
        {
            //Act
            var filtered = _fakeItems.FilterSourceByText(searchInAllTextFields, searchText, searchProperties);

            //Assert
            filtered.Count().Should().Be(expected);
        }
    }
}