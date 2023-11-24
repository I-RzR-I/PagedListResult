// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-11-02 22:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-14 00:52
// ***********************************************************************
//  <copyright file="DefaultOrderPropertyHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using AggregatedGenericResultMessage.Extensions.Result;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace PagedListResult.Common.Helpers.Attributes
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>Default order property helper.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    /// =================================================================================================
    internal class DefaultOrderPropertyHelper
    {
        /// <summary>Default order property.</summary>
        private PropertyInfo _defaultOrderProperty;

        /// <summary>Has or not default order property.</summary>
        private bool? _hasDefaultProperty;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultOrderPropertyHelper" /> class.
        /// </summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Type.</param>
        /// =================================================================================================
        internal DefaultOrderPropertyHelper(Type type)
        {
            Type = type;
            Methods = new List<PropertyInfo>();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultOrderPropertyHelper" /> class.
        /// </summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Type.</param>
        /// <param name="propertyInfos">Properties.</param>
        /// =================================================================================================
        internal DefaultOrderPropertyHelper(Type type, IList<PropertyInfo> propertyInfos)
        {
            Type = type;
            Methods = propertyInfos;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Type.</summary>
        /// <value>The type.</value>
        /// =================================================================================================
        internal Type Type { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Methods.</summary>
        /// <value>The methods.</value>
        /// =================================================================================================
        internal IList<PropertyInfo> Methods { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Get default order property.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <returns>The default order property.</returns>
        /// =================================================================================================
        internal IResult<PropertyInfo> GetDefaultOrderProperty()
        {
            try
            {
                var defaultOrderProperty = Methods.FirstOrDefault(x => Attribute.IsDefined(x, typeof(PaginationDefaultOrderPropertyAttribute)));
                _hasDefaultProperty = defaultOrderProperty.IsNotNull();

                if (_defaultOrderProperty.IsNotNull())
                    return Result<PropertyInfo>.Success(_defaultOrderProperty);

                if (_hasDefaultProperty.IsFalse())
                    return Result<PropertyInfo>.Success(null);

                return Result<PropertyInfo>.Success(_defaultOrderProperty ??= defaultOrderProperty);
            }
            catch (Exception e)
            {
                return Result<PropertyInfo>.Failure().WithError(e);
            }
        }
    }
}