// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-11-02 21:19
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-02 21:27
// ***********************************************************************
//  <copyright file="SourceTopRecordFilterTests.cs" company="">
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
using PagedListResult.Common.Extensions.Filters;

#endregion

namespace FilterTests.Filters
{
    [TestClass]
    public class SourceTopRecordFilterTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        [DataRow(new[] { "0" }, "Id", 1)]
        [DataRow(new[] { "0", "1" }, "Id", 2)]
        [DataRow(new[] { "0", "1", "10" }, "Id", 2)]
        [DataRow(new[] { "0", "1", "10" }, null, 2)]
        public void GetPredefinedRecordsInTop_Test(string[] ids, string defaultPk, int expected)
        {
            //Act
            var filtered = _fakeItems.GetInTopPredefinedRecords(ids, new[] { defaultPk });

            //Assert
            filtered.Count().Should().Be(expected);
        }

        [TestMethod]
        [DataRow(new[] { "0" }, "Id", 3)]
        [DataRow(new[] { "0", "1" }, "Id", 2)]
        [DataRow(new[] { "0", "1", "10" }, "Id", 2)]
        public void GetNotInTopPredefinedRecords_Test(string[] ids, string defaultPk, int expected)
        {
            //Act
            var filtered = _fakeItems.GetNotInTopPredefinedRecords(ids, new[] { defaultPk });

            //Assert
            filtered.Count().Should().Be(expected);
        }
    }
}