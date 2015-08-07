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
using TestFx.Evaluation.Intents;
using TestFx.Extensibility;
using TestFx.Extensibility.Providers;
using TestFx.Utilities;
using TestFx.Utilities.Collections;
using TestFx.Utilities.Reflection;

namespace TestFx.Evaluation.Loading
{
  public interface IAssemblyLoader
  {
    ISuiteProvider Load (IIntent assemblyIntent);
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

    public ISuiteProvider Load (IIntent assemblyIntent)
    {
      var assembly = Assembly.LoadFrom(assemblyIntent.Identity.Absolute);
      var provider = SuiteProvider.Create(assemblyIntent.Identity, assembly.GetName().Name, ignored: false);
      var controller = _suiteControllerFactory.Create(provider);

      var explorationData = _assemblyExplorer.Explore(assembly);

      var lazyBootstraps = explorationData.BootstrapTypes.Select(x => new TypedLazy<ILazyBootstrap>(x)).ToList();
      var suiteTypes = Filter(assemblyIntent, explorationData.SuiteTypes);

      provider.SuiteProviders = suiteTypes.Select(x => Load(x, explorationData.TypeLoaders, lazyBootstraps, provider.Identity));
      lazyBootstraps
          .Where(x => x.IsValueCreated)
          .Select(x => x.Value)
          .ForEach(
              x =>
                  controller.AddSetupCleanup<SetupCommon, CleanupCommon>(
                      x.GetType().Name + ".Setup",
                      x.Setup,
                      x.GetType().Name + ".Cleanup",
                      x.Cleanup));

      return provider;
    }

    private IEnumerable<Type> Filter (IIntent assemblyIntent, IEnumerable<Type> suiteTypes)
    {
      if (!assemblyIntent.Intents.Any())
        return suiteTypes;
      else
        return suiteTypes.Where(x => assemblyIntent.Intents.Any(y => y.Identity.Relative == x.FullName));
    }

    private ISuiteProvider Load (
        Type suiteType,
        IDictionary<Type, ITypeLoader> loaderDictionary,
        ICollection<TypedLazy<ILazyBootstrap>> assemblySetups,
        IIdentity assemblyIdentity)
    {
      var suiteTypeLoader = loaderDictionary.Single(x => x.Key.IsAssignableFrom(suiteType)).Value;
      return suiteTypeLoader.Load(suiteType, assemblySetups, assemblyIdentity);
    }
  }
}