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
using JetBrains.Annotations;
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.Specifications.Implementation.Utilities;
using TestFx.Specifications.InferredApi;

namespace TestFx.Specifications.Implementation.Controllers
{
  public interface IClassSuiteController<TSubject> : ISuiteController
  {
    void AddTestSetupCleanup (Action<ITestContext<TSubject>> setup, [CanBeNull] Action<ITestContext<TSubject>> cleanup);

    ISpecializedSuiteController<TSubject, TResult> CreateSpecializedSuiteController<TResult> (Action<TSubject> voidExpression);
    ISpecializedSuiteController<TSubject, TResult> CreateSpecializedSuiteController<TResult> (Func<TSubject, TResult> resultExpression);
  }

  public class ClassSuiteController<TSubject> : ClassSuiteController, IClassSuiteController<TSubject>
  {
    private readonly SuiteProvider _provider;
    private readonly ISpecK<TSubject> _suite;
    private readonly IControllerFactory _controllerFactory;
    private readonly List<Tuple<Action<ITestContext<TSubject>>, Action<ITestContext<TSubject>>>> _testSetupCleanupTuples;

    private bool _controllerAdded;

    public ClassSuiteController (
        SuiteProvider provider,
        ISpecK<TSubject> suite,
        IEnumerable<ITestExtension> testExtensions,
        IControllerFactory controllerFactory,
        IOperationSorter operationSorter)
        : base(provider, suite, testExtensions, operationSorter)
    {
      _provider = provider;
      _suite = suite;
      _controllerFactory = controllerFactory;
      _testSetupCleanupTuples = new List<Tuple<Action<ITestContext<TSubject>>, Action<ITestContext<TSubject>>>>();
    }

    public void AddTestSetupCleanup (Action<ITestContext<TSubject>> setup, [CanBeNull] Action<ITestContext<TSubject>> cleanup)
    {
      Trace.Assert(!_controllerAdded);

      _testSetupCleanupTuples.Add(Tuple.Create(setup, cleanup));
    }

    public ISpecializedSuiteController<TSubject, TResult> CreateSpecializedSuiteController<TResult> (Action<TSubject> voidExpression)
    {
      return CreateSpecializedSuiteController<TResult>(voidExpression, null);
    }

    public ISpecializedSuiteController<TSubject, TResult> CreateSpecializedSuiteController<TResult> (Func<TSubject, TResult> resultExpression)
    {
      return CreateSpecializedSuiteController(null, resultExpression);
    }

    public override void ConfigureTestController (ITestController testController)
    {
      _controllerAdded = true;

      base.ConfigureTestController(testController);

      var testControllerWithSubject = (ITestController<TSubject>) testController;

      if (typeof (TSubject) != typeof (Dummy))
        testControllerWithSubject.SetSubjectFactory<SetupSubject>("<Create_Subject>", x => _suite.CreateSubject());

      _testSetupCleanupTuples.ForEach(
          x => testControllerWithSubject.AddSetupCleanup<SetupCommon, CleanupCommon>(ConvertToNonGeneric(x.Item1), ConvertToNonGeneric(x.Item2)));
    }

    private ISpecializedSuiteController<TSubject, TResult> CreateSpecializedSuiteController<TResult> (
        [CanBeNull] Action<TSubject> voidAction,
        [CanBeNull] Func<TSubject, TResult> resultAction)
    {
      //var actionText = _introspectionPresenter.Present("{0}", new[] { expression.ToCommon(typeof (ISuite), typeof (ITestContext)) });
      var actionContainer = new ActionContainer<TSubject, TResult>("<Action>", voidAction, resultAction);
      return _controllerFactory.CreateSpecializedSuiteController(_provider, actionContainer, ConfigureTestController);
    }

    private Action<Extensibility.Contexts.ITestContext> ConvertToNonGeneric (Action<ITestContext<TSubject>> action)
    {
      return action != null ? (x => action((ITestContext<TSubject>) x)) : (Action<Extensibility.Contexts.ITestContext>) null;
    }
  }
}