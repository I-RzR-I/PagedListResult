// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet7Tests
//  Author           : RzR
//  Created On       : 2023-11-15 00:42
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-15 00:50
// ***********************************************************************
//  <copyright file="Role.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace PagedListResultNet7Tests.Data.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        //public ICollection<UserRole> UserRoles { get; set; }
    }
}