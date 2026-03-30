// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.WebApiNet8Npgsql
//  Author           : RzR
//  Created On       : 2026-03-30 12:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-30 12:25
// ***********************************************************************
//  <copyright file="UserDataModel.cs" company="RzR SOFT & TECH">
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
    public class UserDataModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}