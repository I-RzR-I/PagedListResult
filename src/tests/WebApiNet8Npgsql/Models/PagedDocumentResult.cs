// ***********************************************************************
//  Assembly         : MMDbConnect.MMDbConnect.Host
//  Author           : RzR
//  Created On       : 2026-03-29 02:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-29 02:18
// ***********************************************************************
//  <copyright file="PagedDocumentResult.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace WebApiNet8Npgsql.Models
{
    public class PagedDocumentResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

