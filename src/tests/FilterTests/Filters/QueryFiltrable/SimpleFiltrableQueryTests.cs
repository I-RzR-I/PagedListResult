// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-11-04 15:56
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-06 23:42
// ***********************************************************************
//  <copyright file="SimpleFiltrableQueryTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using FilterTests.Data;
using FilterTests.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResult.Common.Enums;
using PagedListResult.Common.Extensions.Filters;
using PagedListResult.Common.Models.Request;

#endregion

namespace FilterTests.Filters.QueryFiltrable
{
    [TestClass]
    public class SimpleFiltrableQueryTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        public void AsSimpleFilterable_Price_Equals_3_Should_2_Test()
        {
            //Act
            var filtered = _fakeItems.AsSimpleFilterable(new List<DataFilter>
            {
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "price", Values = new List<string> { "3" }, Condition = FilterType.Equals } }
            });

            //Assert
            filtered.Count().Should().Be(2);
        }

        [TestMethod]
        public void AsSimpleFilterable_Price_Equals_3_And_IsActive_Equal_True_Should_1_Test()
        {
            //Act
            var filtered = _fakeItems.AsSimpleFilterable(new List<DataFilter>
            {
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "price", Values = new List<string> { "3" }, Condition = FilterType.Equals } },
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "isActive", Values = new List<string> { "true" }, Condition = FilterType.Equals } }
            });

            //Assert
            filtered.Count().Should().Be(1);
        }

        [TestMethod]
        public void AsSimpleFilterable_Name_SensitiveContains_Test_And_IsActive_Equal_True_Should_1_Test()
        {
            //Act
            var filtered = _fakeItems.AsSimpleFilterable(new List<DataFilter>
            {
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "name", Values = new List<string> { "Test" }, Condition = FilterType.SensitiveContains } },
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "isActive", Values = new List<string> { "true" }, Condition = FilterType.Equals } }
            });

            //Assert
            filtered.Count().Should().Be(1);
        }

        [TestMethod]
        public void AsSimpleFilterable_Name_SensitiveContains_Test_And_MinQuantity_IsNull_Should_1_Test()
        {
            //Act
            var filtered = _fakeItems.AsSimpleFilterable(new List<DataFilter>
            {
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "name", Values = new List<string> { "Test" }, Condition = FilterType.SensitiveContains } },
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "minQuantity", Condition = FilterType.IsNull } }
            });

            //Assert
            filtered.Count().Should().Be(1);
        }

        [TestMethod]
        public void AsSimpleFilterable_Name_SensitiveContains_Test_And_MinQuantity_IsNotNull_Should_2_Test()
        {
            //Act
            var filtered = _fakeItems.AsSimpleFilterable(new List<DataFilter>
            {
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "name", Values = new List<string> { "Test" }, Condition = FilterType.SensitiveContains } },
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "minQuantity", Condition = FilterType.IsNotNull } }
            });

            //Assert
            filtered.Count().Should().Be(2);
        }

        [TestMethod]
        public void AsSimpleFilterable_Name_SensitiveContains_Test_And_MinQuantity_IsNotNull_And_Count_Between_90_120_Should_1_Test()
        {
            //Act
            var filtered = _fakeItems.AsSimpleFilterable(new List<DataFilter>
            {
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "name", Values = new List<string> { "Test" }, Condition = FilterType.SensitiveContains } },
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "minQuantity", Condition = FilterType.IsNotNull } },
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "count", Values = new List<string> { "90" }, CompareValue = "120", Condition = FilterType.Between } }
            });

            //Assert
            filtered.Count().Should().Be(1);
        }

        [TestMethod]
        public void AsSimpleFilterable_Name_SensitiveContains_Test_And_MinQuantity_IsNotNull_And_Count_Between_90_120_And_IsActive_Equals_true_Should_0_Test()
        {
            //Act
            var filtered = _fakeItems.AsSimpleFilterable(new List<DataFilter>
            {
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "name", Values = new List<string> { "Test" }, Condition = FilterType.SensitiveContains } },
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "minQuantity", Condition = FilterType.IsNotNull } },
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "count", Values = new List<string> { "90" }, CompareValue = "120", Condition = FilterType.Between } },
                new DataFilter { FilterValue = new DataFilterValue { PropertyName = "isActive", Values = new List<string> { "true" }, Condition = FilterType.Equals } }
            });

            //Assert
            filtered.Count().Should().Be(0);
        }
    }
}