// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet5Tests
//  Author           : RzR
//  Created On       : 2023-11-10 18:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-12 21:10
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

namespace WebApiNet5.Models
{
    public class PostDetail
    {
        [PaginationDefaultTopRecordPrimaryKey]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Contents { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [PaginationDefaultOrderProperty] 
        public int AuthorId { get; set; }

        public string AuthorName { get; set; }
    }
}