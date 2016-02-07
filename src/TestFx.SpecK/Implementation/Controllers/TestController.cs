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
using TestFx.Extensibility.Controllers;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.SpecK.Implementation.Contexts;
using TestFx.SpecK.Implementation.Utilities;
using TestFx.SpecK.InferredApi;
using TestFx.Utilities;

namespace TestFx.SpecK.Implementation.Controllers
{
  public interface ITestController<in TSubject> : ITestController
  {
    void SetSubjectFactory<T> (string text, Func<Dummy, TSubject> subjectFactory)
        where T : SubjectFactory;
  }

  public interface ITestController<TSubject, out TResult, out TVars, out TSequence> : ITestController<TSubject>
  {
    ITestController<TSubject, TResult, TNewVars, TSequence> SetVariables<TNewVars> (Func<Dummy, TNewVars> variablesProvider);
    ITestController<TSubject, TResult, Dummy, TNewSequence> SetSequences<TNewSequence> (IDictionary<string, TNewSequence> sequences);

    void AddArrangement (string text, Arrangement<TSubject, TResult, TVars, TSequence> arrangement);
    void AddAssertion (string text, Assertion<TSubject, TResult, TVars, TSequence> assertion, bool expectException = false);

    ITestController<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateSequence>
        CreateDelegate<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateSequence> ();
  }

  public class TestController<TSubject, TResult, TVars, TSequence> : TestController, ITestController<TSubject, TResult, TVars, TSequence>
  {
    private readonly SuiteProvider _suiteProvider;
    private readonly TestProvider _provider;
    private readonly TestContext<TSubject, TResult, TVars, TSequence> _context;
    private readonly IControllerFactory _controllerFactory;

    public TestController (
        SuiteProvider suiteProvider,
        TestProvider provider,
        TestContext<TSubject, TResult, TVars, TSequence> context,
        IOperationSorter operationSorter,
        IControllerFactory controllerFactory)
        : base(provider, context, operationSorter)
    {
      _suiteProvider = suiteProvider;
      _provider = provider;
      _context = context;
      _controllerFactory = controllerFactory;
    }

    public void SetSubjectFactory<T> (string text, Func<Dummy, TSubject> subjectFactory)
        where T : SubjectFactory
    {
      RemoveAll<SubjectFactory>();
      AddAction<T>(text, x => _context.Subject = subjectFactory(null));
    }

    public ITestController<TSubject, TResult, Dummy, TNewSequence> SetSequences<TNewSequence> (IDictionary<string, TNewSequence> sequences)
    {
      var mainContext = (MainTestContext<TSubject, TResult, Dummy, TSequence>) (object) _context;
      var configurator = mainContext.Configurator;
      var actionContainer = mainContext.ActionContainer;

      var combinationSuiteProvider = SuiteProvider.Create(_provider.Identity, _provider.Text, _provider.Ignored);

      _suiteProvider.SuiteProviders = _suiteProvider.SuiteProviders.Concat(new[] { combinationSuiteProvider });
      _suiteProvider.TestProviders = _suiteProvider.TestProviders.Except(new[] { _provider });

      var testControllers = sequences.Select(
          x => CreateCombinationTestController(combinationSuiteProvider, configurator, actionContainer, x.Key, x.Value));

      return _controllerFactory.CreateCompositeTestController(testControllers);
    }

    private ITestController<TSubject, TResult, Dummy, TNewSequence> CreateCombinationTestController<TNewSequence> (
        SuiteProvider suiteProvider,
        Action<ITestController> configurator,
        ActionContainer<TSubject, TResult> actionContainer,
        string text,
        TNewSequence sequence)
    {
      var identity = _provider.Identity.CreateChildIdentity(text);
      var testProvider = TestProvider.Create(identity, text, ignored: false);
      suiteProvider.TestProviders = suiteProvider.TestProviders.Concat(new[] { testProvider });
      return _controllerFactory.CreateMainTestController<TSubject, TResult, Dummy, TNewSequence>(
          suiteProvider,
          testProvider,
          configurator,
          actionContainer,
          sequence);
    }

    public ITestController<TSubject, TResult, TNewVars, TSequence> SetVariables<TNewVars> (Func<Dummy, TNewVars> variablesProvider)
    {
      AddAction<Arrange>("<Set_Variables>", x => _context.VarsObject = variablesProvider(null));
      return CreateDelegate<TSubject, TResult, TNewVars, TSequence>();
    }

    public void AddArrangement (string text, Arrangement<TSubject, TResult, TVars, TSequence> arrangement)
    {
      AddAction<Arrange>(text, x => arrangement(_context));
    }

    public void AddAssertion (string text, Assertion<TSubject, TResult, TVars, TSequence> assertion, bool expectException = false)
    {
      _context.ExpectsException |= expectException;
      AddAssertion<Assert>(text, x => assertion(_context));
    }

    public ITestController<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateSequence>
        CreateDelegate<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateSequence> ()
    {
      CheckDelegateCompatibility(typeof (TDelegateSubject), typeof (TSubject));
      CheckDelegateCompatibility(typeof (TDelegateResult), typeof (TResult));

      var delegateContext = _context.CreateDelegate<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateSequence>();
      return _controllerFactory.CreateTestController(_suiteProvider, _provider, delegateContext);
    }

    private void CheckDelegateCompatibility (Type delegateType, Type originalType)
    {
      if (delegateType != typeof (Dummy) && !delegateType.IsAssignableFrom(originalType))
        throw new Exception(string.Format("Type {0} is not assignable from {1}.", delegateType.Name, originalType.Name));
    }
  }
}