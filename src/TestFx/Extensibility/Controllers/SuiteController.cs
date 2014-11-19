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
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Extensibility.Controllers
{
  public interface ISuiteController
  {
    void AddSetupCleanup<TSetup, TCleanup> (string setupText, Action setup, string cleanupText, Action cleanup)
        where TSetup : IActionDescriptor
        where TCleanup : ICleanupDescriptor;
  }

  public class SuiteController : ISuiteController
  {
    private readonly SuiteProvider _provider;
    private readonly IOperationSorter _operationSorter;

    public SuiteController (SuiteProvider provider, IOperationSorter operationSorter)
    {
      _provider = provider;
      _operationSorter = operationSorter;
    }

    public void AddSetupCleanup<TSetup, TCleanup> (string setupText, Action setup, string cleanupText, Action cleanup)
        where TSetup : IActionDescriptor
        where TCleanup : ICleanupDescriptor
    {
      // TODO: shared code with TestController
      IOperationProvider cleanupProvider = null;
      if (cleanup != null)
        cleanupProvider = OperationProvider.Create<TCleanup>(OperationType.Action, cleanupText, cleanup);
      var setupProvider = OperationProvider.Create<TSetup>(OperationType.Action, setupText, setup, cleanupProvider);
      var unsortedOperationProviders = cleanupProvider.Concat(_provider.ContextProviders).Concat(setupProvider).WhereNotNull();
      _provider.ContextProviders = _operationSorter.Sort(unsortedOperationProviders);
    }

    protected SuiteProvider CreateSuiteProvider (string relativeId, string text, bool ignore)
    {
      var identity = _provider.Identity.CreateChildIdentity(relativeId);
      var provider = SuiteProvider.Create(identity, text, ignore);
      _provider.SuiteProviders = _provider.SuiteProviders.Concat(provider);
      return provider;
    }

    protected TestProvider CreateTestProvider (string relativeId, string text, bool ignore)
    {
      var identity = _provider.Identity.CreateChildIdentity(relativeId);
      var provider = TestProvider.Create(identity, text, ignore);
      _provider.TestProviders = _provider.TestProviders.Concat(provider);
      return provider;
    }
  }
}