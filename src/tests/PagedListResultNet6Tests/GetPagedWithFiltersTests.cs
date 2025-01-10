// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet6Tests
//  Author           : RzR
//  Created On       : 2023-11-15 00:29
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-15 00:35
// ***********************************************************************
//  <copyright file="GetPagedWithFiltersTests.cs" company="">
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
using PagedListResultNet6Tests.Data;
using PagedListResultNet6Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace PagedListResultNet6Tests
{
    [TestClass]
    public class GetPagedWithFiltersTests
    {
        private AppDbContext _dbContext;

        [TestInitialize]
        public void Init()
        {
            var dbName = $"GetPagedWithFiltersTestsDb_{DateTime.Now.ToFileTimeUtc()}";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _dbContext = new AppDbContext(options);
            _dbContext.AddInitInfo();
        }

        [TestMethod]
        public async Task GetPagedWithFilters_Exception_Query_Test()
            => await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => ((IQueryable<PostDetail>)null).GetPagedWithFiltersAsync((PageRequestWithFilters)null));

        [TestMethod]
        public async Task GetPagedWithFilters_Exception_Request_Test()
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
                    Title = x.Title,
                    ModifiedOn = x.ModifiedOn
                });

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => query.GetPagedWithFiltersAsync((PageRequestWithFilters)null));
        }

        [TestMethod]
        public async Task GetPagedWithFilters_Request_FilterException_Fields_Test()
        {
            var pageRequest = new PageRequestWithFilters
            {
                Page = 1, PageSize = 5, Fields = new List<string> { "id", "authorId", "authorName" }, Filters = new List<DataFilter> { new DataFilter { FilterValue = new DataFilterValue() } }
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

            await Assert.ThrowsExceptionAsync<Exception>(
                () => query.GetPagedWithFiltersAsync(pageRequest));
        }

        [TestMethod]
        public async Task GetPagedWithFilters_Request_Fields_MainFilter_Or_Test()
        {
            var pageRequest = new PageRequestWithFilters
            {
                Page = 1,
                PageSize = 5,
                Fields = new List<string> { "id", "authorId", "authorName" },
                Filters = new List<DataFilter>
                {
                    new DataFilter { FilterValue = new DataFilterValue { Values = new List<string> { "1" }, PropertyName = "authorId", Condition = FilterType.Equals } },
                    new DataFilter { FilterValue = new DataFilterValue { Values = new List<string> { "2" }, PropertyName = "authorId", Condition = FilterType.Equals } }
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

            var records = await query.GetPagedWithFiltersAsync(pageRequest, null, FilterConditionType.Or);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(1, records.PageCount);
            Assert.AreEqual(3, records.RowCount);
            Assert.AreEqual(3, records.Response.Count);

            Assert.IsFalse(records.Response[0].AuthorName.IsNullOrEmpty());
            Assert.IsTrue(records.Response[0].Contents.IsNullOrEmpty());
            Assert.IsTrue(records.Response[0].CreatedOn.IsNull());
        }

        [TestMethod]
        public async Task GetPagedWithFilters_Request_MainFilter_And_Test()
        {
            var pageRequest = new PageRequestWithFilters
            {
                Page = 1,
                PageSize = 5,
                Filters = new List<DataFilter>
                {
                    new DataFilter { FilterValue = new DataFilterValue { Values = new List<string> { "1", "2", "4" }, PropertyName = "authorId", Condition = FilterType.IsIn } },
                    new DataFilter
                    {
                        FilterValue = new DataFilterValue
                        {
                            Values = new List<string> { DateTime.Now.StartOfDay().ToString() },
                            CompareValue = DateTime.Now.EndOfDay().ToString(),
                            PropertyName = "createdOn",
                            Condition = FilterType.Between
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

            var records = await query.GetPagedWithFiltersAsync(pageRequest);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(1, records.PageCount);
            Assert.AreEqual(1, records.RowCount);
            Assert.AreEqual(1, records.Response.Count);

            Assert.IsFalse(records.Response[0].AuthorName.IsNullOrEmpty());
            Assert.IsFalse(records.Response[0].Contents.IsNullOrEmpty());
            Assert.IsFalse(records.Response[0].CreatedOn.IsNull());
        }

        [TestMethod]
        public async Task GetPagedWithFilters_Request_MainFilter_Or_WithDependencies_Test()
        {
            var pageRequest = new PageRequestWithFilters
            {
                Page = 1,
                PageSize = 5,
                Filters = new List<DataFilter>
                {
                    new DataFilter
                    {
                        FilterValue = new DataFilterValue { Values = new List<string> { "2" }, PropertyName = "authorId", Condition = FilterType.IsIn },
                        FilterApplyOrder = 0,
                        Dependencies = new List<DataFilterDependence>
                        {
                            new DataFilterDependence
                            {
                                FilterValue = new DataFilterValue { PropertyName = "authorId", Condition = FilterType.Equals, Values = new List<string> { "3" } },
                                ParentFilterLinkType = FilterConditionType.Or
                            }
                        }
                    },
                    new DataFilter
                    {
                        FilterValue = new DataFilterValue
                        {
                            Values = new List<string> { DateTime.Now.StartOfDay().ToString() },
                            CompareValue = DateTime.Now.EndOfDay().ToString(),
                            PropertyName = "createdOn",
                            Condition = FilterType.Between
                        },
                        FilterApplyOrder = 1
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

            var records = await query.GetPagedWithFiltersAsync(pageRequest, null, FilterConditionType.Or);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(1, records.PageCount);
            Assert.AreEqual(4, records.RowCount);
            Assert.AreEqual(4, records.Response.Count);
        }

        [TestMethod]
        public async Task GetPagedWithFilters_Request_MainFilter_Or_WithDependencies_PredefinedRecord_Test()
        {
            var pageRequest = new PageRequestWithFilters
            {
                Page = 1,
                PageSize = 5,
                Filters = new List<DataFilter>
                {
                    new DataFilter
                    {
                        FilterValue = new DataFilterValue { Values = new List<string> { "2" }, PropertyName = "authorId", Condition = FilterType.IsIn },
                        FilterApplyOrder = 0,
                        Dependencies = new List<DataFilterDependence>
                        {
                            new DataFilterDependence
                            {
                                FilterValue = new DataFilterValue { PropertyName = "authorId", Condition = FilterType.Equals, Values = new List<string> { "3" } },
                                ParentFilterLinkType = FilterConditionType.Or
                            }
                        }
                    },
                    new DataFilter
                    {
                        FilterValue = new DataFilterValue
                        {
                            Values = new List<string> { DateTime.Now.StartOfDay().ToString() },
                            CompareValue = DateTime.Now.EndOfDay().ToString(),
                            PropertyName = "createdOn",
                            Condition = FilterType.Between
                        },
                        FilterApplyOrder = 1
                    }
                },
                PredefinedRecords = new List<string> { "5" }
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

            var records = await query.GetPagedWithFiltersAsync(pageRequest, null, FilterConditionType.Or);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(1, records.PageCount);
            Assert.AreEqual(4, records.RowCount);
            Assert.AreEqual(4, records.Response.Count);

            Assert.AreEqual(5, records.Response.First().Id);
        }

        [TestMethod]
        public async Task GetPagedWithFilters_Request_MainFilter_Or_WithDependencies_PredefinedRecord_AndPk_Test()
        {
            var pageRequest = new PageRequestWithFilters
            {
                Page = 1,
                PageSize = 5,
                Filters = new List<DataFilter>
                {
                    new DataFilter
                    {
                        FilterValue = new DataFilterValue { Values = new List<string> { "2" }, PropertyName = "authorId", Condition = FilterType.IsIn },
                        FilterApplyOrder = 0,
                        Dependencies = new List<DataFilterDependence>
                        {
                            new DataFilterDependence
                            {
                                FilterValue = new DataFilterValue { PropertyName = "authorId", Condition = FilterType.Equals, Values = new List<string> { "3" } },
                                ParentFilterLinkType = FilterConditionType.Or
                            }
                        }
                    },
                    new DataFilter
                    {
                        FilterValue = new DataFilterValue
                        {
                            Values = new List<string> { DateTime.Now.StartOfDay().ToString() },
                            CompareValue = DateTime.Now.EndOfDay().ToString(),
                            PropertyName = "createdOn",
                            Condition = FilterType.Between
                        },
                        FilterApplyOrder = 1
                    }
                },
                PredefinedRecords = new List<string> { "5" }
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

            var records = await query.GetPagedWithFiltersAsync(
                pageRequest,
                new DefaultPrimaryKeyDefinition { DefaultPrimaryKey = "id" },
                FilterConditionType.Or);

            Assert.IsNotNull(records);
            Assert.IsNotNull(records.Response);
            Assert.IsNotNull(records.ExecutionDetails);
            Assert.IsTrue(records.ExecutionDetails.ExecutionTimeMs > -1);
            Assert.AreEqual(1, records.CurrentPage);
            Assert.AreEqual(1, records.PageCount);
            Assert.AreEqual(4, records.RowCount);
            Assert.AreEqual(4, records.Response.Count);

            Assert.AreEqual(5, records.Response.First().Id);
        }
    }
}