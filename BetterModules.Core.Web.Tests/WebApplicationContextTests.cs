﻿using System.Linq;
using System.Web.Mvc;
using Autofac;
using BetterModules.Core.Configuration;
using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Dependencies;
using BetterModules.Core.Environment.Assemblies;
using BetterModules.Core.Environment.FileSystem;
using BetterModules.Core.Modules.Registration;
using BetterModules.Core.Security;
using BetterModules.Core.Web.Configuration;
using BetterModules.Core.Web.Dependencies;
using BetterModules.Core.Web.Environment.Assemblies;
using BetterModules.Core.Web.Environment.Host;
using BetterModules.Core.Web.Modules.Registration;
using BetterModules.Core.Web.Mvc;
using BetterModules.Core.Web.Mvc.Commands;
using BetterModules.Core.Web.Mvc.Extensions;
using BetterModules.Core.Web.Mvc.Routes;
using BetterModules.Core.Web.Security;
using BetterModules.Core.Web.Services.Caching;
using BetterModules.Core.Web.Web;
using BetterModules.Core.Web.Web.EmbeddedResources;
using BetterModules.Sample.Web.Module;
using NUnit.Framework;
using RazorGenerator.Mvc;

namespace BetterModules.Core.Web.Tests
{
    [TestFixture]
    public class WebApplicationContextTests : TestBase
    {
        [Test]
        public void Dependecies_Should_Be_Initialized_Correctly()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                // Core services
                Assert.IsNotNull(container.Resolve<IConfiguration>());
                Assert.IsNotNull(container.Resolve<IModulesRegistration>());
                Assert.IsNotNull(container.Resolve<ISessionFactoryProvider>());
                Assert.IsNotNull(container.Resolve<IAssemblyLoader>());
                Assert.IsNotNull(container.Resolve<IUnitOfWorkFactory>());
                Assert.IsNotNull(container.Resolve<IMappingResolver>());
                Assert.IsNotNull(container.Resolve<IWorkingDirectory>());
                Assert.IsNotNull(container.Resolve<IFetchingProvider>());
                Assert.IsNotNull(container.Resolve<IUnitOfWork>());
                Assert.IsNotNull(container.Resolve<IRepository>());
                Assert.IsNotNull(container.Resolve<IVersionChecker>());
                Assert.IsNotNull(container.Resolve<IMigrationRunner>());
                Assert.IsNotNull(container.Resolve<IPrincipalProvider>());
                Assert.IsNotNull(container.Resolve<IAssemblyManager>());

                // Web services
                Assert.IsNotNull(container.Resolve<IWebConfiguration>());
                Assert.IsNotNull(container.Resolve<IWebModulesRegistration>());
                Assert.IsNotNull(container.Resolve<DefaultWebControllerFactory>());
                Assert.IsNotNull(container.Resolve<IEmbeddedResourcesProvider>());
                Assert.IsNotNull(container.Resolve<IHttpContextAccessor>());
                Assert.IsNotNull(container.Resolve<IControllerExtensions>());
                Assert.IsNotNull(container.Resolve<ICommandResolver>());
                Assert.IsNotNull(container.Resolve<IRouteTable>());
                Assert.IsNotNull(container.Resolve<PerWebRequestContainerProvider>());
                Assert.IsNotNull(container.Resolve<ICacheService>());
                Assert.IsNotNull(container.Resolve<IWebApplicationHost>());

                // Core Overrided instances
                Assert.AreEqual(container.Resolve<IConfiguration>().GetType(), typeof(DefaultWebConfigurationSection));
                Assert.AreEqual(container.Resolve<IModulesRegistration>().GetType(), typeof(DefaultWebModulesRegistration));
                Assert.AreEqual(container.Resolve<IPrincipalProvider>().GetType(), typeof(DefaultWebPrincipalProvider));
                Assert.AreEqual(container.Resolve<IAssemblyManager>().GetType(), typeof(DefaultWebAssemblyManager));
            }
        }

        [Test]
        public void Correct_Modules_Should_Be_Loaded()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var modulesRegistration = container.Resolve<IModulesRegistration>();
                var modules = modulesRegistration.GetModules();

                Assert.IsNotNull(modules);
                Assert.AreEqual(modules.Count(), 1);
                Assert.AreEqual(modules.First().ModuleDescriptor.GetType(), typeof(SampleWebModuleDescriptor));
            }
        }

        [Test]
        public void Correct_Controller_Factory_Should_Be_Registered()
        {
            Assert.IsTrue(ControllerBuilder.Current.GetControllerFactory() is DefaultWebControllerFactory); 
        }
        
        [Test]
        public void Precompiled_Views_Engine_Should_Be_Registered()
        {
            Assert.IsTrue(ViewEngines.Engines.Any(e => e is CompositePrecompiledMvcEngine));
        }

        [Test]
        public void Should_Retrieve_Registered_Host_Successfully()
        {
            var host = WebApplicationContext.RegisterHost();
            
            Assert.IsNotNull(host);
            Assert.IsTrue(host is DefaultWebApplicationHost);
        }
    }
}
