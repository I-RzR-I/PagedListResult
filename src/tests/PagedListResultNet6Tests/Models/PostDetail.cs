// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet6Tests
//  Author           : RzR
//  Created On       : 2023-11-15 00:29
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-15 00:35
// ***********************************************************************
//  <copyright file="PostDetail.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using PagedListResult.Common.Attributes;
using System;

#endregion

namespace PagedListResultNet6Tests.Models
{
    public class PostDetail
    {
        [PaginationDefaultTopRecordPrimaryKey] public int Id { get; set; }

        public string Title { get; set; }

        public string Contents { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [PaginationDefaultOrderProperty] public int AuthorId { get; set; }

        public string AuthorName { get; set; }
    }
}