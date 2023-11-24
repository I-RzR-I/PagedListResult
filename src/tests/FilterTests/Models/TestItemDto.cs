// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-10-26 10:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-31 15:00
// ***********************************************************************
//  <copyright file="TestItemDto.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using PagedListResult.Common.Attributes;

#endregion

namespace FilterTests.Models
{
    internal class TestItemDto
    {
        [PaginationDefaultOrderProperty]
        [PaginationDefaultTopRecordPrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int Count { get; set; }
        public DateTime Date { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool? IsBlocked { get; set; }
        public decimal Price { get; set; }
        public decimal? RetailPrice { get; set; }
        public int? MinQuantity { get; set; }
    }
}