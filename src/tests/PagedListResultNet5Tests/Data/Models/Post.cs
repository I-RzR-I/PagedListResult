// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet5Tests
//  Author           : RzR
//  Created On       : 2023-11-10 16:45
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-12 21:10
// ***********************************************************************
//  <copyright file="Post.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace PagedListResultNet5Tests.Data.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Contents { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [ForeignKey(nameof(Author))] public int AuthorId { get; set; }

        public User Author { get; set; }
    }
}