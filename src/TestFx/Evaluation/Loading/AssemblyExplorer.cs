// Copyright 2014, 2013 Matthias Koch
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
using TestFx.Evaluation.Utilities;
using TestFx.Extensibility;
using JetBrains.Annotations;
using TestFx.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.Evaluation.Loading
{
  public interface IAssemblyExplorer
  {
    IAssemblyExplorationData Explore (Assembly assembly);
  }

  public class AssemblyExplorer : IAssemblyExplorer
  {
    public IAssemblyExplorationData Explore (Assembly assembly)
    {
      var assemblySetups = assembly.CreateInstancesOf<IAssemblySetup>();

      var suiteTypes = assembly.GetTypes().Where(x => x.IsInstantiatable<ISuite>()).ToList();
      var suiteBaseTypes = suiteTypes.Select(x => x.GetDirectDerivedTypesOf<ISuite>().Single()).Distinct();
      var testExtensions = assembly.GetAttributes<UseTestExtension>().Select(CreateTestExtension).ToList();
      var typeLoaders = suiteBaseTypes.Select(x => CreateTypeLoader(x, testExtensions));

      return new AssemblyExplorationData(typeLoaders, suiteTypes, assemblySetups);
    }

    private static ITypeLoader CreateTypeLoader (Type suiteBaseType, IEnumerable<ITestExtension> testExtensions)
    {
      var typeLoaderType = suiteBaseType.GetAttribute<TypeLoaderAttribute>().TypeLoaderType;
      var operationOrdering = suiteBaseType.GetAttribute<OperationOrderingAttribute>().OperationDescriptors;

      var builder = new ContainerBuilder();
      builder.RegisterModule<UtilitiesModule>();
      builder.RegisterModule(new ExtensibilityModule(typeLoaderType, operationOrdering));
      builder.RegisterInstance(testExtensions).As<IEnumerable<ITestExtension>>();
      var container = builder.Build();

      return (ITypeLoader) container.Resolve(typeLoaderType);
    }

    private static ITestExtension CreateTestExtension (UseTestExtension x)
    {
      return x.TestExtensionType.CreateInstance<ITestExtension>();
    }
  }
}