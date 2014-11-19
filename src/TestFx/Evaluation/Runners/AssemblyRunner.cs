// Copyright 2014, 2013 Matthias Koch
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
using System.Reflection;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Loading;
using TestFx.Evaluation.Results;
using TestFx.Evaluation.Utilities;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Evaluation.Runners
{
  public interface IAssemblyRunner
  {
    ISuiteResult Run (ISuiteIntent suiteIntent);
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

    public ISuiteResult Run (ISuiteIntent suiteIntent)
    {
      AddThrowingTraceListener();

      var suiteProvider = _assemblyLoader.Load(suiteIntent.Identity.Relative);
      return _suiteRunner.Run(suiteIntent, suiteProvider);
    }

    private void AddThrowingTraceListener ()
    {
      // TODO: needed per appdomain?
      // TODO: revert when elevating programatically?
      Trace.Listeners.OfType<DefaultTraceListener>().ForEach(x => x.AssertUiEnabled = false);
      Trace.Listeners.Add(new ThrowingTraceListener());
    }
  }
}