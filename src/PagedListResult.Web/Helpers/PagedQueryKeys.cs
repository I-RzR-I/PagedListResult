// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult.Web
//  Author           : RzR
//  Created On       : 2026-04-27 21:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-27 21:43
// ***********************************************************************
//  <copyright file="PagedQueryKeys.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace RzR.ResultMessage.Pagination.AspNetCore.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     The standard query-string keys understood by this parser.
    /// </summary>
    /// =================================================================================================
    public static class PagedQueryKeys
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) <c>?page=N</c> - 1-based page index.
        /// </summary>
        /// =================================================================================================
        public const string Page = "page";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) <c>?pageSize=N</c> -> page size (rejected when above <c>MaxPageSize</c>).
        /// </summary>
        /// =================================================================================================
        public const string PageSize = "pageSize";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) <c>?search=text</c> - free-text search term.
        /// </summary>
        /// =================================================================================================
        public const string Search = "search";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) <c>?searchAll=true|text|fields|all|false|none</c> - search-mode flag.
        /// </summary>
        /// =================================================================================================
        public const string SearchAll = "searchAll";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) <c>?searchFields=a,b</c> - restrict free-text search to listed properties.
        /// </summary>
        /// =================================================================================================
        public const string SearchFields = "searchFields";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) <c>?order=name:desc</c> - single-key sort.
        /// </summary>
        /// =================================================================================================
        public const string Order = "order";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) <c>?filter=prop:cond:val[,val2]</c> - repeatable filter expression.
        /// </summary>
        /// =================================================================================================
        public const string Filter = "filter";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) <c>?fields=a,b,c</c> - projection / partial response.
        /// </summary>
        /// =================================================================================================
        public const string Fields = "fields";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) <c>?predefinedField=Id</c> - predefined record key.
        /// </summary>
        /// =================================================================================================
        public const string PredefinedField = "predefinedField";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) <c>?predefinedRecords=1,2,3</c> - predefined record values.
        /// </summary>
        /// =================================================================================================
        public const string PredefinedRecords = "predefinedRecords";
    }
}