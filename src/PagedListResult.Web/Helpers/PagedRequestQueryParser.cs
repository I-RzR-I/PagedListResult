// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 20:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 21:38
// ***********************************************************************
//  <copyright file="PagedRequestQueryParser.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;
using RzR.ResultMessage.Pagination.DataModels.Enums;
using RzR.ResultMessage.Pagination.DataModels.Models.Request;
using RzR.ResultMessage.Pagination.DataModels.Models.Request.Page;
using RzR.ResultMessage.Pagination.Web.ModelBinding;
using RzR.ResultMessage.Pagination.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace RzR.ResultMessage.Pagination.Web.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Shared, framework-agnostic parser that maps an <see cref="IQueryCollection" /> into the
    ///     existing <see cref="PagedRequest" /> / <see cref="PageRequestWithFilters" /> shapes. Used
    ///     by the MVC <see cref="PagedRequestModelBinder" /> and by the minimal-API
    ///     <c>BindAsync</c> wrappers (<c>PagedQuery&lt;T&gt;</c>, <c>PagedQueryWithFilters&lt;T&gt;</c>)
    ///     so both pipelines share identical parsing semantics.
    /// </summary>
    /// <remarks>
    ///     <para>Supported query-string syntax:</para>
    ///     <list type="bullet">
    ///         <item><c>?page=2&amp;pageSize=20</c> — pagination (rejects values past <c>MaxPageSize</c>).</item>
    ///         <item><c>?search=foo</c> — free-text search.</item>
    ///         <item><c>?searchAll=true|text|fields|all|false|none</c> — search-mode flag.</item>
    ///         <item><c>?searchFields=name,description</c> — restrict free-text search to listed properties.</item>
    ///         <item><c>?order=name:desc</c> — single-key sort (<c>asc</c>/<c>desc</c>, case-insensitive).</item>
    ///         <item><c>?filter=status:Equals:active&amp;filter=price:GreaterThan:100</c> — repeatable filter.</item>
    ///         <item><c>?filter=price:Between:10,20</c> — left/right (Values[0] = 10, CompareValue = 20).</item>
    ///         <item><c>?filter=status:IsIn:active,draft</c> — multi-value (all tokens land in <c>Values</c>).</item>
    ///         <item><c>?filter=name:IsNull</c> / <c>?filter=name:IsNotNull</c> — value-less.</item>
    ///         <item><c>?fields=id,name,price</c> — projection / partial response.</item>
    ///         <item><c>?predefinedField=Id&amp;predefinedRecords=1,2,3</c> — predefined record lookup.</item>
    ///     </list>
    ///     <para>
    ///         When a <see cref="PageableMetadata" /> allow-list is supplied, sort / filter /
    ///         search property names are rejected with a parser error <em>before</em> any
    ///         expression-builder reflection runs.
    ///     </para>
    ///     <para>
    ///         <example>
    ///             Manual parsing in a custom binding pipeline:
    ///             <code>
    ///             var req = new PagedRequest();
    ///             var parse = PagedRequestQueryParser.Populate(req, httpContext.Request.Query, options);
    ///             if (!parse.IsValid)
    ///                 return Results.ValidationProblem(parse.Errors.ToDictionary(e => e.Key, e => new[] { e.Value }));
    ///             </code>
    ///         </example>
    ///     </para>
    /// </remarks>
    /// =================================================================================================
    public static class PagedRequestQueryParser
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Populate <paramref name="target" /> from <paramref name="query" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="target">The request instance to fill.</param>
        /// <param name="query">The HTTP query collection.</param>
        /// <param name="options">
        ///     Web options (used for <c>MaxPageSize</c>/<c>DefaultPageSize</c>).
        /// </param>
        /// <param name="allowList">
        ///     (Optional) Optional per-entity allow-list for sort/filter/search validation.
        /// </param>
        /// <returns>
        ///     A ParseResult.
        /// </returns>
        /// =================================================================================================
        public static PagedParseResult Populate(
            PagedRequest target,
            IQueryCollection query,
            PagedListResultWebOptions options,
            PageableMetadata allowList = null)
        {
            if (target.IsNull()) 
                throw new ArgumentNullException(nameof(target));
            if (query.IsNull()) 
                throw new ArgumentNullException(nameof(query));
            if (options.IsNull())
                throw new ArgumentNullException(nameof(options));

            var result = new PagedParseResult();

            ParsePaging(target, query, options, result);
            ParseSearch(target, query, allowList, result);
            ParseOrder(target, query, allowList, result);
            ParseFields(target, query);
            ParsePredefined(target, query);

            return result;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Populate the <see cref="PageRequestWithFilters" /> overload (calls the base populate and
        ///     then maps the repeatable <c>filter</c> query parameter into
        ///     <see cref="PageRequestWithFilters.Filters" />).
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="target">The request instance to fill.</param>
        /// <param name="query">The HTTP query collection.</param>
        /// <param name="options">
        ///     Web options (used for <c>MaxPageSize</c>/<c>DefaultPageSize</c>).
        /// </param>
        /// <param name="allowList">
        ///     (Optional) Optional per-entity allow-list for sort/filter/search validation.
        /// </param>
        /// <returns>
        ///     A ParseResult.
        /// </returns>
        /// =================================================================================================
        public static PagedParseResult Populate(
            PageRequestWithFilters target,
            IQueryCollection query,
            PagedListResultWebOptions options,
            PageableMetadata allowList = null)
        {
            if (target.IsNull()) 
                throw new ArgumentNullException(nameof(target));

            var result = Populate((PagedRequest)target, query, options, allowList);
            ParseFilters(target, query, allowList, result);

            return result;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse paging.
        /// </summary>
        /// <param name="target">The request instance to fill.</param>
        /// <param name="query">The HTTP query collection.</param>
        /// <param name="options">
        ///     Web options (used for <c>MaxPageSize</c>/<c>DefaultPageSize</c>).
        /// </param>
        /// <param name="result">The result.</param>
        /// =================================================================================================
        private static void ParsePaging(
            PagedRequest target, IQueryCollection query,
            PagedListResultWebOptions options, PagedParseResult result)
        {
            if (TryGetInt(query, PagedQueryKeys.Page, result, out var page))
            {
                if (page < 1) 
                    result.Errors[PagedQueryKeys.Page] = "Must be >= 1.";
                else 
                    target.Page = page;
            }

            if (TryGetInt(query, PagedQueryKeys.PageSize, result, out var pageSize))
            {
                if (pageSize < 1)
                    result.Errors[PagedQueryKeys.PageSize] = "Must be >= 1.";
                else if (pageSize > options.MaxPageSize)
                    result.Errors[PagedQueryKeys.PageSize] = $"Exceeds MaxPageSize ({options.MaxPageSize}).";
                else
                    target.PageSize = pageSize;
            }
            else if (target.PageSize <= 0) 
                target.PageSize = options.DefaultPageSize > 0 ? options.DefaultPageSize : 10;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse search.
        /// </summary>
        /// <param name="target">The request instance to fill.</param>
        /// <param name="query">The HTTP query collection.</param>
        /// <param name="allowList">
        ///     (Optional) Optional per-entity allow-list for sort/filter/search validation.
        /// </param>
        /// <param name="result">The result.</param>
        /// =================================================================================================
        private static void ParseSearch(
            PagedRequest target, IQueryCollection query,
            PageableMetadata allowList, PagedParseResult result)
        {
            target.Search ??= new DataSearchDefinition();

            if (query.TryGetValue(PagedQueryKeys.Search, out var searchVals) && searchVals.Count > 0)
                target.Search.Search = searchVals[0];

            if (query.TryGetValue(PagedQueryKeys.SearchAll, out var searchAllVals) && searchAllVals.Count > 0)
                ApplySearchMode(target.Search, searchAllVals[0], result);

            if (query.TryGetValue(PagedQueryKeys.SearchFields, out var sfVals) && sfVals.Count > 0)
                ApplySearchFields(target.Search, sfVals, allowList, result);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the search mode.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="raw">The raw.</param>
        /// <param name="result">The result.</param>
        /// =================================================================================================
        private static void ApplySearchMode(DataSearchDefinition search, string raw, PagedParseResult result)
        {
            switch (raw.IfNullThenEmpty().Trim().ToLowerInvariant())
            {
                case "true":
                case "text":
                    search.SearchInAllTextFields = true;
                    search.SearchInAllFields = false;
                    break;
                case "fields":
                case "all":
                    search.SearchInAllTextFields = false;
                    search.SearchInAllFields = true;
                    break;
                case "false":
                case "none":
                    search.SearchInAllTextFields = false;
                    search.SearchInAllFields = false;
                    break;
                default:
                    result.Errors[PagedQueryKeys.SearchAll] =
                        "Expected one of: true|text|fields|all|false|none.";
                    break;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the search fields.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="values">The values.</param>
        /// <param name="allowList">
        ///     (Optional) Optional per-entity allow-list for sort/filter/search validation.
        /// </param>
        /// <param name="result">The result.</param>
        /// =================================================================================================
        private static void ApplySearchFields(
            DataSearchDefinition search, StringValues values,
            PageableMetadata allowList, PagedParseResult result)
        {
            search.CustomSearchTextProperties ??= new HashSet<string>();
            foreach (var prop in SplitTokenComma(values))
            {
                if (allowList.IsNotNull() && allowList.IsSearchAllowed(prop).IsFalse())
                {
                    result.Errors[PagedQueryKeys.SearchFields] =
                        $"Property '{prop}' is not in the search allow-list.";
                    continue;
                }

                search.CustomSearchTextProperties.Add(prop);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse order.
        /// </summary>
        /// <param name="target">The request instance to fill.</param>
        /// <param name="query">The HTTP query collection.</param>
        /// <param name="allowList">
        ///     (Optional) Optional per-entity allow-list for sort/filter/search validation.
        /// </param>
        /// <param name="result">The result.</param>
        /// =================================================================================================
        private static void ParseOrder(
            PagedRequest target, IQueryCollection query,
            PageableMetadata allowList, PagedParseResult result)
        {
            target.Order ??= new DataOrderDefinition();

            if (!query.TryGetValue(PagedQueryKeys.Order, out var orderVals) || orderVals.Count == 0)
                return;

            // Accept both repeated keys and a comma-separated single value.
            var tokens = orderVals
                .SelectMany(v => SplitTokenComma(v))
                .Where(t => t.IsPresent())
                .ToList();

            if (tokens.Count == 0) 
                return;

            ApplyOrderToken(target.Order, tokens[0], allowList, result);

            if (tokens.Count > 1)
            {
                result.Warnings.Add(
                    $"Multi-key '{PagedQueryKeys.Order}' is not yet supported; ignored extras: " +
                    string.Join(",", tokens.Skip(1)));
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the order token.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="token">The token.</param>
        /// <param name="allowList">
        ///     (Optional) Optional per-entity allow-list for sort/filter/search validation.
        /// </param>
        /// <param name="result">The result.</param>
        /// =================================================================================================
        private static void ApplyOrderToken(
            DataOrderDefinition order, string token,
            PageableMetadata allowList, PagedParseResult result)
        {
            var parts = token.Split(':');
            var prop = parts[0].Trim();

            if (prop.IsMissing())
            {
                result.Errors[PagedQueryKeys.Order] = "Property name is required.";

                return;
            }

            if (allowList.IsNotNull() && allowList.IsSortAllowed(prop).IsFalse())
            {
                result.Errors[PagedQueryKeys.Order] = $"Property '{prop}' is not in the sort allow-list.";

                return;
            }

            order.OrderByProperty = prop;
            if (parts.Length < 2) 
                return;

            if (Enum.TryParse<OrderDirection>(parts[1].Trim(), true, out var dir))
                order.OrderDirection = dir;
            else
                result.Errors[PagedQueryKeys.Order] = $"Unknown direction '{parts[1]}'. Use 'asc' or 'desc'.";
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse fields.
        /// </summary>
        /// <param name="target">The request instance to fill.</param>
        /// <param name="query">The HTTP query collection.</param>
        /// =================================================================================================
        private static void ParseFields(PagedRequest target, IQueryCollection query)
        {
            if (!query.TryGetValue(PagedQueryKeys.Fields, out var fieldsVals) || fieldsVals.Count == 0)
                return;

            target.Fields.NotNull();
            foreach (var f in fieldsVals.SelectMany(v => SplitTokenComma(v)))
            {
                target.Fields.Add(f);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse predefined.
        /// </summary>
        /// <param name="target">The request instance to fill.</param>
        /// <param name="query">The HTTP query collection.</param>
        /// =================================================================================================
        private static void ParsePredefined(PagedRequest target, IQueryCollection query)
        {
            target.PredefinedRecord ??= new DataPredefinedFilterDefinition();

            if (query.TryGetValue(PagedQueryKeys.PredefinedField, out var pfVals) && pfVals.Count > 0)
                target.PredefinedRecord.PredefinedFieldName = pfVals[0];

            if (!query.TryGetValue(PagedQueryKeys.PredefinedRecords, out var prVals) || prVals.Count == 0)
                return;

            target.PredefinedRecord.PredefinedRecords ??= new HashSet<string>();
            foreach (var r in prVals.SelectMany(v => SplitTokenComma(v)))
            {
                target.PredefinedRecord.PredefinedRecords.Add(r);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse filters.
        /// </summary>
        /// <param name="target">The request instance to fill.</param>
        /// <param name="query">The HTTP query collection.</param>
        /// <param name="allowList">
        ///     (Optional) Optional per-entity allow-list for sort/filter/search validation.
        /// </param>
        /// <param name="result">The result.</param>
        /// =================================================================================================
        private static void ParseFilters(
            PageRequestWithFilters target, IQueryCollection query,
            PageableMetadata allowList, PagedParseResult result)
        {
            if (!query.TryGetValue(PagedQueryKeys.Filter, out var filterVals) || filterVals.Count == 0)
                return;

            // PageRequestWithFilters.Filters is initialized in the model, but be defensive.
            if (target.Filters.IsNull())
                target.Filters = new HashSet<DataFilter>();

            var order = 0;
            for (var i = 0; i < filterVals.Count; i++)
            {
                var raw = filterVals[i];
                if (raw.IsMissing())
                    continue;

                var filter = TryParseFilter(raw, order, i, allowList, result);
                if (filter.IsNotNull())
                {
                    target.Filters.Add(filter);
                    order++;
                }
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attempts to parse a filter from the given data, returning a default value rather than
        ///     throwing an exception if it fails.
        /// </summary>
        /// <param name="raw">The raw.</param>
        /// <param name="applyOrder">The apply order.</param>
        /// <param name="rowIndex">
        ///     The zero-based index of this filter within the repeated <c>?filter=</c> values; used to
        ///     scope per-row error keys (<c>filter[0]</c>, <c>filter[1]</c>, ...) so that multiple bad
        ///     filters do not overwrite each other in <see cref="PagedParseResult.Errors" />.
        /// </param>
        /// <param name="allowList">
        ///     (Optional) Optional per-entity allow-list for sort/filter/search validation.
        /// </param>
        /// <param name="result">The result.</param>
        /// <returns>
        ///     A DataFilter.
        /// </returns>
        /// =================================================================================================
        private static DataFilter TryParseFilter(
            string raw, int applyOrder, int rowIndex,
            PageableMetadata allowList, PagedParseResult result)
        {
            var errKey = $"{PagedQueryKeys.Filter}[{rowIndex}]";

            var parts = raw.Split(new[] { ':' }, 3);
            if (parts.Length < 3)
            {
                result.Errors[errKey] = $"Bad filter '{raw}'. Expected '<property>:<condition>:<value[,value2,...]>'.";

                return null;
            }

            var prop = parts[0].Trim();
            var condRaw = parts[1].Trim();
            var valRaw = parts[2];

            if (prop.IsMissing())
            {
                result.Errors[errKey] = $"Bad filter '{raw}'. Property name is required.";

                return null;
            }

            if (allowList.IsNotNull() && allowList.IsFilterAllowed(prop).IsFalse())
            {
                result.Errors[errKey] = $"Property '{prop}' is not in the filter allow-list.";

                return null;
            }

            if (!Enum.TryParse<FilterType>(condRaw, true, out var cond))
            {
                result.Errors[errKey] = $"Unknown filter condition '{condRaw}'.";

                return null;
            }

            var values = SplitTokenComma(valRaw);
            var filterValues = new HashSet<string>();
            string compareValue = null;

            switch (cond)
            {
                case FilterType.Between:
                    if (values.Count < 2)
                    {
                        result.Errors[errKey] =
                            $"Filter '{prop}:Between' requires two comma-separated values (e.g. 'price:Between:10,20').";

                        return null;
                    }
                    filterValues.Add(values[0]);
                    compareValue = values[1];
                    break;

                case FilterType.IsIn:
                case FilterType.IsNotIn:
                    foreach (var v in values) 
                        filterValues.Add(v);
                    break;

                case FilterType.IsNull:
                case FilterType.IsNotNull:
                    // No value expected.
                    break;

                default:
                    if (values.Count > 0)
                        filterValues.Add(values[0]);
                    break;
            }

            return new DataFilter
            {
                FilterApplyOrder = applyOrder,
                FilterValue = new DataFilterValue()
                {
                    Condition = cond,
                    PropertyName = prop,
                    Values = filterValues,
                    CompareValue = compareValue
                }
            };
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attempts to get int.
        /// </summary>
        /// <param name="query">The HTTP query collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="result">The result.</param>
        /// <param name="value">[out] The value.</param>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        /// =================================================================================================
        private static bool TryGetInt(IQueryCollection query, string key, PagedParseResult result, out int value)
        {
            value = 0;
            if (!query.TryGetValue(key, out var v) || v.Count == 0)
                return false;
            if (int.TryParse(v[0], out value)) 
                return true;

            result.Errors[key] = $"'{v[0]}' is not a valid integer.";

            return false;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Splits token comma.
        /// </summary>
        /// <param name="raw">The raw.</param>
        /// <returns>
        ///     A list of token.
        /// </returns>
        /// =================================================================================================
        private static IList<string> SplitTokenComma(string raw)
        {
            if (raw.IsMissing()) 
                return Array.Empty<string>();

            return raw
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => s.IsPresent())
                .ToList();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Splits token comma.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>
        ///     A list of token.
        /// </returns>
        /// =================================================================================================
        private static IEnumerable<string> SplitTokenComma(StringValues values)
        {
            var result = new List<string>();
            foreach (var v in values)
            {
                if (v.IsMissing()) continue;
                result.AddRange(SplitTokenComma(v));
            }

            return result;
        }
    }
}