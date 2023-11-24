// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet6Tests
//  Author           : RzR
//  Created On       : 2023-11-15 00:29
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-15 00:34
// ***********************************************************************
//  <copyright file="Role.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace PagedListResultNet6Tests.Data.Models
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