// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-10-31 14:57
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-31 15:00
// ***********************************************************************
//  <copyright file="FilterLessThanOrEqualsTests.cs" company="">
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
    public class FilterLessThanOrEqualsTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        [DataRow("minQuantity", "5", 3)]
        [DataRow("price", "5", 4)]
        [DataRow("price", "1", 1)]
        [DataRow("count", "100", 3)]
        [DataRow("count", "101", 4)]
        [DataRow("count", "1", 0)]
        [DataRow("date", "2010/05/01", 1)]
        [DataRow("date", "2021/07/01", 4)]
        [DataRow("date", "2111/07/01", 4)]
        [DataRow("endDate", "2111/07/01", 4)]
        [DataRow("endDate", "2011/07/01", 3)]
        public void LessThanOrEquals_Test(string propertyName, string value, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertyLessThanOrEquals(propertyName, value);

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
                Assert.ThrowsException<NotSupportedException>(() => _fakeItems.PropertyLessThanOrEquals(propertyName, value));
            }
            catch (Exception e)
            {
                //Assert
                Assert.AreEqual(typeof(NotSupportedException), e.GetType());
            }
        }
    }
}