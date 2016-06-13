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
using System.Runtime.Serialization;
using TestFx.Evaluation.Intents;
using TestFx.Extensibility;
using TestFx.Extensibility.Providers;
using TestFx.Utilities.Collections;

namespace TestFx.Evaluation.Loading
{
  public interface IAssemblyLoader
  {
    ISuiteProvider Load (IIntent assemblyIntent);
  }

  internal class AssemblyLoader : IAssemblyLoader
  {
    private readonly IAssemblyExplorer _assemblyExplorer;
    private readonly ISuiteControllerFactory _suiteControllerFactory;

    public AssemblyLoader (IAssemblyExplorer assemblyExplorer, ISuiteControllerFactory suiteControllerFactory)
    {
      _assemblyExplorer = assemblyExplorer;
      _suiteControllerFactory = suiteControllerFactory;
    }

    public ISuiteProvider Load (IIntent assemblyIntent)
    {
      var assembly = Assembly.LoadFrom(assemblyIntent.Identity.Absolute);
      var provider = SuiteProvider.Create(assemblyIntent.Identity, assembly.GetName().Name, ignoreReason: null);
      var controller = _suiteControllerFactory.Create(provider);

      var explorationData = _assemblyExplorer.Explore(assembly);

      var suiteProviders = LoadSuiteProviders(
          assemblyIntent,
          explorationData.TestLoaderFactories.ToList(),
          explorationData.PotentialSuiteTypes.ToList(),
          explorationData.AssemblySetupTypes);

      provider.SuiteProviders = suiteProviders;
      explorationData.AssemblySetupTypes.Values
          .Where(x => x.IsValueCreated)
          .Select(x => x.Value)
          .ForEach(
              x => controller.AddSetupCleanup<SetupCommon, CleanupCommon>(
                  x.GetType().Name + ".Setup",
                  x.Setup,
                  x.GetType().Name + ".Cleanup",
                  x.Cleanup));

      return provider;
    }

    private IEnumerable<ISuiteProvider> LoadSuiteProviders (IIntent assemblyIntent, List<TypeLoaderFactory> testLoaderFactories, List<Type> potentialSuiteTypes, IDictionary<Type, Lazy<IAssemblySetup>> assemblySetupTypes)
    {
      foreach (var potentialSuiteType in potentialSuiteTypes)
      {
        if (assemblyIntent.Intents.Any() && assemblyIntent.Intents.All(y => y.Identity.Relative != potentialSuiteType.FullName))
          continue;

        foreach (var testLoaderFactory in testLoaderFactories)
        {
          var suite = FormatterServices.GetUninitializedObject(potentialSuiteType);
          var testLoader = testLoaderFactory(suite);
          var suiteProvider = testLoader.Load(suite, assemblySetupTypes, assemblyIntent.Identity);
          if (suiteProvider != null)
            yield return suiteProvider;
        }
      }
    }
  }
}