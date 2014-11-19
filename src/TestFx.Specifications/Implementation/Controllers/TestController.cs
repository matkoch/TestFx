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
using TestFx.Extensibility.Controllers;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.Specifications.Implementation.Contexts;
using TestFx.Specifications.InferredApi;
using JetBrains.Annotations;

namespace TestFx.Specifications.Implementation.Controllers
{
  public interface ITestController<TSubject> : ITestController
  {
    void SetSubjectFactory<T> (string text, Func<Dummy, TSubject> subjectFactory)
        where T : SubjectFactory;
  }

  public interface ITestController<TSubject, out TResult, out TVars> : ITestController<TSubject>
  {
    ITestController<TSubject, TResult, TNewVars> SetVariables<TNewVars> (Func<Dummy, TNewVars> variablesProvider); 

    void AddArrangement (string text, Arrangement<TSubject, TResult, TVars> arrangement);
    void AddAssertion (string text, Assertion<TSubject, TResult, TVars> assertion, bool expectException = false);

    ITestController<TDelegateSubject, TDelegateResult, TDelegateVars> CreateDelegate<TDelegateSubject, TDelegateResult, TDelegateVars> ();
  }

  public class TestController<TSubject, TResult, TVars> : TestController, ITestController<TSubject, TResult, TVars>
  {
    private readonly TestProvider _provider;
    private readonly TestContext<TSubject, TResult, TVars> _context;
    private readonly IControllerFactory _controllerFactory;

    public TestController (
        TestProvider provider,
        TestContext<TSubject, TResult, TVars> context,
        IOperationSorter operationSorter,
        IControllerFactory controllerFactory)
        : base(provider, context, operationSorter)
    {
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

    public virtual ITestController<TSubject, TResult, TNewVars> SetVariables<TNewVars> (Func<Dummy, TNewVars> variablesProvider)
    {
      throw new NotSupportedException();
    }

    public void AddArrangement (string text, Arrangement<TSubject, TResult, TVars> arrangement)
    {
      AddAction<Arrange>(text, x => arrangement(_context));
    }

    public void AddAssertion (string text, Assertion<TSubject, TResult, TVars> assertion, bool expectException = false)
    {
      _context.ExpectsException |= expectException;
      AddAssertion<Assert>(text, x => assertion(_context));
    }

    public ITestController<TDelegateSubject, TDelegateResult, TDelegateVars> CreateDelegate<TDelegateSubject, TDelegateResult, TDelegateVars> ()
    {
      CheckDelegateCompatibility(typeof (TDelegateSubject), typeof (TSubject));
      CheckDelegateCompatibility(typeof (TDelegateResult), typeof (TResult));

      return _controllerFactory.CreateDelegateTestController<TDelegateSubject, TDelegateResult, TDelegateVars, TSubject, TResult, TVars>(
          _provider,
          _context);
    }

    private void CheckDelegateCompatibility (Type delegateType, Type originalType)
    {
      if (delegateType != typeof (Dummy) && !delegateType.IsAssignableFrom(originalType))
        throw new Exception(string.Format("Type {0} is not assignable from {1}.", delegateType.Name, originalType.Name));
    }
  }
}