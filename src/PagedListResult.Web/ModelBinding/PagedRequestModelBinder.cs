// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 20:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 21:34
// ***********************************************************************
//  <copyright file="PagedRequestModelBinder.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RzR.Extensions.Domain.Primitives;
using RzR.ResultMessage.Pagination.Abstractions.Models.Request.Page;
using RzR.ResultMessage.Pagination.AspNetCore.Abstractions;
using RzR.ResultMessage.Pagination.AspNetCore.Helpers;
using RzR.ResultMessage.Pagination.AspNetCore.Models;
using System;
using System.Threading.Tasks;

#endregion

namespace RzR.ResultMessage.Pagination.AspNetCore.ModelBinding
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     MVC <see cref="IModelBinder" /> that hydrates a <see cref="PagedRequest" /> (or
    ///     <see cref="PageRequestWithFilters" />) parameter from the request query string via
    ///     <see cref="PagedRequestQueryParser" />, applying allow-list validation when the
    ///     parameter is decorated with <c>[FromPagedQuery(typeof(TEntity))]</c>.
    /// </summary>
    /// <remarks>
    ///     Activated by <see cref="PagedRequestModelBinderProvider" /> which is registered at
    ///     position <c>0</c> in <c>MvcOptions.ModelBinderProviders</c> by
    ///     <c>AddPagedListResultWeb()</c>. Parser errors are copied to <see cref="ModelStateDictionary" />
    ///     so an <c>[ApiController]</c> automatically returns <c>400 ValidationProblem</c>.
    /// </remarks>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder"/>
    /// =================================================================================================
    public sealed class PagedRequestModelBinder : IModelBinder
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) type of the entity.
        /// </summary>
        /// =================================================================================================
        private readonly Type _entityType;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) type of the model.
        /// </summary>
        /// =================================================================================================
        private readonly Type _modelType;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="modelType">The concrete request type to instantiate.</param>
        /// <param name="entityType">Optional entity type for allow-list look-up.</param>
        /// =================================================================================================
        public PagedRequestModelBinder(Type modelType, Type entityType)
        {
            _modelType = modelType ?? throw new ArgumentNullException(nameof(modelType));
            _entityType = entityType;
        }

        /// <inheritdoc/>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.IsNull())
                throw new ArgumentNullException(nameof(bindingContext));

            var request = Activator.CreateInstance(_modelType);
            var http = bindingContext.HttpContext;
            var options = ResolveOptions(http);
            var allowList = ResolveAllowList(http);
            var parseResult = ParseQuery(request, http.Request.Query, options, allowList);

            if (parseResult.IsValid.IsFalse())
            {
                CopyErrorsToModelState(parseResult, bindingContext.ModelState);

                if (request is IPagedQueryValidation pagedQueryValidation)
                {
                    foreach (var kvp in parseResult.Errors)
                    {
                        pagedQueryValidation.Errors[kvp.Key] = kvp.Value;
                    }

                    bindingContext.Result = ModelBindingResult.Success(request);

                    return Task.CompletedTask;
                }

                bindingContext.Result = ModelBindingResult.Failed();

                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(request);

            return Task.CompletedTask;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Resolve options.
        /// </summary>
        /// <param name="http">The HTTP.</param>
        /// <returns>
        ///     A list of.
        /// </returns>
        /// =================================================================================================
        private static PagedListResultWebOptions ResolveOptions(HttpContext http) 
            => http.RequestServices.GetService<IOptions<PagedListResultWebOptions>>()?.Value ?? new PagedListResultWebOptions();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Resolve allow list.
        /// </summary>
        /// <param name="http">The HTTP.</param>
        /// <returns>
        ///     A PageableMetadata.
        /// </returns>
        /// =================================================================================================
        private PageableMetadata ResolveAllowList(HttpContext http)
        {
            if (_entityType.IsNull())
                return null;

            return http.RequestServices.GetService<IPageableMetadataRegistry>()?.Get(_entityType);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse query.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="query">The query.</param>
        /// <param name="options">Options for controlling the operation.</param>
        /// <param name="allowList">List of allows.</param>
        /// <returns>
        ///     A PagedRequestQueryParser.PagedParseResult.
        /// </returns>
        /// =================================================================================================
        private static PagedParseResult ParseQuery(
            object request, IQueryCollection query,
            PagedListResultWebOptions options, PageableMetadata allowList)
            => request is PageRequestWithFilters withFilters
                ? PagedRequestQueryParser.Populate(withFilters, query, options, allowList)
                : PagedRequestQueryParser.Populate((PagedRequest)request, query, options, allowList);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Copies the errors to model state.
        /// </summary>
        /// <param name="parseResult">The parse result.</param>
        /// <param name="modelState">State of the model.</param>
        /// =================================================================================================
        private static void CopyErrorsToModelState(
            PagedParseResult parseResult, ModelStateDictionary modelState)
        {
            foreach (var kvp in parseResult.Errors)
            {
                modelState.AddModelError(kvp.Key, kvp.Value);
            }
        }
    }
}