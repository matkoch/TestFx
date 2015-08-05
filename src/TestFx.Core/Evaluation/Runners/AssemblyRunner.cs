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
using System.Diagnostics;
using System.Linq;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Loading;
using TestFx.Evaluation.Results;
using TestFx.Evaluation.Utilities;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.Evaluation.Runners
{
  public interface IAssemblyRunner
  {
    ISuiteResult Run (IIntent assemblyIntent);
  }

  public class AssemblyRunner : MarshalByRefObject, IAssemblyRunner
  {
    private readonly IAssemblyLoader _assemblyLoader;
    private readonly ISuiteRunner _suiteRunner;

    public AssemblyRunner (IAssemblyLoader assemblyLoader, ISuiteRunner suiteRunner)
    {
      _assemblyLoader = assemblyLoader;
      _suiteRunner = suiteRunner;
    }

    public ISuiteResult Run (IIntent assemblyIntent)
    {
      using (SetupTraceListeners())
      {
        var suiteProvider = _assemblyLoader.Load(assemblyIntent);
        return _suiteRunner.Run(assemblyIntent, suiteProvider);
      }
    }

    private IDisposable SetupTraceListeners ()
    {
      var defaultTraceListener = Trace.Listeners.OfType<DefaultTraceListener>().Single();
      var previousAssertUiEnabled = defaultTraceListener.AssertUiEnabled;

      defaultTraceListener.AssertUiEnabled = false;
      var throwingTraceListener = new ThrowingTraceListener();
      Trace.Listeners.Add(throwingTraceListener);

      return new DelegateDisposable(
          () =>
          {
            defaultTraceListener.AssertUiEnabled = previousAssertUiEnabled;
            Trace.Listeners.Remove(throwingTraceListener);
          });
    }
  }
}