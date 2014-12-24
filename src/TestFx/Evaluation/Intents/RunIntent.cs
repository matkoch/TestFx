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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Evaluation.Intents
{
  public interface IRunIntent : ISuiteIntentHolder
  {
    [CanBeNull]
    string ShadowCopyPath { get; }

    bool CreateSeparateAppDomains { get; }

    CancellationTokenSource CancellationTokenSource { get; }
  }

  public class RunIntent : Intent, IRunIntent
  {
    public static IRunIntent Create (bool useSeparateAppDomains = true, bool shadowCopy = true, int visualStudioProcessId = -1)
    {
      var runId = Guid.NewGuid().ToString("N").Substring(0, 8);
      var identity = new Identity(runId);
      var shadowCopyPath = shadowCopy ? Path.Combine(Path.GetTempPath(), runId) : null;

      return new RunIntent(identity, useSeparateAppDomains, shadowCopyPath);
    }

    private readonly bool _useSeparateAppDomains;
    private readonly string _shadowCopyPath;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly List<ISuiteIntent> _suiteIntents;

    private RunIntent (IIdentity identity, bool useSeparateAppDomains, [CanBeNull] string shadowCopyPath)
        : base(identity)
    {
      _useSeparateAppDomains = useSeparateAppDomains;
      _shadowCopyPath = shadowCopyPath;
      _cancellationTokenSource = new CancellationTokenSource();
      _suiteIntents = new List<ISuiteIntent>();
    }

    [CanBeNull]
    public string ShadowCopyPath
    {
      get { return _shadowCopyPath; }
    }

    public bool CreateSeparateAppDomains
    {
      get { return _useSeparateAppDomains; }
    }

    public CancellationTokenSource CancellationTokenSource
    {
      get { return _cancellationTokenSource; }
    }

    public IEnumerable<ISuiteIntent> SuiteIntents
    {
      get { return _suiteIntents; }
    }

    public void AddSuiteIntent (ISuiteIntent suiteIntent)
    {
      Trace.Assert(suiteIntent.Identity.Parent == null);
      _suiteIntents.Add(suiteIntent);
    }
  }
}