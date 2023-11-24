// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet5Tests
//  Author           : RzR
//  Created On       : 2023-11-10 15:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-12 21:10
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
using WebApiNet5.Data.Models;

#endregion

namespace WebApiNet5.Data
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