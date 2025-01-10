// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultCommonDataModelTests
//  Author           : RzR
//  Created On       : 2024-12-27 13:56
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-27 13:56
// ***********************************************************************
//  <copyright file="PagedResultTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResult.DataModels.Models.Result;
using PagedListResultCommonDataModelTests.Data;
using PagedListResultCommonDataModelTests.Models;
using System;
using System.Collections.Generic;

namespace PagedListResultCommonDataModelTests.Result
{
    [TestClass]
    public class PagedResultTests
    {
        private int _rowCount;
        private DateTime _currentDateTime;
        private IList<TempDataDto> _localInfo;

        [TestInitialize]
        public void Init()
        {
            _rowCount = MockTempData.TempData1.Count;
            _currentDateTime = DateTime.Now;
            _localInfo = MockTempData.TempData1;
        }

        [TestMethod]
        public void PagedResultDefaultTest()
        {
            var obj = new PagedResult<TempDataDto>();

            Assert.IsNotNull(obj);
            Assert.IsNull(obj.Response);
            Assert.AreEqual(0, obj.PageCount);
            Assert.AreEqual(0, obj.CurrentPage);
            Assert.AreEqual(0, obj.PageSize);
            Assert.AreEqual(0, obj.RowCount);

            Assert.IsFalse(obj.HasNextPage);
            Assert.IsFalse(obj.HasPreviousPage);
            
            Assert.IsNotNull(obj.ExecutionDetails);
            Assert.IsNotNull(obj.ExecutionDetails.ExecutionTimeMs);
            Assert.IsNotNull(obj.ExecutionDetails.ExecutionDate);
            Assert.AreEqual(-1, obj.ExecutionDetails.ExecutionTimeMs);
            Assert.AreEqual(DateTime.Now.Date, obj.ExecutionDetails.ExecutionDate.Value.Date);
        }

        [TestMethod]
        public void PagedResultWithDataTest()
        {
            var execDateTime = DateTime.Now;
            var obj = new PagedResult<TempDataDto>()
            {
                //IsSuccess = true,
                Response = MockTempData.TempData1,
                RowCount = MockTempData.TempData1.Count,
                PageSize = 5,
                PageCount = 2,
                CurrentPage = 1,
                ExecutionDetails = new PagedExecDetailsResult(150, execDateTime),
                //Messages = null
            };

            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.Response);

            //Assert.IsNull(obj.Messages);

            Assert.AreEqual(2, obj.PageCount);
            Assert.AreEqual(1, obj.CurrentPage);
            Assert.AreEqual(5, obj.PageSize);
            Assert.AreEqual(11, obj.RowCount);

            Assert.IsTrue(obj.HasNextPage);
            Assert.IsFalse(obj.HasPreviousPage);
            
            Assert.IsNotNull(obj.ExecutionDetails);
            Assert.IsNotNull(obj.ExecutionDetails.ExecutionTimeMs);
            Assert.IsNotNull(obj.ExecutionDetails.ExecutionDate);
            Assert.AreEqual(150, obj.ExecutionDetails.ExecutionTimeMs);
            Assert.AreEqual(execDateTime, obj.ExecutionDetails.ExecutionDate);
        }

        [TestMethod]
        public void PagedResultWithDataHasPrevNextPageTest()
        {
            var obj = new PagedResult<TempDataDto>()
            {
                //IsSuccess = true,
                Response = _localInfo,
                RowCount = _rowCount,
                PageSize = 2,
                PageCount = 6,
                CurrentPage = 3,
                ExecutionDetails = new PagedExecDetailsResult(150, _currentDateTime),
               // Messages = null
            };

            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.Response);

            //Assert.IsNull(obj.Messages);

            Assert.AreEqual(6, obj.PageCount);
            Assert.AreEqual(3, obj.CurrentPage);
            Assert.AreEqual(2, obj.PageSize);
            Assert.AreEqual(11, obj.RowCount);

            Assert.IsTrue(obj.HasNextPage);
            Assert.IsTrue(obj.HasPreviousPage);
            
