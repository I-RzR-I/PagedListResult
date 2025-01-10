// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet5Tests
//  Author           : RzR
//  Created On       : 2023-11-12 20:34
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-12 21:10
// ***********************************************************************
//  <copyright file="GetPagedAsyncWithPagedRequestTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResult;
using PagedListResult.DataModels.Enums;
using PagedListResult.DataModels.Models.Request;
using PagedListResult.DataModels.Models.Request.Page;
using PagedListResultNet5Tests.Data;
using PagedListResultNet5Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace PagedListResultNet5Tests
{
    [TestClass]
    public class GetPagedAsyncWithPagedRequestTests
    {
        private AppDbContext _dbContext;

        [TestInitialize]
        public void Init()
        {
            var dbName = $"GetPagedAsyncWithPagedRequestTestsDb_{DateTime.Now.ToFileTimeUtc()}";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _dbContext = new AppDbContext(options);
            _dbContext.AddInitInfo();
        }

        [TestMethod]
        public async Task GetPagedAsync_Exception_Query_Test()
            => await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => ((IQueryable<PostDetail>)null).GetPagedAsync((PagedRequest)null));

        [TestMethod]
        public async Task GetPagedAsync_Exception_Request_Test()
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

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => query.GetPagedAsync((PagedRequest)null));
        }

        [TestMethod]
        public async Task DataCount_Test()
        {
            var usersCount = await _dbContext.Users.CountAsync();
            var rolesCount = await _dbContext.Roles.CountAsync();
            var postsCount = await _dbContext.Posts.CountAsync();

            Assert.AreEqual(5, usersCount);
            Assert.AreEqual(4, rolesCount);
            Assert.AreEqual(6, postsCount);
        }

        [TestMethod]
        public async Task GetPagedAsync_Request_Fields_Test()
        {
            var pageRequest = new PagedRequest { Page = 1, PageSize = 5, Fields = new List<string> { "id", "authorId", "authorName" } };

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

            var records = await query.GetPagedAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(2, records.PageCount);
            Assert.AreEqual(6, records.RowCount);
            Assert.AreEqual(5, records.Response.Count);

            Assert.IsFalse(records.Response[0].AuthorName.IsNullOrEmpty());
            Assert.IsTrue(records.Response[0].Contents.IsNullOrEmpty());
            Assert.IsTrue(records.Response[0].CreatedOn.IsNull());
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_1_PageSize_5_Test()
        {
            var pageRequest = new PagedRequest { Page = 1, PageSize = 5 };

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

            var records = await query.GetPagedAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(2, records.PageCount);
            Assert.AreEqual(6, records.RowCount);
            Assert.AreEqual(5, records.Response.Count);
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_2_PageSize_5_Test()
        {
            var pageRequest = new PagedRequest { Page = 2, PageSize = 5 };

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

            var records = await query.GetPagedAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(2, records.CurrentPage);
            Assert.AreEqual(2, records.PageCount);
            Assert.AreEqual(6, records.RowCount);
            Assert.AreEqual(1, records.Response.Count);
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_1_PageSize_5_Search_admin_Test()
        {
            var pageRequest = new PagedRequest { Page = 1, PageSize = 5, Search = new DataSearchDefinition { Search = "admin" } };

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

            var records = await query.GetPagedAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(0, records.PageCount);
            Assert.AreEqual(0, records.RowCount);
            Assert.AreEqual(0, records.Response.Count);
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_1_PageSize_5_Search_post_Test()
        {
            var pageRequest = new PagedRequest { Page = 1, PageSize = 5, Search = new DataSearchDefinition { Search = "post" } };

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

            var records = await query.GetPagedAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(2, records.PageCount);
            Assert.AreEqual(6, records.RowCount);
            Assert.AreEqual(5, records.Response.Count);
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_1_PageSize_5_Search_Post01_Test()
        {
            var pageRequest = new PagedRequest { Page = 1, PageSize = 5, Search = new DataSearchDefinition { Search = "Post 01" } };

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

            var records = await query.GetPagedAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(1, records.PageCount);
            Assert.AreEqual(1, records.RowCount);
            Assert.AreEqual(1, records.Response.Count);
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_1_PageSize_5_Search_Post01_1_Test()
        {
            var pageRequest = new PagedRequest
            {
                Page = 1,
                PageSize = 5,
                Search = new DataSearchDefinition { Search = "Post 01", SearchInAllTextFields = false, CustomSearchTextProperties = new List<string> { "contents", "title" } }
            };

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

            var records = await query.GetPagedAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(1, records.PageCount);
            Assert.AreEqual(1, records.RowCount);
            Assert.AreEqual(1, records.Response.Count);
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_1_PageSize_5_Search_Post010_0_Test()
        {
            var pageRequest = new PagedRequest
            {
                Page = 1, PageSize = 5, Search = new DataSearchDefinition { Search = "Post 010", SearchInAllTextFields = false, CustomSearchTextProperties = new List<string> { "title" } }
            };

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

            var records = await query.GetPagedAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(0, records.PageCount);
            Assert.AreEqual(0, records.RowCount);
            Assert.AreEqual(0, records.Response.Count);
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_1_PageSize_5_Order_Default_Asc_Test()
        {
            var pageRequest = new PagedRequest { Page = 1, PageSize = 5, Order = new DataOrderDefinition { OrderByDefaultProperty = true } };

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

            var records = await query.GetPagedAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(2, records.PageCount);
            Assert.AreEqual(6, records.RowCount);
            Assert.AreEqual(5, records.Response.Count);

            Assert.AreEqual(1, records.Response.FirstOrDefault()!.AuthorId);
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_1_PageSize_5_Order_Default_Desc_Test()
        {
            var pageRequest = new PagedRequest { Page = 1, PageSize = 5, Order = new DataOrderDefinition { OrderByDefaultProperty = true, OrderDirection = OrderDirection.Desc } };

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

            var records = await query.GetPagedAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(2, records.PageCount);
            Assert.AreEqual(6, records.RowCount);
            Assert.AreEqual(5, records.Response.Count);

            Assert.AreEqual(4, records.Response.FirstOrDefault()!.AuthorId);
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_1_PageSize_5_Order_ByProp_Desc_Test()
        {
            var pageRequest = new PagedRequest { Page = 1, PageSize = 5, Order = new DataOrderDefinition { OrderByProperty = "id", OrderDirection = OrderDirection.Desc } };

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

            var records = await query.GetPagedAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(2, records.PageCount);
            Assert.AreEqual(6, records.RowCount);
            Assert.AreEqual(5, records.Response.Count);

            Assert.AreEqual(6, records.Response.FirstOrDefault()!.Id);
            Assert.AreEqual(4, records.Response.FirstOrDefault()!.AuthorId);
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_1_PageSize_5_PredefinedRecords_Default_PkAttribute_Test()
        {
            var pageRequest = new PagedRequest { Page = 1, PageSize = 5, PredefinedRecords = new List<string> { "5" } };

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

            var records = await query.GetPagedAsync(pageRequest, new DefaultPrimaryKeyDefinition { FindByAttribute = true });

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(2, records.PageCount);
            Assert.AreEqual(6, records.RowCount);
            Assert.AreEqual(5, records.Response.Count);

            Assert.AreEqual(5, records.Response.FirstOrDefault()!.Id);
            Assert.AreEqual(1, records.Response[1].Id);
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_1_PageSize_5_PredefinedRecords_Default_PkEntity_Test()
        {
            var pageRequest = new PagedRequest { Page = 1, PageSize = 5, PredefinedRecords = new List<string> { "4" } };

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

            var records = await query.GetPagedAsync(pageRequest, new DefaultPrimaryKeyDefinition { FindByEntity = true });

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(2, records.PageCount);
            Assert.AreEqual(6, records.RowCount);
            Assert.AreEqual(5, records.Response.Count);

            Assert.AreEqual(4, records.Response.FirstOrDefault()!.Id);
            Assert.AreEqual(1, records.Response[1].Id);
        }

        [TestMethod]
        public async Task GetPagedAsync_Page_1_PageSize_5_PredefinedRecords_Default_PkCustom_Test()
        {
            var pageRequest = new PagedRequest { Page = 1, PageSize = 5, PredefinedRecords = new List<string> { "3" } };

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

            var records = await query.GetPagedAsync(pageRequest, new DefaultPrimaryKeyDefinition { DefaultPrimaryKey = "id" });

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(2, records.PageCount);
            Assert.AreEqual(6, records.RowCount);
            Assert.AreEqual(5, records.Response.Count);

            Assert.AreEqual(3, records.Response.FirstOrDefault()!.Id);
            Assert.AreEqual(1, records.Response[1].Id);
        }
    }
}