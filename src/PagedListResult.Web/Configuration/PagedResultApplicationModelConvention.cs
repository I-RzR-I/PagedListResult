// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-26 20:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 20:46
// ***********************************************************************
//  <copyright file="PagedResultApplicationModelConvention.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using RzR.Extensions.Domain.Primitives;
using RzR.ResultMessage.Pagination.DataModels.Abstractions;
using RzR.ResultMessage.Pagination.DataModels.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Configuration
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Application model convention that auto-attaches OpenAPI / response-type metadata to every
    ///     action whose return type resolves to <see cref="IPagedResult{T}" /> (or
    ///     <see cref="PagedResult{T}" />), unwrapping common containers (<see cref="Task{TResult}" />
    ///     ,
    ///     <see cref="ValueTask{TResult}" />, <see cref="ActionResult{TValue}" />).
    ///     <para>
    ///         For each matched action the convention adds (without overriding existing user
    ///         attributes):
    ///         <list type="bullet">
    ///             <item><c>[Produces("application/json")]</c></item>
    ///             <item><c>[ProducesResponseType(typeof(PagedResult&lt;T&gt;), 200)]</c></item>
    ///             <item><c>[ProducesResponseType(typeof(ProblemDetails), 400)]</c></item>
    ///         </list>
    ///     </para>
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.IApplicationModelConvention"/>
    /// <seealso cref="IApplicationModelConvention"/>
    /// =================================================================================================
    internal sealed class PagedResultApplicationModelConvention : IApplicationModelConvention
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) type of the JSON content.
        /// </summary>
        /// =================================================================================================
        private const string JsonContentType = "application/json";

        /// <inheritdoc/>
        public void Apply(ApplicationModel application)
        {
            if (application.IsNull())
                return;

            foreach (var controller in application.Controllers)
            {
                foreach (var action in controller.Actions)
                {
                    ApplyToAction(action);
                }
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies to action described by action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// =================================================================================================
        private static void ApplyToAction(ActionModel action)
        {
            var pagedItemType = ResolvePagedItemType(action);
            if (pagedItemType.IsNull())
                return;

            var closedPagedResult = typeof(PagedResult<>).MakeGenericType(pagedItemType);
            var declaredStatusCodes = CollectDeclaredStatusCodes(action);

            EnsureProducesJson(action);
            EnsureProducesResponseType(action, closedPagedResult, StatusCodes.Status200OK, declaredStatusCodes);
            EnsureProducesResponseType(action, typeof(ProblemDetails), StatusCodes.Status400BadRequest, declaredStatusCodes);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Resolve paged item type.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>
        ///     A Type.
        /// </returns>
        /// =================================================================================================
        private static Type ResolvePagedItemType(ActionModel action)
        {
            var returnType = action.ActionMethod?.ReturnType;
            if (returnType.IsNull() || returnType == typeof(void))
                return null;

            returnType = Unwrap(returnType);

            return ExtractPagedItemType(returnType);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Unwraps the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     A Type.
        /// </returns>
        /// =================================================================================================
        private static Type Unwrap(Type type)
        {
            // Task<T>, ValueTask<T>, ActionResult<T> -> T (recursively)
            while (type.IsGenericType)
            {
                var genericDef = type.GetGenericTypeDefinition();
                if (genericDef == typeof(Task<>) || genericDef == typeof(ValueTask<>) || genericDef == typeof(ActionResult<>))
                {
                    type = type.GetGenericArguments()[0];
                    continue;
                }

                break;
            }

            return type;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Extracts the paged item type described by type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     The extracted paged item type.
        /// </returns>
        /// =================================================================================================
        private static Type ExtractPagedItemType(Type type)
        {
            if (!type.IsGenericType)
                return null;

            var genericDef = type.GetGenericTypeDefinition();
            if (genericDef == typeof(IPagedResult<>) || genericDef == typeof(PagedResult<>))
                return type.GetGenericArguments()[0];

            // Type implements IPagedResult<T>?
            foreach (var iface in type.GetInterfaces())
            {
                if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IPagedResult<>))
                    return iface.GetGenericArguments()[0];
            }

            return null;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Collect declared status codes.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>
        ///     A HashSet&lt;int&gt;
        /// </returns>
        /// =================================================================================================
        private static HashSet<int> CollectDeclaredStatusCodes(ActionModel action)
        {
            var codes = new HashSet<int>();
            foreach (var attr in action.Attributes)
            {
                if (attr is IApiResponseMetadataProvider apiMeta)
                    codes.Add(apiMeta.StatusCode);
            }

            foreach (var filter in action.Filters)
            {
                if (filter is IApiResponseMetadataProvider apiMeta)
                    codes.Add(apiMeta.StatusCode);
            }

            return codes;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Ensures that produces JSON.
        /// </summary>
        /// <param name="action">The action.</param>
        /// =================================================================================================
        private static void EnsureProducesJson(ActionModel action)
        {
            var hasProduces = action.Attributes.OfType<ProducesAttribute>().Any()
                              || action.Filters.OfType<ProducesAttribute>().Any()
                              || action.Controller.Attributes.OfType<ProducesAttribute>().Any()
                              || action.Controller.Filters.OfType<ProducesAttribute>().Any();
            if (hasProduces)
                return;

            action.Filters.Add(new ProducesAttribute(JsonContentType));
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Ensures that produces response type.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="declaredStatusCodes">The declared status codes.</param>
        /// =================================================================================================
        private static void EnsureProducesResponseType(
            ActionModel action,
            Type responseType,
            int statusCode,
            HashSet<int> declaredStatusCodes)
        {
            if (declaredStatusCodes.Contains(statusCode))
                return;

            action.Filters.Add(new ProducesResponseTypeAttribute(responseType, statusCode));
            declaredStatusCodes.Add(statusCode);
        }
    }
}