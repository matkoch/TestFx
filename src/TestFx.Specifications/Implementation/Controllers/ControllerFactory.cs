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
using System.Diagnostics;
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.Specifications.Implementation.Contexts;
using TestFx.Specifications.Implementation.Utilities;
using TestFx.Specifications.InferredApi;
using TestFx.Utilities;
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

    ITestController<TSubject, TResult, TVars, TCombi> CreateTestController<TSubject, TResult, TVars, TCombi> (
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
      return suiteControllerType.CreateInstance<IClassSuiteController>(provider, suite, _testExtensions, this, _operationSorter);
    }

    public ISpecializedSuiteController<TSubject, TResult> CreateSpecializedSuiteController<TSubject, TResult> (
        SuiteProvider provider,
        ActionContainer<TSubject, TResult> actionContainer,
        IClassSuiteController<TSubject> classSuiteController)
    {
      return new SpecializedSuiteController<TSubject, TResult>(provider, actionContainer, classSuiteController, this, _operationSorter);
    }

    public ITestController<TSubject, TResult, TVars, TCombi> CreateMainTestController<TSubject, TResult, TVars, TCombi> (
        SuiteProvider suiteProvider,
        TestProvider provider,
        ActionContainer<TSubject, TResult> actionContainer,
        TVars vars,
        TCombi combi)
    {
      var context = new MainTestContext<TSubject, TResult, TVars, TCombi>(actionContainer);
      var controller = CreateTestController(suiteProvider, provider, context);

      var wrappedAction = actionContainer.VoidAction != null
          ? GuardAction(context, actionContainer.VoidAction)
          : GuardAction(context, x => context.Result = actionContainer.ResultAction.AssertNotNull()(x));
      controller.AddAction<Act>(actionContainer.Text, x => wrappedAction());

      return controller;
    }

    public ITestController<TSubject, TResult, TVars, TCombi> CreateTestController<TSubject, TResult, TVars, TCombi> (
        SuiteProvider suiteProvider,
        TestProvider provider,
        TestContext<TSubject, TResult, TVars, TCombi> context)
    {
      return new TestController<TSubject, TResult, TVars, TCombi>(suiteProvider, provider, context, _operationSorter, this);
    }

    private Action GuardAction<TSubject, TResult, TVars, TCombi> (MainTestContext<TSubject, TResult, TVars, TCombi> context, Action<TSubject> action)
    {
      return () =>
      {
        try
        {
          var stopwatch = Stopwatch.StartNew();
          action(context.Subject);
          context.Duration = stopwatch.Elapsed;
        }
        catch (Exception exception)
        {
          if (!context.ExpectsException)
            throw;
          context.Exception = exception;
        }
        finally
        {
          context.ActionExecuted = true;
        }
      };
    }
  }
}