// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResultWebTests
//  Author           : RzR
//  Created On       : 2026-04-26 20:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-26 20:49
// ***********************************************************************
//  <copyright file="PagedResultApplicationModelConventionTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResultWebTests.Stubs;
using RzR.ResultMessage.Pagination.DataModels.Models.Result;
using RzR.ResultMessage.Pagination.Web.Configuration;
using System;
using System.Linq;
using System.Reflection;

#endregion

namespace PagedListResultWebTests
{
    [TestClass]
    public class PagedResultApplicationModelConventionTests
    {
        private static (ApplicationModel Model, ActionModel Action) BuildModel<TController>(string actionName)
            where TController : ControllerBase
        {
            var controllerType = typeof(TController).GetTypeInfo();
            var controllerAttributes = controllerType.GetCustomAttributes(true);
            var controllerModel = new ControllerModel(controllerType, controllerAttributes);
            controllerModel.ControllerName = controllerType.Name.Replace("Controller", string.Empty);

            var method = controllerType.GetMethod(actionName)
                         ?? throw new InvalidOperationException($"Action '{actionName}' not found.");
            var actionAttributes = method.GetCustomAttributes(true);
            var actionModel = new ActionModel(method, actionAttributes) { Controller = controllerModel, ActionName = actionName };
            controllerModel.Actions.Add(actionModel);

            var application = new ApplicationModel();
            application.Controllers.Add(controllerModel);
            controllerModel.Application = application;

            return (application, actionModel);
        }

        private static PagedResultApplicationModelConvention NewConvention() => new PagedResultApplicationModelConvention();

        [TestMethod]
        [DataRow(nameof(SampleController.ReturnsConcretePagedResult))]
        [DataRow(nameof(SampleController.ReturnsInterfacePagedResult))]
        [DataRow(nameof(SampleController.ReturnsTaskPagedResult))]
        [DataRow(nameof(SampleController.ReturnsTaskInterfacePagedResult))]
        [DataRow(nameof(SampleController.ReturnsValueTaskPagedResult))]
        [DataRow(nameof(SampleController.ReturnsActionResultPagedResult))]
        [DataRow(nameof(SampleController.ReturnsTaskActionResultPagedResult))]
        public void Apply_AddsResponseTypes_ForPagedResultActions(string actionName)
        {
            var (model, action) = BuildModel<SampleController>(actionName);

            NewConvention().Apply(model);

            var responseAttrs = action.Filters.OfType<ProducesResponseTypeAttribute>().ToList();

            Assert.IsTrue(responseAttrs.Any(a => a.StatusCode == 200), "Missing 200 response.");
            Assert.IsTrue(responseAttrs.Any(a => a.StatusCode == 400), "Missing 400 response.");

            var ok = responseAttrs.First(a => a.StatusCode == 200);
            Assert.AreEqual(typeof(PagedResult<SampleItem>), ok.Type);

            var bad = responseAttrs.First(a => a.StatusCode == 400);
            Assert.AreEqual(typeof(ProblemDetails), bad.Type);
        }

        [TestMethod]
        public void Apply_AddsProducesJson_ForPagedResultAction()
        {
            var (model, action) = BuildModel<SampleController>(nameof(SampleController.ReturnsConcretePagedResult));

            NewConvention().Apply(model);

            var produces = action.Filters.OfType<ProducesAttribute>().FirstOrDefault();
            Assert.IsNotNull(produces, "Expected [Produces] to be added.");
            CollectionAssert.Contains(produces.ContentTypes.ToArray(), "application/json");
        }

        [TestMethod]
        [DataRow(nameof(SampleController.ReturnsIActionResult))]
        [DataRow(nameof(SampleController.ReturnsString))]
        [DataRow(nameof(SampleController.ReturnsVoid))]
        public void Apply_DoesNothing_WhenActionIsNotPagedResult(string actionName)
        {
            var (model, action) = BuildModel<SampleController>(actionName);

            NewConvention().Apply(model);

            Assert.IsFalse(action.Filters.OfType<ProducesResponseTypeAttribute>().Any(),
                "Convention must not annotate non-paged actions.");
            Assert.IsFalse(action.Filters.OfType<ProducesAttribute>().Any(),
                "Convention must not add [Produces] on non-paged actions.");
        }

        [TestMethod]
        public void Apply_DoesNotDuplicate_WhenStatusCodeAlreadyDeclared()
        {
            var (model, action) = BuildModel<SampleController>(nameof(SampleController.AlreadyAnnotatedPagedResult));

            NewConvention().Apply(model);

            var added200 = action.Filters.OfType<ProducesResponseTypeAttribute>().Count(a => a.StatusCode == 200);
            var added400 = action.Filters.OfType<ProducesResponseTypeAttribute>().Count(a => a.StatusCode == 400);

            // Action attributes already declare 200 + 400; convention must add nothing for those codes.
            Assert.AreEqual(0, added200, "200 must not be duplicated by convention.");
            Assert.AreEqual(0, added400, "400 must not be duplicated by convention.");
        }

        [TestMethod]
        public void Apply_SkipsProduces_WhenActionAlreadyDeclaresIt()
        {
            var (model, action) = BuildModel<SampleController>(nameof(SampleController.ProducesXmlPagedResult));

            NewConvention().Apply(model);

            // Convention must not add a second [Produces].
            var producesFromFilters = action.Filters.OfType<ProducesAttribute>().Count();
            Assert.AreEqual(0, producesFromFilters,
                "[Produces] must not be added when action already declares one (attribute is on the method).");
        }

        [TestMethod]
        public void Apply_SkipsProduces_WhenControllerAlreadyDeclaresIt()
        {
            var (model, action) = BuildModel<SampleControllerWithProducesAtControllerLevel>(
                nameof(SampleControllerWithProducesAtControllerLevel.ReturnsPagedResult));

            NewConvention().Apply(model);

            var producesFromFilters = action.Filters.OfType<ProducesAttribute>().Count();
            Assert.AreEqual(0, producesFromFilters,
                "[Produces] must not be added when controller already declares one.");
        }

        [TestMethod]
        public void Apply_DoesNotThrow_WhenApplicationIsNull()
            => NewConvention().Apply(null);

        [TestMethod]
        public void Apply_DoesNotThrow_WhenApplicationHasNoControllers()
        {
            var application = new ApplicationModel();
            NewConvention().Apply(application);
        }
    }
}