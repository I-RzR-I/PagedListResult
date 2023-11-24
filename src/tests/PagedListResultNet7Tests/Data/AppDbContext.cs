// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet7Tests
//  Author           : RzR
//  Created On       : 2023-11-15 00:42
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-15 00:50
// ***********************************************************************
//  <copyright file="AppDbContext.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.EntityFrameworkCore;
using PagedListResultNet7Tests.Data.Models;

#endregion

namespace PagedListResultNet7Tests.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Post> Posts { get; set; }
    }
}