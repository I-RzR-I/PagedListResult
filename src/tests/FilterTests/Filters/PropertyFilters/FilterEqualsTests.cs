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

using System;
using System.Linq;
using FilterTests.Data;
using FilterTests.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.ResultMessage.Pagination.Core.Extensions.Filters.PropertyFilterQuery;

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

        [TestMethod]
        public void Equals_Guid_Test()
        {
            //Act
            var filtered = _fakeItems.PropertyEquals("uniqueId", "11111111-1111-1111-1111-111111111111");

            //Assert
            filtered.Count().Should().Be(1);
            filtered.Single().Id.Should().Be(0);
        }

        [TestMethod]
        public void Equals_NullableGuid_WithValue_Test()
        {
            //Act
            var filtered = _fakeItems.PropertyEquals("optionalUniqueId", "22222222-2222-2222-2222-222222222222");

            //Assert
            filtered.Count().Should().Be(1);
            filtered.Single().Id.Should().Be(1);
        }

        [TestMethod]
        public void Equals_Int_Test()
        {
            //Act
            var filtered = _fakeItems.PropertyEquals("count", "100");

            //Assert
            filtered.Count().Should().Be(1);
            filtered.Single().Id.Should().Be(2);
        }

        [TestMethod]
        public void Equals_DateTimeOffset_Test()
        {
            //Act
            var filtered = _fakeItems.PropertyEquals("createdAt", new DateTimeOffset(2010, 7, 1, 0, 0, 0, TimeSpan.Zero).ToString("o"));

            //Assert
            filtered.Count().Should().Be(1);
            filtered.Single().Id.Should().Be(1);
        }

        [TestMethod]
        public void Equals_TimeSpan_Test()
        {
            //Act
            var filtered = _fakeItems.PropertyEquals("duration", TimeSpan.FromHours(1).ToString());

            //Assert
            filtered.Count().Should().Be(1);
            filtered.Single().Id.Should().Be(1);
        }

        [TestMethod]
        public void Equals_Enum_ByName_Test()
        {
            //Act
            var filtered = _fakeItems.PropertyEquals("status", nameof(TestItemStatus.Active));

            //Assert
            filtered.Count().Should().Be(1);
            filtered.Single().Id.Should().Be(1);
        }
    }
}