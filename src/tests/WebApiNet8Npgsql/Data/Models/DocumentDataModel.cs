// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.WebApiNet8Npgsql
//  Author           : RzR
//  Created On       : 2026-03-30 12:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-30 12:25
// ***********************************************************************
//  <copyright file="DocumentDataModel.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

#endregion

namespace WebApiNet8Npgsql.Data.Models
{
    public class DocumentDataModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}