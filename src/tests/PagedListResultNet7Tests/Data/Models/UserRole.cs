// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet7Tests
//  Author           : RzR
//  Created On       : 2023-11-15 00:42
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-15 00:50
// ***********************************************************************
//  <copyright file="UserRole.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace PagedListResultNet7Tests.Data.Models
{
    public class UserRole
    {
        [Key] [ForeignKey(nameof(User))] public int UserId { get; set; }

        public User User { get; set; }

        [Key] [ForeignKey(nameof(Role))] public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}