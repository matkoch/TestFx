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
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.Specifications.Implementation.Contexts;
using TestFx.Specifications.Implementation.Utilities;
using TestFx.Specifications.InferredApi;
using TestFx.Utilities.Reflection;

namespace TestFx.Specifications.Implementation.Controllers
{
  public interface IControllerFactory
  {
    IClassSuiteController CreateClassSuiteController (ISuite suite, Type subjectType, SuiteProvider provider);

    ISpecializedSuiteController<TSubject, TResult> CreateSpecializedSuiteController<TSubject, TResult> (
        SuiteProvider provider,
        ActionContainer<TSubject, TResult> actionContainer,
        IClassSuiteController<TSubject> classSuiteController);

    ITestController<TSubject, TResult, TVars, TCombi> CreateMainTestController<TSubject, TResult, TVars, TCombi> (
        SuiteProvider suiteProvider,
        TestProvider provider,
        ActionContainer<TSubject, TResult> actionContainer,
        TVars vars,
        TCombi combi);

    ITestController<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombo>
        CreateDelegateTestController<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombo, TSubject, TResult, TVars, TCombi> (
        SuiteProvider suiteProvider,
        TestProvider provider,
        TestContext<TSubject, TResult, TVars, TCombi> context);
  }

  public class ControllerFactory : IControllerFactory
  {
    private readonly IOperationSorter _operationSorter;
    private readonly IEnumerable<ITestExtension> _testExtensions;

    public ControllerFactory (
        IOperationSorter operationSorter,
        IEnumerable<ITestExtension> testExtensions)
    {
      _operationSorter = operationSorter;
      _testExtensions = testExtensions;
    }

    public IClassSuiteController CreateClassSuiteController (ISuite suite, Type subjectType, SuiteProvider provider)
    {
      var suiteControllerType = typeof (ClassSuiteController<>).MakeGenericType(subjectType);
      var controller = suiteControllerType.CreateInstance<IClassSuiteController>(provider, suite, _testExtensions, this, _operationSorter);
      provider.Controller = controller;
      return controller;
    }

    public ISpecializedSuiteController<TSubject, TResult> CreateSpecializedSuiteController<TSubject, TResult> (
        SuiteProvider provider,
        ActionContainer<TSubject, TResult> actionContainer,
        IClassSuiteController<TSubject> classSuiteController)
    {
      var controller = new SpecializedSuiteController<TSubject, TResult>(provider, actionContainer, classSuiteController, this, _operationSorter);
      provider.Controller = controller;
      return controller;
    }

    public ITestController<TSubject, TResult, TVars, TCombi> CreateMainTestController<TSubject, TResult, TVars, TCombi> (
        SuiteProvider suiteProvider,
        TestProvider provider,
        ActionContainer<TSubject, TResult> actionContainer,
        TVars vars,
        TCombi combi)
    {
      var context = new MainTestContext<TSubject, TResult, TVars, TCombi>();
      var controller = new MainTestController<TSubject, TResult, TVars, TCombi>(
          suiteProvider,
          provider,
          context,
          actionContainer,
          _operationSorter,
          this);
      return controller;
    }

    public ITestController<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombo>
        CreateDelegateTestController<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombo, TSubject, TResult, TVars, TCombi> (
        SuiteProvider suiteProvider,
        TestProvider provider,
        TestContext<TSubject, TResult, TVars, TCombi> context)
    {
      var delegateContext =
          new DelegateTestContext<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombo, TSubject, TResult, TVars, TCombi>(context);
      return new TestController<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombo>(
          suiteProvider,
          provider,
          delegateContext,
          _operationSorter,
          this);
    }
  }
}