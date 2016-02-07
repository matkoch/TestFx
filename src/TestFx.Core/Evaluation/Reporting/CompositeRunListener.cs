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
  public class CompositeRunListener : IRunListener
  {
    private readonly ICollection<IRunListener> _listeners;

    public CompositeRunListener (params IRunListener[] listeners)
    {
      _listeners = listeners;
    }

    public void OnError (IExceptionDescriptor exception)
    {
      _listeners.ForEach(x => x.OnError(exception));
    }

    public void OnRunFinished (IRunResult result)
    {
      _listeners.ForEach(x => x.OnRunFinished(result));
    }

    public void OnRunStarted (IRunIntent intent)
    {
      _listeners.ForEach(x => x.OnRunStarted(intent));
    }

    public void OnSuiteFinished (ISuiteResult result)
    {
      _listeners.ForEach(x => x.OnSuiteFinished(result));
    }

    public void OnSuiteStarted (IIntent intent)
    {
      _listeners.ForEach(x => x.OnSuiteStarted(intent));
    }

    public void OnTestFinished (ITestResult result)
    {
      _listeners.ForEach(x => x.OnTestFinished(result));
    }

    public void OnTestStarted (IIntent intent)
    {
      _listeners.ForEach(x => x.OnTestStarted(intent));
    }
  }
}