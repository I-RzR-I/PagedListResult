// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-10-30 08:50
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-31 15:00
// ***********************************************************************
//  <copyright file="FilterBetweenTests.cs" company="">
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
    public class FilterBetweenTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        [DataRow("count", "5", "20", 2)]
        [DataRow("count", "10", "100", 2)]
        [DataRow("minQuantity", "0", "1", 2)]
        [DataRow("minQuantity", "-1", "0", 1)]
        [DataRow("minQuantity", "5", "7", 0)]
        [DataRow("minQuantity", "0", "11", 3)]
        [DataRow("minQuantity", "0", "10", 3)]
        [DataRow("minQuantity", "1", "10", 2)]
        [DataRow("date", "2010/05/01", "2011/07/01", 3)]
        [DataRow("date", "2010/06/01", "2011/07/01", 2)]
        [DataRow("endDate", "2010/06/01", "2011/07/01", 1)]
        [DataRow("endDate", "2010/05/01", "2011/07/01", 2)]
        public void Between_Test(string propertyName, string leftValue, string rightValue, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertyBetween(propertyName, leftValue, rightValue);

            //Assert
            filtered.Count().Should().Be(expected);
        }
    }
}