// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet7Tests
//  Author           : RzR
//  Created On       : 2023-11-15 00:42
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-15 00:50
// ***********************************************************************
//  <copyright file="GetPagedAsyncTests.cs" company="">
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
using PagedListResultNet7Tests.Data;
using PagedListResultNet7Tests.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace PagedListResultNet7Tests
{
    [TestClass]
    public class GetPagedAsyncTests
    {
        private AppDbContext _dbContext;

        [TestInitialize]
        public void Init()
        {
            var dbName = $"GetPagedAsyncTestsDb_{DateTime.Now.ToFileTimeUtc()}";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _dbContext = new AppDbContext(options);
            _dbContext.AddInitInfo();
        }

        [TestMethod]
        public async Task GetPagedAsync_Exception_Query_Test()
            => await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => ((IQueryable<PostDetail>)null).GetPagedAsync(1, 2));

        [TestMethod]
        [DataRow(0, 5)]
        [DataRow(2, 0)]
        [DataRow(0, 0)]
        public async Task GetPagedAsync_Exception_Test(int page, int pageSize)
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

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => query.GetPagedAsync(page, pageSize));
        }

        [TestMethod]
        [DataRow(1, 5, 5)]
        [DataRow(2, 5, 1)]
        [DataRow(1, 10, 6)]
        public async Task GetPagedAsync_Test(int page, int pageSize, int rowExcepted)
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

            var records = await query.GetPagedAsync(page, pageSize);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs >= 0);
            Assert.AreEqual(page, records.CurrentPage);
            Assert.AreEqual(rowExcepted, records.Response.Count);
        }
    }
}