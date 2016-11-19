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
using System.Threading.Tasks;
using TestFx.Extensibility;
using TestFx.SpecK.Implementation.Containers;
using TestFx.SpecK.Implementation.Controllers;
using TestFx.SpecK.Implementation.Utilities;
using TestFx.SpecK.InferredApi;

namespace TestFx.SpecK
{
  public abstract class Spec<TSubject> : ISuite<TSubject>
  {
    private IClassSuiteController<TSubject> _classSuiteController;
    private ISubjectFactory _subjectFactory;

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
      return Specify(x =>
      {
        action(x);
        return (Dummy) null;
      });
    }

    public IIgnoreOrCase<TSubject, TResult> Specify<TResult> (Func<TSubject, TResult> action)
    {
      var expressionSuiteController = _classSuiteController.CreateSpecializedSuiteController(action);
      return new SpecializedSuiteContainer<TSubject, TResult>(expressionSuiteController);
    }

    public IIgnoreOrCase<TSubject, IReadOnlyList<TItem>> Specify<TItem> (Func<TSubject, IEnumerable<TItem>> action)
    {
      return Specify(x => (IReadOnlyList<TItem>) action(x).ToList());
    }

    public IIgnoreOrCase<TSubject, Dummy> SpecifyAsync (Func<TSubject, Task> action)
    {
      return Specify(x => action(x).Wait());
    }

    public IIgnoreOrCase<TSubject, TResult> SpecifyAsync<TResult> (Func<TSubject, Task<TResult>> action)
    {
      return Specify(x => action(x).Result);
    }

    public virtual TSubject CreateSubject ()
    {
      return _subjectFactory.CreateFor(this);
    }
  }

  public abstract class Spec : Spec<Dummy>
  {
    public sealed override Dummy CreateSubject ()
    {
      throw new NotSupportedException("Non-generic Spec classes do not provide a subject instance.");
    }
  }
}