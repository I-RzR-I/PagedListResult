// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-10-30 16:48
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-31 15:00
// ***********************************************************************
//  <copyright file="FilterNullTests.cs" company="">
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
    public class FilterNullTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        [DataRow("isBlocked", 2)]
        [DataRow("endDate", 1)]
        [DataRow("minQuantity", 1)]
        [DataRow("count", 0)]
        public void IsNull_Test(string propertyName, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertyIsNull(propertyName);

            //Assert
            filtered.Count().Should().Be(expected);
        }

        [TestMethod]
        [DataRow("isBlocked", 2)]
        [DataRow("endDate", 3)]
        [DataRow("minQuantity", 3)]
        [DataRow("count", 4)]
        public void IsNotNull_Test(string propertyName, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertyIsNotNull(propertyName);

            //Assert
            filtered.Count().Should().Be(expected);
        }
    }
}