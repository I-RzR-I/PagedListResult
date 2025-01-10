// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet5Tests
//  Author           : RzR
//  Created On       : 2023-11-12 21:38
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-13 21:28
// ***********************************************************************
//  <copyright file="GetPagedWithMainFiltersTests.cs" company="">
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
    public class GetPagedWithMainFiltersTests
    {
        private AppDbContext _dbContext;

        [TestInitialize]
        public void Init()
        {
            var dbName = $"GetPagedWithMainFiltersTestsDb_{DateTime.Now.ToFileTimeUtc()}";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _dbContext = new AppDbContext(options);
            _dbContext.AddInitInfo();
        }

        [TestMethod]
        public async Task GetPagedWithMainFilters_Exception_Query_Test()
            => await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => ((IQueryable<PostDetail>)null).GetPagedWithMainFiltersAsync((PageRequestWithFilters)null));

        [TestMethod]
        public async Task GetPagedWithMainFiltersAsync_Exception_Request_Test()
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

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => query.GetPagedWithMainFiltersAsync((PageRequestWithFilters)null));
        }

        [TestMethod]
        public async Task GetPagedWithMainFiltersAsync_Request_FilterException_Fields_Test()
        {
            var pageRequest = new PageRequestWithFilters
            {
                Page = 1,
                PageSize = 5,
                Fields = new List<string> { "id", "authorId", "authorName" },
                Filters = new List<DataFilter> { new DataFilter { FilterValue = new DataFilterValue() } }
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

            await Assert.ThrowsExceptionAsync<Exception>(
                () => query.GetPagedWithMainFiltersAsync(pageRequest));
        }

        [TestMethod]
        public async Task GetPagedWithMainFiltersAsync_Request_Fields_Test()
        {
            var pageRequest = new PageRequestWithFilters
            {
                Page = 1,
                PageSize = 5,
                Fields = new List<string> { "id", "authorId", "authorName" },
                Filters = new List<DataFilter>
                    {
                        new DataFilter
                        {
                            FilterValue = new DataFilterValue
                            {
                                Values = new List<string> { "1" },
                                PropertyName = "id",
                                Condition = FilterType.Equals
                            }
                        }
                    }
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

            var records = await query.GetPagedWithMainFiltersAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(1, records.PageCount);
            Assert.AreEqual(1, records.RowCount);
            Assert.AreEqual(1, records.Response.Count);

            Assert.IsFalse(records.Response[0].AuthorName.IsNullOrEmpty());
            Assert.IsTrue(records.Response[0].Contents.IsNullOrEmpty());
            Assert.IsTrue(records.Response[0].CreatedOn.IsNull());
        }

        [TestMethod]
        public async Task GetPagedWithMainFiltersAsync_Page_1_PageSize_5_Test()
        {
            var pageRequest = new PageRequestWithFilters
            {
                Page = 1,
                PageSize = 5,
                Filters = new List<DataFilter>
                {
                    new DataFilter
                    {
                        FilterApplyOrder = 0,
                        FilterValue = new DataFilterValue
                        {
                            Condition = FilterType.Equals,
                            PropertyName = "authorId",
                            Values = new List<string> { "3" }
                        }
                    }
                }
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

            var records = await query.GetPagedWithMainFiltersAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(1, records.PageCount);
            Assert.AreEqual(2, records.RowCount);
            Assert.AreEqual(2, records.Response.Count);
        }

        [TestMethod]
        public async Task GetPagedWithMainFiltersAsync_Page_1_PageSize_5_FilterTypeIsNull_Test()
        {
            var pageRequest = new PageRequestWithFilters
            {
                Page = 1,
                PageSize = 5,
                Filters = new List<DataFilter>
                {
                    new DataFilter
                    {
                        FilterValue = new DataFilterValue
                        {
                            PropertyName = "modifiedOn",
                            Condition = FilterType.IsNull
                        }
                    }
                }
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
                    Title = x.Title,
                    ModifiedOn = x.ModifiedOn
                });

            var records = await query.GetPagedWithMainFiltersAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(1, records.PageCount);
            Assert.AreEqual(3, records.RowCount);
            Assert.AreEqual(3, records.Response.Count);
        }

        [TestMethod]
        public async Task GetPagedWithMainFiltersAsync_Page_1_PageSize_5_FilterTypeIsNull_And_AuthorIdIn_Test()
        {
            var pageRequest = new PageRequestWithFilters
            {
                Page = 1,
                PageSize = 5,
                Filters = new List<DataFilter>
                {
                    new DataFilter
                    {
                        FilterValue = new DataFilterValue
                        {
                            PropertyName = "modifiedOn",
                            Condition = FilterType.IsNull
                        },
                        FilterApplyOrder = 0
                    },
                    new DataFilter
                    {
                        FilterValue = new DataFilterValue
                        {
                            PropertyName = "authorId",
                            Condition = FilterType.IsIn,
                            Values = new List<string> { "3", "4" }
                        },
                        FilterApplyOrder = 0
                    }
                }
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
                    Title = x.Title,
                    ModifiedOn = x.ModifiedOn
                });

            var records = await query.GetPagedWithMainFiltersAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(1, records.PageCount);
            Assert.AreEqual(2, records.RowCount);
            Assert.AreEqual(2, records.Response.Count);
        }
    }
}