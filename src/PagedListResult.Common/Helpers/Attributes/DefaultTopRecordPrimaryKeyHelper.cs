// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 22:35
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-02 22:51
// ***********************************************************************
//  <copyright file="DefaultTopRecordPrimaryKeyHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Attributes;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Extensions.Result;

#endregion

namespace PagedListResult.Common.Helpers.Attributes
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Default primary key property helper.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    internal class DefaultTopRecordPrimaryKeyHelper
    {
        /// <summary>Default order property.</summary>
        private PropertyInfo _defaultPkProperty;

        /// <summary>Has or not default order property.</summary>
        private bool? _hasDefaultProperty;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultTopRecordPrimaryKeyHelper" /> class.
        /// </summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Type.</param>
        ///=================================================================================================
        internal DefaultTopRecordPrimaryKeyHelper(Type type)
        {
            Type = type;
            Methods = new List<PropertyInfo>();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultTopRecordPrimaryKeyHelper" /> class.
        /// </summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Type.</param>
        /// <param name="propertyInfos">Properties.</param>
        ///=================================================================================================
        internal DefaultTopRecordPrimaryKeyHelper(Type type, IList<PropertyInfo> propertyInfos)
        {
            Type = type;
            Methods = propertyInfos;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Type.</summary>
        /// <value>The type.</value>
        ///=================================================================================================
        internal Type Type { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Methods.</summary>
        /// <value>The methods.</value>
        ///=================================================================================================
        internal IList<PropertyInfo> Methods { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets default primary key property.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <returns>The default primary key property.</returns>
        ///=================================================================================================
        internal IResult<PropertyInfo> GetDefaultPrimaryKeyProperty()
        {
            try
            {
                var defaultPkProperty = Methods.FirstOrDefault(x => Attribute.IsDefined(x, typeof(PaginationDefaultTopRecordPrimaryKeyAttribute)));
                _hasDefaultProperty = defaultPkProperty.IsNotNull();

                if (_defaultPkProperty.IsNotNull())
                    return Result<PropertyInfo>.Success(_defaultPkProperty);

                if (_hasDefaultProperty.IsFalse())
                    return Result<PropertyInfo>.Success(null);

                return Result<PropertyInfo>.Success(_defaultPkProperty ??= defaultPkProperty);
            }
            catch (Exception e)
            {
                return Result<PropertyInfo>.Failure().WithError(e);
            }
        }
    }
}