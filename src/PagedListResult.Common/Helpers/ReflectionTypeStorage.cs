// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Common
//  Author           : RzR
//  Created On       : 2023-10-24 17:13
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-24 17:23
// ***********************************************************************
//  <copyright file="ReflectionTypeStorage.cs" company="">
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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using PagedListResult.Common.Helpers.Attributes;
using PagedListResult.Common.Helpers.Internal.Common;

#endregion

namespace PagedListResult.Common.Helpers
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Reflection storage type.</summary>
    /// <remarks>RzR, 14-Nov-23.</remarks>
    ///=================================================================================================
    public static class ReflectionTypeStorage
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable)
        ///     Concurrent dictionary store default order.
        /// </summary>
        ///=================================================================================================
        private static readonly ConcurrentDictionary<string, DefaultOrderPropertyHelper> DefaultOrderStorage
            = new ConcurrentDictionary<string, DefaultOrderPropertyHelper>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable)
        ///     Concurrent dictionary store default order.
        /// </summary>
        ///=================================================================================================
        private static readonly ConcurrentDictionary<string, DefaultTopRecordPrimaryKeyHelper> DefaultTopRecordPrimaryKeyStorage
            = new ConcurrentDictionary<string, DefaultTopRecordPrimaryKeyHelper>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get properties default order.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Required.Type.</param>
        /// <param name="filterPredicate">(Optional) Optional. The default value is null.</param>
        /// <returns>The default order properties.</returns>
        ///=================================================================================================
        public static IResult<IList<PropertyInfo>> GetDefaultOrderProperties(Type type, Func<PropertyInfo, bool> filterPredicate = null)
        {
            try
            {
                var descriptor = GetDescriptorDefaultOrderProperty(type);
                if (descriptor.IsSuccess.IsFalse())
                    ThrowHelper.Exception(descriptor.GetFirstMessage());

                return Result<IList<PropertyInfo>>
                    .Success(filterPredicate.IsNull()
                        ? descriptor.Response.Methods
                        : descriptor.Response.Methods.Where(filterPredicate!).ToList());
            }
            catch (Exception e)
            {
                return Result<IList<PropertyInfo>>.Failure().WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get properties default order.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Required.Type.</param>
        /// <param name="filterPredicate">(Optional) Optional. The default value is null.</param>
        /// <returns>The default top record primary key properties.</returns>
        ///=================================================================================================
        public static IResult<IList<PropertyInfo>> GetDefaultTopRecordPrimaryKeyProperties(Type type, Func<PropertyInfo, bool> filterPredicate = null)
        {
            try
            {
                var descriptor = GetDescriptorDefaultTopRecordPrimaryKeyProperty(type);
                if (descriptor.IsSuccess.IsFalse())
                    ThrowHelper.Exception(descriptor.GetFirstMessage());

                return Result<IList<PropertyInfo>>
                    .Success(filterPredicate.IsNull()
                        ? descriptor.Response.Methods
                        : descriptor.Response.Methods.Where(filterPredicate!).ToList());
            }
            catch (Exception e)
            {
                return Result<IList<PropertyInfo>>.Failure().WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get type property by name.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Type.</param>
        /// <param name="propertyName">.</param>
        /// <returns>The default order property by name.</returns>
        ///=================================================================================================
        public static IResult<PropertyInfo> GetDefaultOrderPropertyByName(Type type, string propertyName)
        {
            try
            {
                propertyName.ThrowIfArgNullOrEmpty(nameof(propertyName));
                var descriptor = GetDescriptorDefaultOrderProperty(type);
                if (descriptor.IsSuccess.IsFalse())
                    ThrowHelper.Exception(descriptor.GetFirstMessage());

                return Result<PropertyInfo>
                    .Success(descriptor.Response.Methods.FirstOrDefault(x => x.Name == propertyName));
            }
            catch (Exception e)
            {
                return Result<PropertyInfo>.Failure().WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get type property by name.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Type.</param>
        /// <param name="propertyName">.</param>
        /// <returns>The default top record primary key property by name.</returns>
        ///=================================================================================================
        public static IResult<PropertyInfo> GetDefaultTopRecordPrimaryKeyPropertyByName(Type type, string propertyName)
        {
            try
            {
                propertyName.ThrowIfArgNullOrEmpty(nameof(propertyName));
                var descriptor = GetDescriptorDefaultTopRecordPrimaryKeyProperty(type);
                if (descriptor.IsSuccess.IsFalse())
                    ThrowHelper.Exception(descriptor.GetFirstMessage());

                return Result<PropertyInfo>
                    .Success(descriptor.Response.Methods.FirstOrDefault(x => x.Name == propertyName));
            }
            catch (Exception e)
            {
                return Result<PropertyInfo>.Failure().WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get default order by property.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Type.</param>
        /// <returns>The default order by property information.</returns>
        ///=================================================================================================
        public static IResult<PropertyInfo> GetDefaultOrderByPropertyInfo(Type type)
        {
            var descriptor = GetDescriptorDefaultOrderProperty(type);
            if (descriptor.IsSuccess.IsFalse())
                ThrowHelper.Exception(descriptor.GetFirstMessage());

            return descriptor.Response.GetDefaultOrderProperty();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get default primary key property by property.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Type.</param>
        /// <returns>The default top record primary key property information.</returns>
        ///=================================================================================================
        public static IResult<PropertyInfo> GetDefaultDefaultTopRecordPrimaryKeyPropertyInfo(Type type)
        {
            var descriptor = GetDescriptorDefaultTopRecordPrimaryKeyProperty(type);
            if (descriptor.IsSuccess.IsFalse())
                ThrowHelper.Exception(descriptor.GetFirstMessage());

            return descriptor.Response.GetDefaultPrimaryKeyProperty();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get property value as delegate.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TDataType">Delegate type.</typeparam>
        /// <param name="propertyInfo">Property info.</param>
        /// <returns>The property value as delegate.</returns>
        ///=================================================================================================
        public static IResult<Func<TSource, TDataType>> GetPropertyValueAsDelegate<TSource, TDataType>(PropertyInfo propertyInfo)
        {
            try
            {
                propertyInfo.ThrowIfArgNull(nameof(propertyInfo));

                return Result<Func<TSource, TDataType>>
                    .Success((Func<TSource, TDataType>)Delegate.CreateDelegate(
                        typeof(Func<TSource, TDataType>), propertyInfo.GetGetMethod()!));
            }
            catch (Exception e)
            {
                return Result<Func<TSource, TDataType>>.Failure().WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get default order property descriptor.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Type.</param>
        /// <returns>The descriptor default order property.</returns>
        ///=================================================================================================
        private static IResult<DefaultOrderPropertyHelper> GetDescriptorDefaultOrderProperty(Type type)
        {
            type.ThrowIfNull("Type must be non null!");
            try
            {
                var key = type.FullName;
                key.ThrowIfNull("Type full name must be non empty");

                if (DefaultOrderStorage.ContainsKey(key!))
                    return Result<DefaultOrderPropertyHelper>.Success(DefaultOrderStorage[key]);

                var description = new DefaultOrderPropertyHelper(type, type.GetProperties());

                DefaultOrderStorage.TryAdd(key, description);

                return Result<DefaultOrderPropertyHelper>.Success(description);
            }
            catch (Exception e)
            {
                return Result<DefaultOrderPropertyHelper>.Failure().WithError(e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get default primary key property descriptor.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <param name="type">Type.</param>
        /// <returns>The descriptor default top record primary key property.</returns>
        ///=================================================================================================
        private static IResult<DefaultTopRecordPrimaryKeyHelper> GetDescriptorDefaultTopRecordPrimaryKeyProperty(Type type)
        {
            type.ThrowIfNull("Type must be non null!");
            try
            {
                var key = type.FullName;
                key.ThrowIfNull("Type full name must be non empty");

                if (DefaultTopRecordPrimaryKeyStorage.ContainsKey(key!))
                    return Result<DefaultTopRecordPrimaryKeyHelper>.Success(DefaultTopRecordPrimaryKeyStorage[key]);

                var description = new DefaultTopRecordPrimaryKeyHelper(type, type.GetProperties());

                DefaultTopRecordPrimaryKeyStorage.TryAdd(key, description);

                return Result<DefaultTopRecordPrimaryKeyHelper>.Success(description);
            }
            catch (Exception e)
            {
                return Result<DefaultTopRecordPrimaryKeyHelper>.Failure().WithError(e);
            }
        }
    }
}