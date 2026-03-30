// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.WebApiNet8Npgsql
//  Author           : RzR
//  Created On       : 2026-03-30 12:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-30 12:26
// ***********************************************************************
//  <copyright file="UserEntityConfiguration.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiNet8Npgsql.Data.Models;

#endregion

namespace WebApiNet8Npgsql.Data.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserDataModel>
    {
        public void Configure(EntityTypeBuilder<UserDataModel> builder)
        {
            builder.ToTable("Users", "identity");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
        }
    }
}