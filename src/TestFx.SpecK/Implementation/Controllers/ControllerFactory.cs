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
using System.Linq;
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.SpecK.Implementation.Contexts;
using TestFx.SpecK.Implementation.Utilities;
using TestFx.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.SpecK.Implementation.Controllers
{
  public interface IControllerFactory
  {
    ISuiteController CreateClassSuiteController(object suite, Type subjectType, SuiteProvider provider);

    ISpecializedSuiteController<TSubject, TResult> CreateSpecializedSuiteController<TSubject, TResult> (
        SuiteProvider provider,
        ActionContainer<TSubject, TResult> actionContainer,
        Action<ITestController> testControllerConfigurator);

    ITestController<TSubject, TResult, TVars, TSequence> CreateMainTestController<TSubject, TResult, TVars, TSequence> (
        SuiteProvider suiteProvider,
        TestProvider provider,
        Action<ITestController> configurator,
        ActionContainer<TSubject, TResult> actionContainer,
        TSequence sequence);

    ITestController<TSubject, TResult, TVars, TSequence> CreateCompositeTestController<TSubject, TResult, TVars, TSequence> (
        IEnumerable<ITestController<TSubject, TResult, TVars, TSequence>> controllers);

    ITestController<TSubject, TResult, TVars, TSequence> CreateTestController<TSubject, TResult, TVars, TSequence> (
        SuiteProvider suiteProvider,
        TestProvider provider,
        TestContext<TSubject, TResult, TVars, TSequence> context);
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

    public ISuiteController CreateClassSuiteController (object suite, Type subjectType, SuiteProvider provider)
    {
      var suiteControllerType = typeof (ClassSuiteController<>).MakeGenericType(subjectType);
      return suiteControllerType.CreateInstance<ISuiteController>(provider, suite, _testExtensions, this, _operationSorter);
    }

    public ISpecializedSuiteController<TSubject, TResult> CreateSpecializedSuiteController<TSubject, TResult> (
        SuiteProvider provider,
        ActionContainer<TSubject, TResult> actionContainer,
        Action<ITestController> testControllerConfigurator)
    {
      return new SpecializedSuiteController<TSubject, TResult>(provider, actionContainer, testControllerConfigurator, this, _operationSorter);
    }

    public ITestController<TSubject, TResult, TVars, TSequence> CreateMainTestController<TSubject, TResult, TVars, TSequence> (
        SuiteProvider suiteProvider,
        TestProvider provider,
        Action<ITestController> configurator,
        ActionContainer<TSubject, TResult> actionContainer,
        TSequence sequence)
    {
      var context = new MainTestContext<TSubject, TResult, TVars, TSequence>(actionContainer, configurator) { Sequence = sequence };
      var controller = CreateTestController(suiteProvider, provider, context);

      var wrappedAction = actionContainer.VoidAction != null
          ? GuardAction(context, actionContainer.VoidAction)
          : GuardAction(context, x => context.Result = actionContainer.ResultAction.NotNull()(x));
      controller.AddAction<Act>(actionContainer.Text, x => wrappedAction());
      configurator(controller);

      return controller;
    }

    public ITestController<TSubject, TResult, TVars, TSequence> CreateCompositeTestController<TSubject, TResult, TVars, TSequence> (
        IEnumerable<ITestController<TSubject, TResult, TVars, TSequence>> controllers)
    {
      return new CompositeTestController<TSubject, TResult, TVars, TSequence>(controllers.ToList(), this);
    }

    public ITestController<TSubject, TResult, TVars, TSequence> CreateTestController<TSubject, TResult, TVars, TSequence> (
        SuiteProvider suiteProvider,
        TestProvider provider,
        TestContext<TSubject, TResult, TVars, TSequence> context)
    {
      return new TestController<TSubject, TResult, TVars, TSequence>(suiteProvider, provider, context, _operationSorter, this);
    }

    private Action GuardAction<TSubject, TResult, TVars, TSequence> (MainTestContext<TSubject, TResult, TVars, TSequence> context, Action<TSubject> action)
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