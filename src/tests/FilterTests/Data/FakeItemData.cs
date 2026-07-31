// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-10-26 10:07
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-31 15:00
// ***********************************************************************
//  <copyright file="FakeItemData.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using FilterTests.Models;

#endregion

namespace FilterTests.Data
{
    internal static class FakeItemData
    {
        internal static EnumerableQuery<TestItemDto> InitInfo() => new EnumerableQuery<TestItemDto>(new List<TestItemDto>
        {
            new TestItemDto
            {
                Id = 0,
                Name = "Test",
                Value = "Test",
                IsActive = true,
                Date = DateTime.Parse("2010/05/01"),
                EndDate = DateTime.Parse("2010/05/10"),
                Count = 5,
                IsBlocked = false,
                Price = 1,
                RetailPrice = (decimal?)1.2,
                MinQuantity = null,
                UniqueId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                OptionalUniqueId = null,
                CreatedAt = new DateTimeOffset(2010, 5, 1, 0, 0, 0, TimeSpan.Zero),
                Duration = TimeSpan.FromMinutes(30),
                Status = TestItemStatus.Pending
            },
            new TestItemDto
            {
                Id = 1,
                Name = "Test1",
                Value = "Test1",
                IsActive = false,
                Date = DateTime.Parse("2010/07/01"),
                Count = 10,
                IsBlocked = true,
                Price = 2,
                MinQuantity = 0,
                UniqueId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                OptionalUniqueId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                CreatedAt = new DateTimeOffset(2010, 7, 1, 0, 0, 0, TimeSpan.Zero),
                Duration = TimeSpan.FromHours(1),
                Status = TestItemStatus.Active
            },
            new TestItemDto
            {
                Id = 2,
                Name = "Test2",
                Value = "Test2",
                IsActive = false,
                Date = DateTime.Parse("2011/07/01"),
                EndDate = DateTime.Parse("2011/07/01"),
                Count = 100,
                IsBlocked = null,
                Price = 3,
                RetailPrice = (decimal?)4.2,
                MinQuantity = 1,
                UniqueId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                OptionalUniqueId = null,
                CreatedAt = new DateTimeOffset(2011, 7, 1, 0, 0, 0, TimeSpan.Zero),
                Duration = TimeSpan.FromHours(2),
                Status = TestItemStatus.Completed
            },
            new TestItemDto
            {
                Id = 3,
                Name = "TrustValue",
                Value = "ZeroTwo",
                IsActive = true,
                Date = DateTime.Parse("2021/07/01"),
                EndDate = DateTime.Parse("2111/07/01"),
                Count = 101,
                IsBlocked = null,
                Price = 3,
                RetailPrice = (decimal?)4.2,
                MinQuantity = 10,
                UniqueId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                OptionalUniqueId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                CreatedAt = new DateTimeOffset(2021, 7, 1, 0, 0, 0, TimeSpan.Zero),
                Duration = TimeSpan.FromMinutes(90),
                Status = TestItemStatus.Cancelled
            }
        });
    }
}