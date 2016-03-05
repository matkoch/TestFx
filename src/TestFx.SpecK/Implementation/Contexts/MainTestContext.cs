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
using TestFx.Extensibility.Controllers;
using TestFx.SpecK.Implementation.Utilities;
using TestFx.SpecK.InferredApi;

namespace TestFx.SpecK.Implementation.Contexts
{
  internal class MainTestContext<TSubject, TResult, TVars, TSequence> : TestContext<TSubject, TResult, TVars, TSequence>
  {
    private TSubject _subject;
    private TResult _result;
    private Exception _exception;
    private TimeSpan _duration;
    private object _varsObject;
    private object _sequenceObject;

    public MainTestContext (ActionContainer<TSubject, TResult> actionContainer, Action<ITestController> configurator)
    {
      ActionContainer = actionContainer;
      Configurator = configurator;
    }

    public ActionContainer<TSubject, TResult> ActionContainer { get; }

    public Action<ITestController> Configurator { get; }

    public bool ActionExecuted { get; set; }

    public override TSubject Subject
    {
      get
      {
        if (typeof (TSubject) == typeof (Dummy))
          throw new NotSupportedException("Non-generic Spec classes do not provide a subject instance.");

        if (typeof (TSubject).IsClass && ReferenceEquals(_subject, default(TSubject)))
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
          throw new NotSupportedException("Void actions do not have a result.");

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

    public override TSequence Sequence
    {
      get { return (TSequence) _sequenceObject; }
      set { _sequenceObject = value; }
    }

    public override object SeqObject
    {
      get { return _sequenceObject; }
      set { _sequenceObject = value; }
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
        throw new Exception($"Action must be executed before accessing {propertyName}.");
    }
  }
}