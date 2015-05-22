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
using TestFx.Extensibility.Controllers;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.Specifications.Implementation.Contexts;
using TestFx.Specifications.InferredApi;

namespace TestFx.Specifications.Implementation.Controllers
{
  public interface ITestController<TSubject> : ITestController
  {
    void SetSubjectFactory<T> (string text, Func<Dummy, TSubject> subjectFactory)
        where T : SubjectFactory;
  }

  public interface ITestController<TSubject, out TResult, out TVars, out TCombi> : ITestController<TSubject>
  {
    ITestController<TSubject, TResult, TNewVars, TCombi> SetVariables<TNewVars> (Func<Dummy, TNewVars> variablesProvider);
    ITestController<TSubject, TResult, TVars, TNewCombi> SetCombinations<TNewCombi> (IDictionary<string, TNewCombi> combinations);

    void AddArrangement (string text, Arrangement<TSubject, TResult, TVars, TCombi> arrangement);
    void AddAssertion (string text, Assertion<TSubject, TResult, TVars, TCombi> assertion, bool expectException = false);

    ITestController<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombo>
        CreateDelegate<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombo> ();
  }

  public class TestController<TSubject, TResult, TVars, TCombi> : TestController, ITestController<TSubject, TResult, TVars, TCombi>
  {
    private readonly SuiteProvider _suiteProvider;
    private readonly TestProvider _provider;
    private readonly TestContext<TSubject, TResult, TVars, TCombi> _context;
    private readonly IControllerFactory _controllerFactory;

    public TestController (
        SuiteProvider suiteProvider,
        TestProvider provider,
        TestContext<TSubject, TResult, TVars, TCombi> context,
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

    public ITestController<TSubject, TResult, TVars, TNewCombi> SetCombinations<TNewCombi> (IDictionary<string, TNewCombi> combinations)
    {
      // Create CompositeController
      // 
      throw new NotImplementedException();
    }

    public ITestController<TSubject, TResult, TNewVars, TCombi> SetVariables<TNewVars> (Func<Dummy, TNewVars> variablesProvider)
    {
      AddAction<Arrange>("<Set_Variables>", x => _context.VarsObject = variablesProvider(null));
      return CreateDelegate<TSubject, TResult, TNewVars, TCombi>();
    }

    public void AddArrangement (string text, Arrangement<TSubject, TResult, TVars, TCombi> arrangement)
    {
      AddAction<Arrange>(text, x => arrangement(_context));
    }

    public void AddAssertion (string text, Assertion<TSubject, TResult, TVars, TCombi> assertion, bool expectException = false)
    {
      _context.ExpectsException |= expectException;
      AddAssertion<Assert>(text, x => assertion(_context));
    }

    public ITestController<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombi>
        CreateDelegate<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombi> ()
    {
      CheckDelegateCompatibility(typeof (TDelegateSubject), typeof (TSubject));
      CheckDelegateCompatibility(typeof (TDelegateResult), typeof (TResult));

      var delegateContext = _context.CreateDelegate<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombi>();
      return _controllerFactory.CreateTestController(_suiteProvider, _provider, delegateContext);
    }

    private void CheckDelegateCompatibility (Type delegateType, Type originalType)
    {
      if (delegateType != typeof (Dummy) && !delegateType.IsAssignableFrom(originalType))
        throw new Exception(string.Format("Type {0} is not assignable from {1}.", delegateType.Name, originalType.Name));
    }
  }
}