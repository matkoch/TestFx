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
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.Specifications.Implementation.Utilities;
using TestFx.Specifications.InferredApi;
using JetBrains.Annotations;
using TestFx.Utilities;
using TestFx.Utilities.Expressions;

namespace TestFx.Specifications.Implementation.Controllers
{
  public interface IClassSuiteController<TSubject> : IClassSuiteController
  {
    void AddTestSetupCleanup (Action<ITestContext<TSubject>> setup, [CanBeNull] Action<ITestContext<TSubject>> cleanup);

    IExpressionSuiteController<TSubject, TResult> CreateExpressionSuiteController<TResult> (
        string displayFormat,
        Expression<Action<TSubject>> voidExpression);

    IExpressionSuiteController<TSubject, TResult> CreateExpressionSuiteController<TResult> (
        string displayFormat,
        Expression<Func<TSubject, TResult>> resultExpression);
  }

  public class ClassSuiteController<TSubject> : ClassSuiteController, IClassSuiteController<TSubject>
  {
    private readonly ISpecK<TSubject> _suite;
    private readonly IControllerFactory _controllerFactory;
    private readonly IIntrospectionPresenter _introspectionPresenter;
    private readonly List<Tuple<Action<ITestContext<TSubject>>, Action<ITestContext<TSubject>>>> _testSetupCleanupTuples;

    private int _sequenceNumber;
    private bool _controllerAdded;

    public ClassSuiteController (
        SuiteProvider provider,
        ISpecK<TSubject> suite,
        IEnumerable<ITestExtension> testExtensions,
        IControllerFactory controllerFactory,
        IOperationSorter operationSorter,
        IIntrospectionPresenter introspectionPresenter)
        : base(provider, suite, testExtensions, operationSorter)
    {
      _suite = suite;
      _controllerFactory = controllerFactory;
      _introspectionPresenter = introspectionPresenter;
      _testSetupCleanupTuples = new List<Tuple<Action<ITestContext<TSubject>>, Action<ITestContext<TSubject>>>>();
    }

    public void AddTestSetupCleanup (Action<ITestContext<TSubject>> setup, [CanBeNull] Action<ITestContext<TSubject>> cleanup)
    {
      Trace.Assert(!_controllerAdded);

      _testSetupCleanupTuples.Add(Tuple.Create(setup, cleanup));
    }

    public IExpressionSuiteController<TSubject, TResult> CreateExpressionSuiteController<TResult> (
        string displayFormat,
        Expression<Action<TSubject>> voidExpression)
    {
      return CreateExpressionSuiteController<TResult>(displayFormat, voidExpression, voidExpression.Compile(), null);
    }

    public IExpressionSuiteController<TSubject, TResult> CreateExpressionSuiteController<TResult> (
        string displayFormat,
        Expression<Func<TSubject, TResult>> resultExpression)
    {
      return CreateExpressionSuiteController(displayFormat, resultExpression, null, resultExpression.Compile());
    }

    public override void ConfigureTestController (ITestController testController)
    {
      _controllerAdded = true;

      base.ConfigureTestController(testController);

      var testControllerWithSubject = testController.To<ITestController<TSubject>>();

      if (typeof (TSubject) != typeof (Dummy))
        testControllerWithSubject.SetSubjectFactory<SetupSubject>("create subject", x => _suite.CreateSubject());

      _testSetupCleanupTuples.ForEach(
          x => testControllerWithSubject.AddSetupCleanup<SetupCommon, CleanupCommon>(ConvertToNonGeneric(x.Item1), ConvertToNonGeneric(x.Item2)));
    }

    private IExpressionSuiteController<TSubject, TResult> CreateExpressionSuiteController<TResult> (
        string displayFormat,
        Expression expression,
        [CanBeNull] Action<TSubject> voidAction,
        [CanBeNull] Func<TSubject, TResult> resultAction)
    {
      var actionText = _introspectionPresenter.Present(displayFormat, new[] { expression.ToCommon(typeof(ISuite), typeof(ITestContext)) });
      var actionContainer = new ActionContainer<TSubject, TResult>(actionText, voidAction, resultAction);
      var provider = CreateSuiteProvider(_sequenceNumber++.ToString(CultureInfo.InvariantCulture), actionContainer.Text, false);
      return _controllerFactory.CreateExpressionSuiteController(provider, actionContainer, this);
    }

    private Action<Extensibility.Contexts.ITestContext> ConvertToNonGeneric (Action<ITestContext<TSubject>> action)
    {
      return action != null ? (x => action(x.To<ITestContext<TSubject>>())) : (Action<Extensibility.Contexts.ITestContext>) null;
    }
  }
}