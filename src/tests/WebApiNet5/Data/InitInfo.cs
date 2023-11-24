// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultNet5Tests
//  Author           : RzR
//  Created On       : 2023-11-10 17:14
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-12 21:10
// ***********************************************************************
//  <copyright file="InitInfo.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebApiNet5.Data.Models;

#endregion

namespace WebApiNet5.Data
{
    internal static class InitInfo
    {
        internal static void AddInitInfo(this DbContext dbContext)
        {
            var roles = new List<Role>
            {
                new Role
                {
                    Id = 1,
                    Name = "UserRole1",
                    Code = "URole1",
                    Description = "User role 1",
                    IsActive = true
                },
                new Role
                {
                    Id = 2,
                    Name = "UserRole2",
                    Code = "URole2",
                    Description = "User role 2",
                    IsActive = true
                },
                new Role
                {
                    Id = 3,
                    Name = "UserRole3",
                    Code = "URole3",
                    Description = "User role 3",
                    IsActive = false
                },
                new Role
                {
                    Id = 4,
                    Name = "UserRole4",
                    Code = "URole4",
                    Description = "User role 4",
                    IsActive = true
                }
            };
            dbContext.Set<Role>().AddRange(roles);

            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "User1",
                    CreatedOn = DateTime.Now.AddDays(-1),
                    IsActive = true,
                    LastActivityDate = DateTime.Now,
                    IsBlocked = false
                },
                new User
                {
                    Id = 2,
                    Name = "User2",
                    CreatedOn = DateTime.Now.AddDays(-2),
                    IsActive = true,
                    LastActivityDate = DateTime.Now.AddDays(-2),
                    IsBlocked = true,
                    BlockedOn = DateTime.Now
                },
                new User
                {
                    Id = 3,
                    Name = "User3",
                    CreatedOn = DateTime.Now.AddDays(-10),
                    IsActive = false,
                    LastActivityDate = DateTime.Now.AddDays(-2),
                    IsBlocked = false
                },
                new User
                {
                    Id = 4,
                    Name = "User4",
                    CreatedOn = DateTime.Now.AddDays(-1),
                    IsActive = true,
                    LastActivityDate = DateTime.Now,
                    IsBlocked = false
                },
                new User
                {
                    Id = 5,
                    Name = "user5.adm",
                    CreatedOn = DateTime.Now.AddDays(-1),
                    IsActive = true,
                    LastActivityDate = DateTime.Now,
                    IsBlocked = false
                }
            };
            dbContext.Set<User>().AddRange(users);

            var posts = new List<Post>
            {
                new Post
                {
                    Id = 1,
                    AuthorId = 1,
                    CreatedOn = DateTime.Now,
                    Contents = "Post 01",
                    Title = "Title post 01"
                },
                new Post
                {
                    Id = 2,
                    AuthorId = 1,
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Contents = "Post 02",
                    Title = "Title post 02",
                    ModifiedOn = DateTime.Now
                },
                new Post
                {
                    Id = 3,
                    AuthorId = 2,
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Contents = "Post 03",
                    Title = "Title post 03",
                    ModifiedOn = DateTime.Now
                },
                new Post
                {
                    Id = 4,
                    AuthorId = 3,
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Contents = "Post 04",
                    Title = "Title post 04"
                },
                new Post
                {
                    Id = 5,
                    AuthorId = 3,
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Contents = "Post 05",
                    Title = "Title post 05"
                },
                new Post
                {
                    Id = 6,
                    AuthorId = 4,
                    CreatedOn = DateTime.Now.AddDays(-4),
                    Contents = "Post 06",
                    Title = "Title post 06",
                    ModifiedOn = DateTime.Now.AddDays(-4)
                }
            };
            dbContext.Set<Post>().AddRange(posts);

            dbContext.SaveChanges();
        }
    }
}