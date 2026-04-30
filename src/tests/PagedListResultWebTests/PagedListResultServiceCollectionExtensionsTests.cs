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
using PagedListResultWebTests.Stubs;
using RzR.ResultMessage.Pagination.AspNetCore.Abstractions;
using RzR.ResultMessage.Pagination.AspNetCore.Builders;
using RzR.ResultMessage.Pagination.AspNetCore.Configuration;
using RzR.ResultMessage.Pagination.AspNetCore.Models;
using RzR.ResultMessage.Pagination.AspNetCore.Registries;
using System;
using System.Linq;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class PagedListResultServiceCollectionExtensionsTests
    {
        private static IMvcBuilder NewMvc() => new ServiceCollection().AddControllers();

        [TestMethod]
        public void AddPagedListResultWeb_RegistersOptions_WithDefaults()
        {
            var services = new ServiceCollection();

            services.AddPagedListResultWeb();

            var sp = services.BuildServiceProvider();
            var opts = sp.GetRequiredService<IOptions<PagedListResultWebOptions>>().Value;

            Assert.AreEqual(200, opts.MaxPageSize);
            Assert.AreEqual(10, opts.DefaultPageSize);
            Assert.IsTrue(opts.EmitLinkHeader);
            Assert.IsTrue(opts.EmitTotalCountHeader);
            Assert.IsFalse(opts.EmitServerTimingHeader);
            Assert.IsNull(opts.StatusCodeMapper);
            Assert.IsNull(opts.ProblemTypeBaseUri);
            Assert.IsNull(opts.JsonSerializerOptions);
        }

        [TestMethod]
        public void AddPagedListResultWeb_AppliesConfigure_Action()
        {
            var services = new ServiceCollection();

            services.AddPagedListResultWeb(o =>
            {
                o.MaxPageSize = 50;
                o.DefaultPageSize = 25;
                o.EmitServerTimingHeader = true;
                o.ProblemTypeBaseUri = new Uri("https://example.com/errors/");
            });

            var sp = services.BuildServiceProvider();
            var opts = sp.GetRequiredService<IOptions<PagedListResultWebOptions>>().Value;

            Assert.AreEqual(50, opts.MaxPageSize);
            Assert.AreEqual(25, opts.DefaultPageSize);
            Assert.IsTrue(opts.EmitServerTimingHeader);
            Assert.AreEqual(new Uri("https://example.com/errors/"), opts.ProblemTypeBaseUri);
        }

        [TestMethod]
        public void AddPagedListResultWeb_RegistersSingletonRegistry()
        {
            var services = new ServiceCollection();

            services.AddPagedListResultWeb();
            var sp = services.BuildServiceProvider();

            var r1 = sp.GetRequiredService<IPageableMetadataRegistry>();
            var r2 = sp.GetRequiredService<IPageableMetadataRegistry>();

            Assert.IsNotNull(r1);
            Assert.AreSame(r1, r2);
        }

        [TestMethod]
        public void AddPagedListResultWeb_RegistersConvention_OnceEvenWhenCalledTwice()
        {
            var services = new ServiceCollection();
            services.AddControllers(); // Provides MvcOptions registration baseline.

            services.AddPagedListResultWeb();
            services.AddPagedListResultWeb();

            var sp = services.BuildServiceProvider();
            var mvcOptions = sp.GetRequiredService<IOptions<MvcOptions>>().Value;

            Assert.AreEqual(
                1,
                mvcOptions.Conventions.OfType<PagedResultApplicationModelConvention>().Count(),
                "Convention must be registered exactly once even when AddPagedListResultWeb is called multiple times.");
        }

        [TestMethod]
        public void AddPagedListResultWeb_BuilderWritesThrough_ToResolvedRegistry()
        {
            var services = new ServiceCollection();

            services
                .AddPagedListResultWeb()
                .ConfigurePageable<SampleItem>(b => b
                    .AllowSort("name", "id")
                    .AllowFilter("id")
                    .AllowSearch("name"));

            var sp = services.BuildServiceProvider();
            var registry = sp.GetRequiredService<IPageableMetadataRegistry>();
            var meta = registry.Get(typeof(SampleItem));

            Assert.IsNotNull(meta, "Registry must contain the configured metadata.");
            Assert.IsTrue(meta.IsSortAllowed("name"));
            Assert.IsTrue(meta.IsSortAllowed("Name"), "Lookup must be case-insensitive.");
            Assert.IsTrue(meta.IsSortAllowed("id"));
            Assert.IsFalse(meta.IsSortAllowed("password"));

            Assert.IsTrue(meta.IsFilterAllowed("id"));
            Assert.IsFalse(meta.IsFilterAllowed("name"));

            Assert.IsTrue(meta.IsSearchAllowed("name"));
            Assert.IsFalse(meta.IsSearchAllowed("id"));
        }

        [TestMethod]
        public void ConfigurePageable_LastCall_Wins()
        {
            var services = new ServiceCollection();
            var builder = services.AddPagedListResultWeb();

            builder.ConfigurePageable<SampleItem>(b => b.AllowSort("name"));
            builder.ConfigurePageable<SampleItem>(b => b.AllowSort("id"));

            var registry = services.BuildServiceProvider().GetRequiredService<IPageableMetadataRegistry>();
            var meta = registry.Get(typeof(SampleItem));

            Assert.IsNotNull(meta);
            Assert.IsFalse(meta.IsSortAllowed("name"), "First registration must be replaced.");
            Assert.IsTrue(meta.IsSortAllowed("id"));
        }

        [TestMethod]
        public void AddPagedListResultWeb_ReturnsBuilder_ExposingServices()
        {
            var services = new ServiceCollection();

            var builder = services.AddPagedListResultWeb();

            Assert.IsNotNull(builder);
            Assert.AreSame(services, builder.Services);
        }

        [TestMethod]
        public void AddPagedListResultWeb_NullServices_Throws()
            => Assert.ThrowsException<ArgumentNullException>(
                () => PagedListResultServiceCollectionExtensions.AddPagedListResultWeb(null));

        [TestMethod]
        public void ConfigurePageable_NullAction_Throws()
        {
            var builder = new ServiceCollection().AddPagedListResultWeb();
            Assert.ThrowsException<ArgumentNullException>(
                () => builder.ConfigurePageable<SampleItem>(null));
        }
    }

    [TestClass]
    public class PageableMetadataRegistryTests
    {
        [TestMethod]
        public void Register_And_Get_RoundTrip()
        {
            IPageableMetadataRegistry registry = new PageableMetadataRegistry();
            var builder = new PageableMetadataBuilder<SampleItem>().AllowSort("a");

            registry.Register(typeof(SampleItem), builder.Build());
            var meta = registry.Get(typeof(SampleItem));

            Assert.IsNotNull(meta);
            Assert.AreSame(typeof(SampleItem), meta.Type);
            Assert.IsTrue(meta.IsSortAllowed("a"));
        }

        [TestMethod]
        public void Get_UnknownType_ReturnsNull()
        {
            IPageableMetadataRegistry registry = new PageableMetadataRegistry();
            Assert.IsNull(registry.Get(typeof(SampleItem)));
        }

        [TestMethod]
        public void All_ReturnsAllRegisteredEntries()
        {
            IPageableMetadataRegistry registry = new PageableMetadataRegistry();
            registry.Register(typeof(SampleItem), new PageableMetadataBuilder<SampleItem>().AllowSort("a").Build());
            registry.Register(typeof(string), new PageableMetadataBuilder<string>().AllowSort("Length").Build());

            var all = registry.All();

            Assert.AreEqual(2, all.Count);
        }

        [TestMethod]
        public void Register_NullType_Throws()
        {
            IPageableMetadataRegistry registry = new PageableMetadataRegistry();
            Assert.ThrowsException<ArgumentNullException>(
                () => registry.Register(null, new PageableMetadataBuilder<SampleItem>().Build()));
        }

        [TestMethod]
        public void Register_NullMetadata_Throws()
        {
            IPageableMetadataRegistry registry = new PageableMetadataRegistry();
            Assert.ThrowsException<ArgumentNullException>(
                () => registry.Register(typeof(SampleItem), null));
        }
    }

    [TestClass]
    public class PageableMetadataBuilderTests
    {
        [TestMethod]
        public void Build_DefaultsToEmptyAllowLists()
        {
            var meta = new PageableMetadataBuilder<SampleItem>().Build();

            Assert.AreEqual(0, meta.AllowedSort.Count);
            Assert.AreEqual(0, meta.AllowedFilter.Count);
            Assert.AreEqual(0, meta.AllowedSearch.Count);
        }

        [TestMethod]
        public void AllowSort_TrimsAndIgnoresEmptyOrWhitespaceEntries()
        {
            var meta = new PageableMetadataBuilder<SampleItem>()
                .AllowSort("  name  ", "", "   ", null, "id")
                .Build();

            Assert.AreEqual(2, meta.AllowedSort.Count);
            Assert.IsTrue(meta.IsSortAllowed("name"));
            Assert.IsTrue(meta.IsSortAllowed("id"));
        }

        [TestMethod]
        public void AllowFilter_DeduplicatesCaseInsensitively()
        {
            var meta = new PageableMetadataBuilder<SampleItem>()
                .AllowFilter("Id", "id", "ID")
                .Build();

            Assert.AreEqual(1, meta.AllowedFilter.Count);
        }
    }
}
