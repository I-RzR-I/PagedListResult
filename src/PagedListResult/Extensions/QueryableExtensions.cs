// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2023-11-07 20:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-09 23:09
// ***********************************************************************
//  <copyright file="QueryableExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#pragma warning disable EF1001

#endregion

namespace PagedListResult.Extensions
{
    internal static class QueryableExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get Db context form IQueryable.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <exception cref="ApplicationException">
        ///     Thrown when an Application error condition occurs.
        /// </exception>
        /// <typeparam name="TSource">Query type.</typeparam>
        /// <param name="query">Current query.</param>
        /// <returns>The database context.</returns>
        ///=================================================================================================
        private static DbContext GetDbContext<TSource>(this IQueryable<TSource> query) where TSource : class
        {
            try
            {
                var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
                var queryCompiler = typeof(EntityQueryProvider).GetField("_queryCompiler", bindingFlags)
                    ?.GetValue(query.Provider);
                if (queryCompiler.IsNotNull())
                {
                    var queryContextFactory = (RelationalQueryContextFactory)queryCompiler!.GetType()
                        .GetField("_queryContextFactory", bindingFlags)?.GetValue(queryCompiler);
                    if (queryContextFactory.IsNotNull())
                    {
                        var dependencies = queryContextFactory!.GetType().GetProperty("Dependencies", bindingFlags)
                            ?.GetValue(queryContextFactory);
                        if (dependencies.IsNull())
                        {
                            try
                            {
                                var fieldOrProperty = queryContextFactory.GetType().GetFieldOrProperty("Dependencies");
                                if (fieldOrProperty.IsNotNull())
                                    dependencies = queryContextFactory.GetType().GetField("Dependencies", bindingFlags)
                                        ?.GetValue(queryContextFactory);
                            }
                            catch
                            {
                                // Error on get GetFieldOrProperty (Name of prop of field, doesn't exist)
                                dependencies = null;
                            }
                        }

                        if (dependencies.IsNull())
                        {
                            dependencies = queryContextFactory.GetType().GetProperty("_dependencies", bindingFlags)
                                ?.GetValue(queryContextFactory);
                            if (dependencies.IsNull())
                            {
                                try
                                {
                                    var fieldOrProperty = queryContextFactory.GetType()
                                        .GetFieldOrProperty("_dependencies");
                                    if (fieldOrProperty.IsNotNull())
                                        dependencies = queryContextFactory.GetType()
                                            .GetField("_dependencies", bindingFlags)
                                            ?.GetValue(queryContextFactory);
                                }
                                catch
                                {
                                    // Error on get GetFieldOrProperty (Name of prop of field, doesn't exist)
                                    dependencies = null;
                                }
                            }
                        }

                        if (dependencies.IsNotNull())
                        {
                            var queryContextDependencies =
                                typeof(DbContext).Assembly.GetType(typeof(QueryContextDependencies).FullName!);
                            var stateManagerProperty = queryContextDependencies
                                ?.GetProperty("StateManager", bindingFlags | BindingFlags.Public)
                                ?.GetValue(dependencies);

                            return ((IStateManager)stateManagerProperty)?.Context;
                        }

                        throw new ApplicationException("Can't get EF dependencies");
                    }

                    throw new ApplicationException("Can't get EF context factory");
                }

                throw new ApplicationException("Can't get EF compiler");
            }
            catch (Exception e)
            {
                throw new ApplicationException("Can't get DB context from IQueryable, check EF version", e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Get PK name list.</summary>
        /// <remarks>RzR, 14-Nov-23.</remarks>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="query">Query.</param>
        /// <returns>The primary keys name list.</returns>
        ///=================================================================================================
        internal static IList<string> GetPrimaryKeysNameList<TSource>(this IQueryable<TSource> query) where TSource : class
        {
            /*
             * ************************************************************************
             * Get primary keys name.
             * For start try to get PK from entity (from DbContext)
             * In case when set model was DTO, we will catch an error,
             * because in DTO no one set PK (it's not an DB entity).
             * Then try to get PK name by logical name of the property ends with 'Id'.
             * For first check if prop in top is default name 'Id', in case of truth,
             * return this value, otherwise find all props that name ends with 'Id'
             * ************************************************************************
             */
            var keysName = new List<string>();
            try
            {
                keysName = query.GetDbContext().FindPrimaryKeysNameList(query).ToList();
            }
            catch
            {
                var type = typeof(TSource);
                var fields = type.GetProperties().ToList();
                if (fields.Any())
                {
                    var first = fields.FirstOrDefault();
                    if (first.IsNotNull() && (first!.Name.Equals("Id") || first.Name.EndsWith("Id")))
                        keysName.Add(first.Name);
                    else
                        foreach (var p in fields)
                            if (p.Name.Equals("Id") || p.Name.EndsWith("Id"))
                                keysName.Add(p.Name);
                }
            }

            return keysName;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;TSource&gt; extension method that gets page record list.
        /// </summary>
        /// <remarks>RzR, 10-Nov-23.</remarks>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="query">Current query.</param>
        /// <param name="skip">The skip record count.</param>
        /// <param name="take">The take record count.</param>
        /// <returns>The page record list.</returns>
        ///=================================================================================================
        internal static IList<TSource> GetPageRecordList<TSource>(this IQueryable<TSource> query, int skip, int take)
        where TSource : class
            => query.Skip(skip).Take(take).ToList();
    }
}