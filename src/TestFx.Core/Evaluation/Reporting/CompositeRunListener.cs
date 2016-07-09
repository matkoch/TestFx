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
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Results;
using TestFx.Utilities.Collections;

namespace TestFx.Evaluation.Reporting
{
  internal class CompositeRunListener : RunListener
  {
    private readonly ICollection<IRunListener> _listeners;

    public CompositeRunListener (params IRunListener[] listeners)
    {
      _listeners = listeners;
    }

    public override void OnRunStarted (IRunIntent intent)
    {
      _listeners.ForEach(x => x.OnRunStarted(intent));
    }

    public override void OnRunFinished (IRunResult result)
    {
      _listeners.ForEach(x => x.OnRunFinished(result));
    }

    public override void OnSuiteStarted (IIntent intent, string text)
    {
      _listeners.ForEach(x => x.OnSuiteStarted(intent, text));
    }

    public override void OnSuiteFinished (ISuiteResult result)
    {
      _listeners.ForEach(x => x.OnSuiteFinished(result));
    }

    public override void OnTestStarted (IIntent intent, string text)
    {
      _listeners.ForEach(x => x.OnTestStarted(intent, text));
    }

    public override void OnTestFinished (ITestResult result)
    {
      _listeners.ForEach(x => x.OnTestFinished(result));
    }

    public override void OnError (IExceptionDescriptor exception)
    {
      _listeners.ForEach(x => x.OnError(exception));
    }
  }
}