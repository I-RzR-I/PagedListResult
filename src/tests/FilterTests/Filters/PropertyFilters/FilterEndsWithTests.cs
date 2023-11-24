// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-10-28 21:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-31 15:00
// ***********************************************************************
//  <copyright file="FilterEndsWithTests.cs" company="">
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
    public class FilterEndsWithTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        [DataRow("name", "Test", 1)]
        [DataRow("name", "es", 0)]
        [DataRow("name", "Value", 1)]
        [DataRow("name", "1", 1)]
        public void EndsWith_Test(string propertyName, string value, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertyEndsWith(propertyName, value);

            //Assert
            filtered.Count().Should().Be(expected);
        }

        [TestMethod]
        [DataRow("name", "Test", 3)]
        [DataRow("name", "Text", 4)]
        [DataRow("name", "Value", 3)]
        [DataRow("name", "value", 4)]
        public void DoesNotEndsWith_est(string propertyName, string value, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertyDoesNotEndsWith(propertyName, value);

            //Assert
            filtered.Count().Should().Be(expected);
        }
    }
}