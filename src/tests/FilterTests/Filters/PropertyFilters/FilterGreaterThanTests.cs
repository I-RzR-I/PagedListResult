// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-10-30 22:27
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-31 15:00
// ***********************************************************************
//  <copyright file="FilterGreaterThanTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
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
    public class FilterGreaterThanTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        [DataRow("minQuantity", "5", 1)]
        [DataRow("price", "5", 0)]
        [DataRow("price", "1", 3)]
        [DataRow("count", "100", 1)]
        [DataRow("count", "101", 0)]
        [DataRow("count", "1", 4)]
        [DataRow("date", "2010/05/01", 3)]
        [DataRow("date", "2021/07/01", 0)]
        [DataRow("date", "2111/07/01", 0)]
        [DataRow("endDate", "2111/07/01", 0)]
        [DataRow("endDate", "2011/07/01", 1)]
        public void GreaterThan_Test(string propertyName, string value, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertyGreaterThan(propertyName, value);

            //Assert
            filtered.Count().Should().Be(expected);
        }

        [TestMethod]
        [DataRow("isActive", "true", 3)]
        [DataRow("name", "true", 3)]
        public void NotSupportedContains_Test(string propertyName, string value, int expected)
        {
            try
            {
                //Act
                Assert.ThrowsException<NotSupportedException>(() => _fakeItems.PropertyGreaterThan(propertyName, value));
            }
            catch (Exception e)
            {
                //Assert
                Assert.AreEqual(typeof(NotSupportedException), e.GetType());
            }
        }
    }
}