            Assert.IsNotNull(obj.ExecutionDetails);
            Assert.IsNotNull(obj.ExecutionDetails.ExecutionTimeMs);
            Assert.IsNotNull(obj.ExecutionDetails.ExecutionDate);
            Assert.AreEqual(150, obj.ExecutionDetails.ExecutionTimeMs);
            Assert.AreEqual(_currentDateTime, obj.ExecutionDetails.ExecutionDate);
        }

        [TestMethod]
        [DataRow(true, false, true, false)]
        [DataRow(false, false, true, false)]
        [DataRow(true, true, true, false)]
        public void PagedResultWithDataAndPrevNextPageTest(bool hasNextPage, bool hasPreviousPage,
            bool exceptedHasNextPage, bool exceptedHasPreviousPage)
        {
            var obj = new PagedResult<TempDataDto>()
            {
                //IsSuccess = true,
                Response = _localInfo,
                RowCount = _rowCount,
                PageSize = 5,
                PageCount = 2,
                CurrentPage = 1,
                ExecutionDetails = new PagedExecDetailsResult(150, _currentDateTime),
                //Messages = null,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage
            };

            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.Response);

            //Assert.IsNull(obj.Messages);

            Assert.AreEqual(2, obj.PageCount);
            Assert.AreEqual(1, obj.CurrentPage);
            Assert.AreEqual(5, obj.PageSize);
            Assert.AreEqual(11, obj.RowCount);

            Assert.AreEqual(exceptedHasNextPage, obj.HasNextPage);
            Assert.AreEqual(exceptedHasPreviousPage, obj.HasPreviousPage);
            
            Assert.IsNotNull(obj.ExecutionDetails);
            Assert.IsNotNull(obj.ExecutionDetails.ExecutionTimeMs);
            Assert.IsNotNull(obj.ExecutionDetails.ExecutionDate);
            Assert.AreEqual(150, obj.ExecutionDetails.ExecutionTimeMs);
            Assert.AreEqual(_currentDateTime, obj.ExecutionDetails.ExecutionDate);
        }

        [TestMethod]
        [DataRow(5, 1, true, false, true, false)]
        [DataRow(6, 1, true, false, true, false)]
        [DataRow(6, 2, false, true, false, true)]
        [DataRow(6, 2, true, false, false, true)]
        public void PagedResultWithDataCustomDataAndPrevNextPageTest(int pageSize, int currentPage,
            bool hasNextPage, bool hasPreviousPage, bool exceptedHasNextPage, bool exceptedHasPreviousPage)
        {
            var pageCount = (int)Math.Ceiling((double)_rowCount / pageSize);

            var obj = new PagedResult<TempDataDto>()
            {
                //IsSuccess = true,
                Response = _localInfo,
                RowCount = _rowCount,
                PageSize = pageSize,
                PageCount = pageCount,
                CurrentPage = currentPage,
                ExecutionDetails = new PagedExecDetailsResult(150, _currentDateTime),
               //Messages = null,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage
            };

            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.Response);

            //Assert.IsNull(obj.Messages);

            Assert.AreEqual(pageCount, obj.PageCount);
            Assert.AreEqual(currentPage, obj.CurrentPage);
            Assert.AreEqual(pageSize, obj.PageSize);
            Assert.AreEqual(_rowCount, obj.RowCount);

            Assert.AreEqual(exceptedHasNextPage, obj.HasNextPage);
            Assert.AreEqual(exceptedHasPreviousPage, obj.HasPreviousPage);
            
            Assert.IsNotNull(obj.ExecutionDetails);
            Assert.IsNotNull(obj.ExecutionDetails.ExecutionTimeMs);
            Assert.IsNotNull(obj.ExecutionDetails.ExecutionDate);
            Assert.AreEqual(150, obj.ExecutionDetails.ExecutionTimeMs);
            Assert.AreEqual(_currentDateTime, obj.ExecutionDetails.ExecutionDate);
        }

        [TestMethod]
        [DataRow(5, 1, true, false)]
        [DataRow(6, 1, true, false)]
        [DataRow(6, 2, false, true)]
        [DataRow(6, 2, false, true)]
        public void PagedResultWithDataCustomDataTest(int pageSize, int currentPage,
            bool exceptedHasNextPage, bool exceptedHasPreviousPage)
        {
            var pageCount = (int)Math.Ceiling((double)_rowCount / pageSize);

            var obj = new PagedResult<TempDataDto>()
            {
                //IsSuccess = true,
                Response = _localInfo,
                RowCount = _rowCount,
                PageSize = pageSize,
                PageCount = pageCount,
                CurrentPage = currentPage,
                ExecutionDetails = new PagedExecDetailsResult(150, _currentDateTime),
                //Messages = null
            };

            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.Response);

            //Assert.IsNull(obj.Messages);

            Assert.AreEqual(pageCount, obj.PageCount);
            Assert.AreEqual(currentPage, obj.CurrentPage);
            Assert.AreEqual(pageSize, obj.PageSize);
            Assert.AreEqual(_rowCount, obj.RowCount);

            Assert.AreEqual(exceptedHasNextPage, obj.HasNextPage);
            Assert.AreEqual(exceptedHasPreviousPage, obj.HasPreviousPage);
            
            Assert.IsNotNull(obj.ExecutionDetails);
            Assert.IsNotNull(obj.ExecutionDetails.ExecutionTimeMs);
            Assert.IsNotNull(obj.ExecutionDetails.ExecutionDate);
            Assert.AreEqual(150, obj.ExecutionDetails.ExecutionTimeMs);
            Assert.AreEqual(_currentDateTime, obj.ExecutionDetails.ExecutionDate);
        }
    }
}