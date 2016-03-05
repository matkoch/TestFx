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
using System.Diagnostics;
using JetBrains.Annotations;
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.SpecK.Implementation.Utilities;
using TestFx.SpecK.InferredApi;
using TestFx.Utilities;

namespace TestFx.SpecK.Implementation.Controllers
{
  public interface IClassSuiteController<TSubject> : ISuiteController
  {
    void AddTestSetupCleanup (Action<ITestContext<TSubject>> setup, [CanBeNull] Action<ITestContext<TSubject>> cleanup);

    ISpecializedSuiteController<TSubject, TResult> CreateSpecializedSuiteController<TResult> (Func<TSubject, TResult> action);
  }

  internal class ClassSuiteController<TSubject> : ClassSuiteController, IClassSuiteController<TSubject>
  {
    private readonly SuiteProvider _provider;
    private readonly ISuite<TSubject> _suite;
    private readonly IControllerFactory _controllerFactory;
    private readonly List<Tuple<Action<ITestContext<TSubject>>, Action<ITestContext<TSubject>>>> _testSetupCleanupTuples;

    private bool _controllerAdded;

    public ClassSuiteController(
        SuiteProvider provider,
        ISuite<TSubject> suite,
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

    public ISpecializedSuiteController<TSubject, TResult> CreateSpecializedSuiteController<TResult> (Func<TSubject, TResult> action)
    {
      var actionContainer = new ActionContainer<TSubject, TResult>("<Action>", action);
      return _controllerFactory.CreateSpecializedSuiteController(_provider, actionContainer, ConfigureTestController);
    }

    public override void ConfigureTestController (ITestController testController)
    {
      _controllerAdded = true;

      base.ConfigureTestController(testController);

      var testControllerWithSubject = (ITestController<TSubject>) testController;

      if (typeof (TSubject) != typeof (Dummy))
        testControllerWithSubject.SetSubjectFactory<SetupSubject>("<Create_Subject>", x => _suite.CreateSubject());

      _testSetupCleanupTuples.ForEach(
          x => testControllerWithSubject.AddSetupCleanup<SetupCommon, CleanupCommon>(
              setup: ConvertToNonGeneric(x.Item1).NotNull(),
              cleanup: ConvertToNonGeneric(x.Item2)));
    }

    [CanBeNull]
    private Action<Extensibility.Contexts.ITestContext> ConvertToNonGeneric ([CanBeNull] Action<ITestContext<TSubject>> action)
    {
      return action != null ? (x => action((ITestContext<TSubject>) x)) : (Action<Extensibility.Contexts.ITestContext>) null;
    }
  }
}