// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-11-02 17:34
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-02 17:34
// ***********************************************************************
//  <copyright file="SourceOrderTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System.Linq;
using FilterTests.Data;
using FilterTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResult.Common.Extensions.Filters;
using PagedListResult.DataModels.Enums;

namespace FilterTests.Filters
{
    [TestClass]
    public class SourceOrderTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        public void OrderByWithDirection_1_Test()
        {
            //Act
            var filtered = _fakeItems.OrderByWithDirection("Id", OrderDirection.Desc);

            //Assert
            Assert.IsNotNull(filtered.FirstOrDefault());
            Assert.AreEqual(filtered.FirstOrDefault()!.Id, 
                _fakeItems.OrderByDescending(x => x.Id).FirstOrDefault()!.Id);
        }

        [TestMethod]
        public void OrderByWithDirection_2_Test()
        {
            //Act
            var filtered = _fakeItems.OrderByWithDirection("Count", OrderDirection.Desc);

            //Assert
            Assert.IsNotNull(filtered.FirstOrDefault());
            Assert.AreEqual(filtered.FirstOrDefault()!.Count,
                _fakeItems.OrderByDescending(x => x.Count).FirstOrDefault()!.Count);
        }

        [TestMethod]
        public void OrderByWithDirection_3_Test()
        {
            //Act
            var filtered = _fakeItems.OrderByWithDirection("MinQuantity", OrderDirection.Asc);

            //Assert
            Assert.IsNotNull(filtered.FirstOrDefault());
            Assert.AreEqual(filtered.FirstOrDefault()!.MinQuantity, 
                _fakeItems.OrderBy(x => x.MinQuantity).FirstOrDefault()!.MinQuantity);
        }

        [TestMethod]
        public void OrderByWithDirectionByDefaultPropAttribute_Test()
        {
            //Act
            var filtered = _fakeItems.OrderByWithDirectionByDefaultProperty(OrderDirection.Desc);

            //Assert
            Assert.IsNotNull(filtered.FirstOrDefault());
            Assert.AreEqual(filtered.FirstOrDefault()!.Id, 
                _fakeItems.OrderByDescending(x => x.Id).FirstOrDefault()!.Id);
        }
    }
}