// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2023-11-07 20:16
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-08 15:43
// ***********************************************************************
//  <copyright file="DbContextPkKeyExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#endregion

namespace PagedListResult.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Database context primary key extensions.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    internal static class DbContextPkKeyExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Search/find primary key names.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="dbContext">Db context.</param>
        /// <param name="entity">Entity to search.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the primary keys name lists in
        ///     this collection.
        /// </returns>
        ///=================================================================================================
        internal static IEnumerable<string> FindPrimaryKeysNameList<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
            => from p in dbContext.FindPrimaryKeyProperties(entity)
                select p.Name;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Search/find for primary key values.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="dbContext">Db context.</param>
        /// <param name="entity">Entity.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the primary keys value lists in
        ///     this collection.
        /// </returns>
        ///=================================================================================================
        internal static IEnumerable<object> FindPrimaryKeysValueList<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
            => from p in dbContext.FindPrimaryKeyProperties(entity)
                select entity.GetPropertyValue(p.Name);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Search for primary key properties.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="dbContext">Db context.</param>
        /// <param name="entity">Entity.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the primary key properties in
        ///     this collection.
        /// </returns>
        ///=================================================================================================
        internal static IEnumerable<IProperty> FindPrimaryKeyProperties<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
            => dbContext.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get property value.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TEntity">Type entity.</typeparam>
        /// <param name="entity">Entity.</param>
        /// <param name="name">Prop name.</param>
        /// <returns>The property value.</returns>
        ///=================================================================================================
        private static object GetPropertyValue<TEntity>(this TEntity entity, string name) where TEntity : class
            => entity.GetType().GetProperty(name)?.GetValue(entity, null);
    }
}