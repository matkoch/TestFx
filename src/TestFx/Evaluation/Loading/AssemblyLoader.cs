﻿// Copyright 2014, 2013 Matthias Koch
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
using TestFx.Extensibility;
using TestFx.Extensibility.Providers;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Evaluation.Loading
{
  public interface IAssemblyLoader
  {
    ISuiteProvider Load (string assemblyLocation);
  }

  public class AssemblyLoader : IAssemblyLoader
  {
    private readonly IAssemblyExplorer _assemblyExplorer;
    private readonly ISuiteControllerFactory _suiteControllerFactory;

    public AssemblyLoader (IAssemblyExplorer assemblyExplorer, ISuiteControllerFactory suiteControllerFactory)
    {
      _assemblyExplorer = assemblyExplorer;
      _suiteControllerFactory = suiteControllerFactory;
    }

    public ISuiteProvider Load (string assemblyLocation)
    {
      var assembly = Assembly.LoadFrom(assemblyLocation);
      var provider = SuiteProvider.Create(new Identity(assemblyLocation), assembly.GetName().Name, false);
      var controller = _suiteControllerFactory.Create(provider);

      var explorationData = _assemblyExplorer.Explore(assembly);

      var loaderDictionary = explorationData.TypeLoaders.ToDictionary(GetApplicableSuiteType, x => x);
      var suiteTypes = explorationData.SuiteTypes.ToList();
      var assemblySetups = explorationData.AssemblySetups.ToList();

      provider.SuiteProviders = suiteTypes.Select(x => Load(x, loaderDictionary, assemblySetups, provider.Identity));
      assemblySetups.ForEach(x => controller.AddSetupCleanup<SetupCommon, CleanupCommon>(x.Setup, x.Cleanup));

      return provider;
    }

    private Type GetApplicableSuiteType (ITypeLoader typeLoader)
    {
      var typeSuiteLoaderType = typeLoader.GetType();
      var closedTypeSuiteLoaderType = typeSuiteLoaderType.GetClosedTypeFor(typeof (ITypeLoader<>));
      return closedTypeSuiteLoaderType.GetGenericArguments().Single();
    }

    private ISuiteProvider Load (
        Type suiteType,
        IDictionary<Type, ITypeLoader> loaderDictionary,
        IEnumerable<IAssemblySetup> assemblySetups,
        IIdentity parentIdentity)
    {
      var suiteTypeLoader = loaderDictionary.Single(x => x.Key.IsAssignableFrom(suiteType)).Value;
      return suiteTypeLoader.Load(suiteType, assemblySetups, parentIdentity);
    }
  }
}