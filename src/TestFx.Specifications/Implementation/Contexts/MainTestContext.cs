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
using TestFx.Extensibility.Controllers;
using TestFx.Specifications.Implementation.Utilities;
using TestFx.Specifications.InferredApi;

namespace TestFx.Specifications.Implementation.Contexts
{
  public class MainTestContext<TSubject, TResult, TVars, TCombi> : TestContext<TSubject, TResult, TVars, TCombi>
  {
    private readonly ActionContainer<TSubject, TResult> _actionContainer;
    private readonly Action<ITestController> _configurator;

    private TSubject _subject;
    private TResult _result;
    private Exception _exception;
    private TimeSpan _duration;
    private object _varsObject;
    private object _comboObject;

    public MainTestContext (ActionContainer<TSubject, TResult> actionContainer, Action<ITestController> configurator)
    {
      _actionContainer = actionContainer;
      _configurator = configurator;
    }

    public ActionContainer<TSubject, TResult> ActionContainer
    {
      get { return _actionContainer; }
    }

    public Action<ITestController> Configurator
    {
      get { return _configurator; }
    }

    public bool ActionExecuted { get; set; }

    public override TSubject Subject
    {
      get
      {
        if (typeof(TSubject) != typeof(Dummy) && typeof (TSubject).IsClass && ReferenceEquals(_subject, default(TSubject)))
          throw new Exception("Subject instance is null.");

        return _subject;
      }
      set { _subject = value; }
    }

    public override TResult Result
    {
      get
      {
        EnsureActionExecuted("Result");

        if (typeof (TResult) == typeof (Dummy))
          throw new Exception("Action does not have a result.");

        return _result;
      }
      set { _result = value; }
    }

    public override TVars Vars
    {
      get { return (TVars) _varsObject; }
      set { _varsObject = value; }
    }

    public override object VarsObject
    {
      get { return _varsObject; }
      set { _varsObject = value; }
    }

    public override TCombi Combi
    {
      get { return (TCombi) _comboObject; }
      set { _comboObject = value; }
    }

    public override object ComboObject
    {
      get { return _comboObject; }
      set { _comboObject = value; }
    }

    public override Exception Exception
    {
      get
      {
        EnsureActionExecuted("Exception");

        return _exception;
      }
      set { _exception = value; }
    }

    public override TimeSpan Duration
    {
      get
      {
        EnsureActionExecuted("Duration");

        return _duration;
      }
      set { _duration = value; }
    }

    public override bool ExpectsException { get; set; }

    private void EnsureActionExecuted (string propertyName)
    {
      if (!ActionExecuted)
        throw new Exception(string.Format("Action must be executed before accessing {0}.", propertyName));
    }
  }
}