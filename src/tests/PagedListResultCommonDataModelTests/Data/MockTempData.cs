// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultCommonDataModelTests
//  Author           : RzR
//  Created On       : 2024-12-27 14:13
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-27 14:13
// ***********************************************************************
//  <copyright file="MockTempData.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using PagedListResultCommonDataModelTests.Models;
using System.Collections.Generic;

namespace PagedListResultCommonDataModelTests.Data
{
    public class MockTempData
    {
        public static readonly IList<TempDataDto> TempData1 = new List<TempDataDto>()
        {
            new TempDataDto()
            {
                Id = 1,
                Code = "Code-001",
                Name = "Name-001",
                IsActive = true
            },
            new TempDataDto()
            {
                Id = 2,
                Code = "Code-002",
                Name = "Name-002",
                IsActive = true
            },
            new TempDataDto()
            {
                Id = 3,
                Code = "Code-003",
                Name = "Name-003",
                IsActive = true
            },
            new TempDataDto()
            {
                Id = 4,
                Code = "Code-004",
                Name = "Name-004",
                IsActive = false
            },
            new TempDataDto()
            {
                Id = 5,
                Code = "Code-005",
                Name = "Name-005",
                IsActive = true
            },
            new TempDataDto()
            {
                Id = 6,
                Code = "Code-006",
                Name = "Name-006",
                IsActive = false
            },
            new TempDataDto()
            {
                Id = 7,
                Code = "Code-007",
                Name = "Name-007",
                IsActive = false
            },
            new TempDataDto()
            {
                Id = 8,
                Code = "Code-008",
                Name = "Name-008",
                IsActive = true
            },
            new TempDataDto()
            {
                Id = 9,
                Code = "Code-009",
                Name = "Name-009",
                IsActive = true
            },
            new TempDataDto()
            {
                Id = 10,
                Code = "Code-010",
                Name = "Name-010",
                IsActive = false
            },
            new TempDataDto()
            {
                Id = 11,
                Code = "Code-011",
                Name = "Name-011",
                IsActive = true
            }
        };
    }
}