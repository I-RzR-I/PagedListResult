// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-10-26 10:08
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-31 15:00
// ***********************************************************************
//  <copyright file="FilterContainsTests.cs" company="">
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
    public class FilterContainsTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        [DataRow("name", "Test", 3)]
        [DataRow("name", "es", 3)]
        [DataRow("name", null, 4)]
        public void Contains_Test(string propertyName, string value, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertyContains(propertyName, value);

            //Assert
            filtered.Count().Should().Be(expected);
        }

        [TestMethod]
        [DataRow("name", "Te", 3)]
        [DataRow("name", "es", 3)]
        [DataRow("name", "Tr", 1)]
        [DataRow("value", "Zer", 1)]
        [DataRow("name", "tV", 1)]
        public void CaseSensitiveContains_Test(string propertyName, string value, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertySensitiveContains(propertyName, value);

            //Assert
            filtered.Count().Should().Be(expected);
        }

        [TestMethod]
        [DataRow("name", "Test", 1)]
        [DataRow("name", "Text", 4)]
        public void DoesNotContains_Test(string propertyName, string value, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertyDoesNotContains(propertyName, value);

            //Assert
            filtered.Count().Should().Be(expected);
        }

        [TestMethod]
        [DataRow("name", "Test", 1)]
        [DataRow("name", "Te", 1)]
        [DataRow("name", "tV", 3)]
        public void CaseSensitiveDoesNotContains_Test(string propertyName, string value, int expected)
        {
            //Act
            var filtered = _fakeItems.PropertySensitiveDoesNotContains(propertyName, value);

            //Assert
            filtered.Count().Should().Be(expected);
        }

        [TestMethod]
        [DataRow("isActive", "true", 3)]
        public void NotSupportedContains_Test(string propertyName, string value, int expected)
        {
            try
            {
                //Act
                Assert.ThrowsException<NotSupportedException>(() => _fakeItems.PropertyContains(propertyName, value));
            }
            catch (Exception e)
            {
                //Assert
                Assert.AreEqual(typeof(NotSupportedException), e.GetType());
            }
        }
    }
}