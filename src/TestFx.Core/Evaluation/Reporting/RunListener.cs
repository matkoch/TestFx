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
using System.Linq;
using JetBrains.Annotations;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Results;

namespace TestFx.Evaluation.Reporting
{
  [PublicAPI]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public interface IRunListener
  {
    void OnRunStarted (IRunIntent intent);
    void OnRunFinished (IRunResult result);

    void OnSuiteStarted (IIntent intent, string text);
    void OnSuiteFinished (ISuiteResult result);

    void OnTestStarted (IIntent intent, string text);
    void OnTestFinished (ITestResult result);

    void OnError (IExceptionDescriptor exception);
  }

  public class RunListener : MarshalByRefObject, IRunListener
  {
    public virtual void OnRunStarted (IRunIntent intent)
    {
    }

    public virtual void OnRunFinished (IRunResult result)
    {
    }

    public virtual void OnSuiteStarted (IIntent intent, string text)
    {
    }

    public virtual void OnSuiteFinished (ISuiteResult result)
    {
    }

    public virtual void OnTestStarted (IIntent intent, string text)
    {
    }

    public virtual void OnTestFinished (ITestResult result)
    {
    }

    public virtual void OnError (IExceptionDescriptor exception)
    {
    }
  }
}