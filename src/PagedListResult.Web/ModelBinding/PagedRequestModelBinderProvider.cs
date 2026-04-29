// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 19:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 21:36
// ***********************************************************************
//  <copyright file="PagedRequestModelBinderProvider.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using RzR.Extensions.Domain.Primitives;
using RzR.ResultMessage.Pagination.Abstractions.Models.Request.Page;
using RzR.ResultMessage.Pagination.AspNetCore.Attributes;
using System;
using System.Linq;

#endregion

namespace RzR.ResultMessage.Pagination.AspNetCore.ModelBinding
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     MVC binder provider that returns <see cref="PagedRequestModelBinder" /> for parameters of
    ///     type <see cref="PagedRequest" /> (or any subclass) that are decorated with
    ///     <see cref="FromPagedQueryAttribute" />.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderProvider"/>
    /// =================================================================================================
    public sealed class PagedRequestModelBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc/>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.IsNull()) 
                throw new ArgumentNullException(nameof(context));

            var modelType = context.Metadata.ModelType;
            if (!typeof(PagedRequest).IsAssignableFrom(modelType))
                return null;

            // Locate the [FromPagedQuery] attribute (if any) for the entity type hint.
            FromPagedQueryAttribute attr = null;
            if (context.Metadata is DefaultModelMetadata dmd)
            {
                attr = dmd.Attributes.ParameterAttributes?
                    .OfType<FromPagedQueryAttribute>()
                    .FirstOrDefault();
            }

            // The parameter must be opted-in via [FromPagedQuery]; otherwise let the
            // default complex-object binder run (back-compat with body-bound usages).
            if (attr.IsNull())
                return null;

            return new PagedRequestModelBinder(modelType, attr!.EntityType);
        }
    }
}