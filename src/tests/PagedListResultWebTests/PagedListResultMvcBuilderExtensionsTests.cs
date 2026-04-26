// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultWebTests
//  Author           : RzR
//  Created On       : 2026-04-26 20:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 20:49
// ***********************************************************************
//  <copyright file="PagedListResultMvcBuilderExtensionsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.ResultMessage.Pagination.Web.Configuration;
using System;
using System.Linq;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class PagedListResultMvcBuilderExtensionsTests
    {
        private static IMvcBuilder CreateMvcBuilder()
        {
            var services = new ServiceCollection();

            return services.AddControllers();
        }

        [TestMethod]
        public void AddPagedListResultApiExplorer_RegistersConvention()
        {
            var builder = CreateMvcBuilder();

            builder.AddPagedListResultApiExplorer();

            var sp = builder.Services.BuildServiceProvider();
            var options = sp.GetRequiredService<IOptions<MvcOptions>>().Value;

            Assert.AreEqual(
                1,
                options.Conventions.OfType<PagedResultApplicationModelConvention>().Count(),
                "Convention must be registered exactly once.");
        }

        [TestMethod]
        public void AddPagedListResultApiExplorer_IsIdempotent()
        {
            var builder = CreateMvcBuilder();

            builder.AddPagedListResultApiExplorer()
                .AddPagedListResultApiExplorer()
                .AddPagedListResultApiExplorer();

            var sp = builder.Services.BuildServiceProvider();
            var options = sp.GetRequiredService<IOptions<MvcOptions>>().Value;

            Assert.AreEqual(
                1,
                options.Conventions.OfType<PagedResultApplicationModelConvention>().Count(),
                "Convention must not be registered more than once when called multiple times.");
        }

        [TestMethod]
        public void AddPagedListResultApiExplorer_ReturnsSameBuilder()
        {
            var builder = CreateMvcBuilder();

            var result = builder.AddPagedListResultApiExplorer();

            Assert.AreSame(builder, result);
        }

        [TestMethod]
        public void AddPagedListResultApiExplorer_NullBuilder_Throws()
            => Assert.ThrowsException<ArgumentNullException>(() => PagedListResultMvcBuilderExtensions.AddPagedListResultApiExplorer(null));
    }
}