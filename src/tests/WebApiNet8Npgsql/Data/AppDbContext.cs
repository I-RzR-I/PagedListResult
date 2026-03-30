// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.WebApiNet8Npgsql
//  Author           : RzR
//  Created On       : 2026-03-30 12:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-30 12:22
// ***********************************************************************
//  <copyright file="AppDbContext.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.EntityFrameworkCore;
using WebApiNet8Npgsql.Data.Models;
using System.Reflection;

namespace WebApiNet8Npgsql.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<DocumentDataModel> Documents { get; set; }

        public DbSet<UserDataModel> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<DocumentDataModel>()
                .HasOne<UserDataModel>()
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_doc_Documents_identity_Users")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

