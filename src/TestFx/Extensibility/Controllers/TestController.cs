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
using System.Linq;
using JetBrains.Annotations;
using TestFx.Extensibility.Contexts;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.Utilities;

namespace TestFx.Extensibility.Controllers
{
  public interface ITestController
  {
    void AddSetupCleanup<TSetup, TCleanup> (
        string setupText,
        Action<ITestContext> setup,
        [CanBeNull] string cleanupText,
        [CanBeNull] Action<ITestContext> cleanup)
        where TSetup : IActionDescriptor
        where TCleanup : ICleanupDescriptor;

    void AddAction<T> (string text, Action<ITestContext> action)
        where T : IActionDescriptor;

    void AddAssertion<T> (string text, Action<ITestContext> action)
        where T : IAssertionDescriptor;

    void Wrap<T> (Action<ITestContext, Action> enclosingAction)
        where T : IOperationDescriptor;

    void RemoveAll<T> ()
        where T : IOperationDescriptor;
  }

  public class TestController : ITestController
  {
    private readonly TestProvider _provider;
    private readonly TestContext _context;
    private readonly IOperationSorter _operationSorter;

    protected TestController (TestProvider provider, TestContext context, IOperationSorter operationSorter)
    {
      _provider = provider;
      _context = context;
      _operationSorter = operationSorter;
    }

    public void AddAction<T> (string text, Action<ITestContext> action)
        where T : IActionDescriptor
    {
      Add<T>(OperationType.Action, text, action);
    }

    public void AddAssertion<T> (string text, Action<ITestContext> action)
        where T : IAssertionDescriptor
    {
      Add<T>(OperationType.Assertion, text, action);
    }

    private void Add<T> (OperationType type, string text, Action<ITestContext> action)
        where T : IOperationDescriptor
    {
      var operationProvider = OperationProvider.Create<T>(type, text, InjectContextAndGuardAction(action));
      var unsortedOperationProviders = _provider.OperationProviders.Concat(operationProvider);
      _provider.OperationProviders = _operationSorter.Sort(unsortedOperationProviders);
    }

    public void AddSetupCleanup<TSetup, TCleanup> (
        string setupText,
        Action<ITestContext> setup,
        [CanBeNull] string cleanupText,
        [CanBeNull] Action<ITestContext> cleanup)
        where TSetup : IActionDescriptor
        where TCleanup : ICleanupDescriptor
    {
      // TODO: shared code with SuiteController
      IOperationProvider cleanupProvider = null;
      if (cleanup != null)
        cleanupProvider = OperationProvider.Create<TCleanup>(OperationType.Action, cleanupText, InjectContextAndGuardAction(cleanup));
      var setupProvider = OperationProvider.Create<TSetup>(OperationType.Action, setupText, InjectContextAndGuardAction(setup), cleanupProvider);
      var unsortedOperationProviders = cleanupProvider.Concat(_provider.OperationProviders).Concat(setupProvider).WhereNotNull();
      _provider.OperationProviders = _operationSorter.Sort(unsortedOperationProviders);
    }

    public void Wrap<T> (Action<ITestContext, Action> enclosingAction)
        where T : IOperationDescriptor
    {
      var wrappedOperationProviders = _provider.OperationProviders.Where(x => typeof (T).IsAssignableFrom(x.Descriptor))
          .Select(x => OperationProvider.Create<T>(x.Type, x.Text, () => enclosingAction(_context, x.Action), x.CleanupProvider));
      RemoveAll<T>();
      var unsortedOperationProviders = _provider.OperationProviders.Concat(wrappedOperationProviders.Cast<IOperationProvider>());
      _provider.OperationProviders = _operationSorter.Sort(unsortedOperationProviders);
    }

    public void RemoveAll<T> ()
        where T : IOperationDescriptor
    {
      var unsortedOperationProviders = _provider.OperationProviders.Where(x => !typeof (T).IsAssignableFrom(x.Descriptor));
      _provider.OperationProviders = _operationSorter.Sort(unsortedOperationProviders);
    }

    private Action InjectContextAndGuardAction (Action<ITestContext> action)
    {
      return () =>
      {
        try
        {
          action(_context);
        }
        catch (Exception)
        {
          _context.IsFailing = true;
          throw;
        }
      };
    }
  }
}