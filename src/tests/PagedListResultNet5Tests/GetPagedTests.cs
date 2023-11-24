// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet5Tests
//  Author           : RzR
//  Created On       : 2023-11-12 19:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-12 21:10
// ***********************************************************************
//  <copyright file="GetPagedTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResult;
using PagedListResultNet5Tests.Data;
using PagedListResultNet5Tests.Models;
using System;
using System.Linq;

#endregion

namespace PagedListResultNet5Tests
{
    [TestClass]
    public class GetPagedTests
    {
        private AppDbContext _dbContext;

        [TestInitialize]
        public void Init()
        {
            var dbName = $"GetPagedTestsDb_{DateTime.Now.ToFileTimeUtc()}";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _dbContext = new AppDbContext(options);
            _dbContext.AddInitInfo();
        }

        [TestMethod]
        public void GetPaged_Exception_Query_Test()
            => Assert.ThrowsException<ArgumentNullException>(
                () => ((IQueryable<PostDetail>)null).GetPaged(1, 2));

        [TestMethod]
        [DataRow(0, 5)]
        [DataRow(2, 0)]
        [DataRow(0, 0)]
        public void GetPaged_Exception_Test(int page, int pageSize)
        {
            var query = _dbContext.Posts
                .Include(x => x.Author)
                .Select(x => new PostDetail
                {
                    AuthorId = x.AuthorId,
                    AuthorName = x.Author.Name,
                    Contents = x.Contents,
                    CreatedOn = x.CreatedOn,
                    Id = x.Id,
                    Title = x.Title
                });

            Assert.ThrowsException<ArgumentException>(() => query.GetPaged(page, pageSize));
        }

        [TestMethod]
        [DataRow(1, 5, 5)]
        [DataRow(2, 5, 1)]
        [DataRow(1, 10, 6)]
        public void GetPaged_Test(int page, int pageSize, int rowExcepted)
        {
            var query = _dbContext.Posts
                .Include(x => x.Author)
                .Select(x => new PostDetail
                {
                    AuthorId = x.AuthorId,
                    AuthorName = x.Author.Name,
                    Contents = x.Contents,
                    CreatedOn = x.CreatedOn,
                    Id = x.Id,
                    Title = x.Title
                });

            var records = query.GetPaged(page, pageSize);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs >= 0);
            Assert.AreEqual(page, records.CurrentPage);
            Assert.AreEqual(rowExcepted, records.Response.Count);
        }
    }
}