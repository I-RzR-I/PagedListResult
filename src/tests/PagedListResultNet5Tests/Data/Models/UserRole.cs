﻿// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet5Tests
//  Author           : RzR
//  Created On       : 2023-11-10 16:51
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-12 21:10
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

namespace PagedListResultNet5Tests.Data.Models
{
    public class UserRole
    {
        [Key] [ForeignKey(nameof(User))] public int UserId { get; set; }

        public User User { get; set; }

        [Key] [ForeignKey(nameof(Role))] public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}