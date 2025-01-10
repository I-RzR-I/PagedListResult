// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultCommonDataModelTests
//  Author           : RzR
//  Created On       : 2024-12-27 13:55
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-27 13:55
// ***********************************************************************
//  <copyright file="PagedExecDetailsResultTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResult.DataModels.Models.Result;
using System;

#endregion

namespace PagedListResultCommonDataModelTests.Result
{
    [TestClass]
    public class PagedExecDetailsResultTests
    {
        [TestMethod]
        public void PagedExecDetailsResultDefaultTest()
        {
            var obj = new PagedExecDetailsResult();

            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.ExecutionTimeMs);
            Assert.IsNotNull(obj.ExecutionDate);
            Assert.AreEqual(-1, obj.ExecutionTimeMs);
            Assert.AreEqual(DateTime.Now.Date, obj.ExecutionDate.Value.Date);
        }

        [TestMethod]
        public void PagedExecDetailsResultDefaultWithDatTest()
        {
            var obj = new PagedExecDetailsResult(100, DateTime.Now.AddDays(-1));

            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.ExecutionTimeMs);
            Assert.IsNotNull(obj.ExecutionDate);
            Assert.AreEqual(100, obj.ExecutionTimeMs);
            Assert.AreEqual(DateTime.Now.Date.AddDays(-1), obj.ExecutionDate.Value.Date);
        }

        [TestMethod]
        public void PagedExecDetailsResultDefaultWithSetDataTest()
        {
            var obj = new PagedExecDetailsResult();

            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.ExecutionTimeMs);
            Assert.IsNotNull(obj.ExecutionDate);
            Assert.AreEqual(-1, obj.ExecutionTimeMs);
            Assert.AreEqual(DateTime.Now.Date, obj.ExecutionDate.Value.Date);

            obj.SetExecutionTimeMs(150, DateTime.Now.AddDays(-10));
            Assert.AreEqual(150, obj.ExecutionTimeMs);
            Assert.AreEqual(DateTime.Now.Date.AddDays(-10), obj.ExecutionDate.Value.Date);
        }
    }
}