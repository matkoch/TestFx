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
using System.Linq;
using TestFx.Extensibility;
using TestFx.SpecK.Implementation.Containers;
using TestFx.SpecK.Implementation.Controllers;
using TestFx.SpecK.Implementation.Utilities;
using TestFx.SpecK.InferredApi;

namespace TestFx.SpecK
{
  public abstract class Spec<TSubject> : ISuite<TSubject>
  {
    // ReSharper disable UnassignedField.Compiler
    private IClassSuiteController<TSubject> _classSuiteController;
    private ISubjectFactory _subjectFactory;
    // ReSharper restore UnassignedField.Compiler

    public void SetupOnce (Action setup, Action cleanup = null)
    {
      _classSuiteController.AddSetupCleanup<SetupCommon, CleanupCommon>(setup, cleanup);
    }

    public void Setup (Action<ITestContext<TSubject>> setup, Action<ITestContext<TSubject>> cleanup = null)
    {
      _classSuiteController.AddTestSetupCleanup(setup, cleanup);
    }

    public IIgnoreOrCase<TSubject, Dummy> Specify (Action<TSubject> action)
    {
      var expressionSuiteController = _classSuiteController.CreateSpecializedSuiteController<Dummy>(action);
      return new SpecializedSuiteContainer<TSubject, Dummy>(expressionSuiteController);
    }

    public IIgnoreOrCase<TSubject, TResult> Specify<TResult> (Func<TSubject, TResult> action)
    {
      var expressionSuiteController = _classSuiteController.CreateSpecializedSuiteController(action);
      return new SpecializedSuiteContainer<TSubject, TResult>(expressionSuiteController);
    }

    public IIgnoreOrCase<TSubject, IList<TItem>> Specify<TItem> (Func<TSubject, IEnumerable<TItem>> action)
    {
      return Specify(x => (IList<TItem>) action(x).ToList());
    }

    public virtual TSubject CreateSubject ()
    {
      return _subjectFactory.CreateFor(this);
    }
  }

  public abstract class Spec : Spec<Dummy>
  {
    public override sealed Dummy CreateSubject ()
    {
      throw new NotSupportedException();
    }
  }
}