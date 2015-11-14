// Copyright 2015, 2014 Matthias Koch
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
using TestFx.Extensibility;
using TestFx.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.Evaluation.Loading
{
  public interface IAssemblyExplorer
  {
    AssemblyExplorationData Explore (Assembly assembly);
  }

  public class AssemblyExplorer : IAssemblyExplorer
  {
    public AssemblyExplorationData Explore (Assembly assembly)
    {
      var bootstrapTypes = assembly.GetTypes().Where(x => x.IsInstantiatable<ILazyBootstrap>());

      var suiteTypes = assembly.GetTypes().Where(x => x.GetAttribute<SuiteAttributeBase>() != null).ToList();
      var suiteAttributes = suiteTypes.Select(x => x.GetAttribute<SuiteAttributeBase>().AssertNotNull().GetType()).Distinct();
      //var suiteBaseTypes = suiteTypes.Select(x => x.GetImmediateDerivedTypesOf<ISuite>().Single()).Distinct();
      var testExtensions = assembly.GetAttributes<UseTestExtension>()
          .Select(x => x.TestExtensionType.CreateInstance<ITestExtension>())
          .OrderByDescending(x => x.Priority);
      var typeLoaders = suiteAttributes.Select(x => CreateTypeLoader(x, testExtensions)).ToDictionary(x => x.Item1, x => x.Item2);

      return new AssemblyExplorationData(typeLoaders, suiteTypes, bootstrapTypes);
    }

    private Tuple<Type, ITypeLoader> CreateTypeLoader (Type suiteAttribute, IEnumerable<ITestExtension> testExtensions)
    {
      var typeLoaderType = suiteAttribute.GetAttribute<TypeLoaderAttribute>().AssertNotNull().TypeLoaderType;
      var operationOrdering = suiteAttribute.GetAttribute<OperationOrderingAttribute>().AssertNotNull().OperationDescriptors;

      var builder = new ContainerBuilder();
      builder.RegisterModule<UtilitiesModule>();
      builder.RegisterModule(new ExtensibilityModule(typeLoaderType, operationOrdering));
      builder.RegisterInstance(testExtensions).As<IEnumerable<ITestExtension>>();
      var container = builder.Build();

      var suiteBaseType = typeLoaderType.GetClosedTypeOf(typeof(TypeLoader<>)).AssertNotNull().GetGenericArguments().Single();
      var typeLoader = (ITypeLoader) container.Resolve(typeLoaderType);

      return Tuple.Create(suiteBaseType, typeLoader);
    }
  }
}