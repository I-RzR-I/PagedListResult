// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-07 23:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-07 23:02
// ***********************************************************************
//  <copyright file="DefaultPrimaryKeyDefinition.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

// ReSharper disable ClassNeverInstantiated.Global
namespace PagedListResult.Common.Models.Request
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Default primary key definition DTO.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    public class DefaultPrimaryKeyDefinition
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Default constructor.</summary>
        /// <remarks>RzR, 23-Nov-23.</remarks>
        ///=================================================================================================
        public DefaultPrimaryKeyDefinition()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        /// <remarks>RzR, 23-Nov-23.</remarks>
        /// <param name="defaultPrimaryKey">The default primary key.</param>
        ///=================================================================================================
        public DefaultPrimaryKeyDefinition(string defaultPrimaryKey) => DefaultPrimaryKey = defaultPrimaryKey;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        /// <remarks>RzR, 23-Nov-23.</remarks>
        /// <param name="defaultPrimaryKey">The default primary key.</param>
        /// <param name="findByAttribute">True if find by attribute, false if not.</param>
        /// <param name="findByEntity">True if find by entity, false if not.</param>
        ///=================================================================================================
        public DefaultPrimaryKeyDefinition(string defaultPrimaryKey, bool findByAttribute, bool findByEntity)
        {
            DefaultPrimaryKey = defaultPrimaryKey;
            FindByAttribute = findByAttribute;
            FindByEntity = findByEntity;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Default primary key property name.</summary>
        /// <value>The default primary key.</value>
        ///=================================================================================================
        public string DefaultPrimaryKey { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Find default primary.</summary>
        /// <value>True if find by attribute, false if not.</value>
        ///=================================================================================================
        public bool FindByAttribute { get; set; } = false;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Find primary key be entity.</summary>
        /// <value>True if find by entity, false if not.</value>
        ///=================================================================================================
        public bool FindByEntity { get; set; } = false;
    }
}