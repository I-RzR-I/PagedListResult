// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.WebApiNet8Npgsql
//  Author           : RzR
//  Created On       : 2026-03-30 12:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-30 12:26
// ***********************************************************************
//  <copyright file="DocumentEntityConfiguration.cs" company="RzR SOFT & TECH">
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
    public class DocumentEntityConfiguration : IEntityTypeConfiguration<DocumentDataModel>
    {
        public void Configure(EntityTypeBuilder<DocumentDataModel> builder)
        {
            builder.ToTable("Documents", "doc");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Content).IsRequired();

            // Just the FK index - relationship defined in Host
            builder.HasIndex(x => x.UserId);
        }
    }
}