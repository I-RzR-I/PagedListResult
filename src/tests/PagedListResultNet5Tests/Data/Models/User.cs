// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet5Tests
//  Author           : RzR
//  Created On       : 2023-11-10 16:31
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-12 21:10
// ***********************************************************************
//  <copyright file="User.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;

#endregion

namespace PagedListResultNet5Tests.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? BlockedOn { get; set; }
        public DateTime? LastActivityDate { get; set; }

        public ICollection<Post> Posts { get; set; }
        //public ICollection<UserRole> UserRoles { get; set; }
    }
}