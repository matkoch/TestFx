// Copyright 2016, 2015, 2014 Matthias Koch
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Resolving;
using TestFx.Extensibility;
using TestFx.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.Evaluation.Loading
{
  public interface IAssemblyExplorer
  {
    IAssemblyExplorationData Explore(Assembly assembly);
  }

  public class AssemblyExplorer : IAssemblyExplorer
  {
    public IAssemblyExplorationData Explore(Assembly assembly)
    {
      var testExtensions = assembly.GetAttributes<UseTestExtensionAttribute>()
          .Select(x => x.TestExtensionType.CreateInstance<ITestExtension>())
          .OrderByDescending(x => x.Priority).ToList();
      var testLoaderTypes = assembly.GetAttributes<UseTestLoaderAttribute>().Select(x => x.TestLoaderType);
      var testLoaderFactories = testLoaderTypes.Select(x => BuildTestLoaderFactory(x, testExtensions)).ToList();

      var potentialSuiteTypes = assembly.GetTypes().Where(x => x.IsInstantiatable<object>()).ToList();

      var assemblySetupTypes = assembly.GetTypes().Where(x => x.IsInstantiatable<IAssemblySetup>()).ToDictionary(
          x => x,
          x => new Lazy<IAssemblySetup>(() => x.CreateInstance<IAssemblySetup>()));

      return new AssemblyExplorationData(testLoaderFactories, potentialSuiteTypes, assemblySetupTypes);
    }

    private TypeLoaderFactory BuildTestLoaderFactory (Type testLoaderType, IEnumerable<ITestExtension> testExtensions)
    {
      var operationOrdering = testLoaderType.GetAttribute<OperationOrderingAttribute>().NotNull().OperationDescriptors;

      var builder = new ContainerBuilder();
      builder.RegisterModule<UtilitiesModule>();
      builder.RegisterModule(new ExtensibilityModule(testLoaderType, operationOrdering)) ;
      builder.RegisterInstance(testExtensions).As<IEnumerable<ITestExtension>>();
      builder.Register<TypeLoaderFactory>(
          ctx =>
          {
            var innerCtx = ctx.Resolve<IComponentContext>();
            return suite => innerCtx.Resolve<ITestLoader>(new NamedParameter("suite", suite));
          });
      var container = builder.Build();

      // TODO: Add ability to resolve suite object
      return container.Resolve<TypeLoaderFactory>();
    }
  }
}