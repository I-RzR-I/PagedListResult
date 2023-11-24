// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-10-30 08:50
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-31 15:00
// ***********************************************************************
//  <copyright file="FilterEqualsTests.cs" company="">
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
    public class FilterEqualsTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        [DataRow("name", "Test", 1)]
        [DataRow("isActive", "True", 2)]
        [DataRow("isBlocked", "True", 1)]
        [DataRow("isBlocked", "False", 1)]
        [DataRow("isBlocked", null, 2)]
        [DataRow("date", "2010/05/01", 1)]
        [DataRow("endDate", "2010/05/01", 0)]
        [DataRow("endDate", "2111/07/01", 1)]
        [DataRow("endDate", null, 1)]
        [DataRow("price", "3", 2)]
        public void Equals_Test(string propertyName, string value, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertyEquals(propertyName, value);

            //Assert
            filtered.Count().Should().Be(expected);
        }

        [TestMethod]
        [DataRow("name", "Test", 3)]
        [DataRow("isActive", "True", 2)]
        [DataRow("isBlocked", "True", 3)]
        [DataRow("isBlocked", "False", 3)]
        [DataRow("isBlocked", null, 2)]
        [DataRow("date", "2010/05/01", 3)]
        [DataRow("endDate", "2010/05/01", 4)]
        [DataRow("endDate", "2111/07/01", 3)]
        [DataRow("endDate", null, 3)]
        public void NotEquals_Test(string propertyName, string value, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertyNotEquals(propertyName, value);

            //Assert
            filtered.Count().Should().Be(expected);
        }
    }
}