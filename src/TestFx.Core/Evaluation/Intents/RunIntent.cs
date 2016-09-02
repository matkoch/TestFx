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
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using TestFx.Evaluation.Utilities;
using TestFx.Utilities;

namespace TestFx.Evaluation.Intents
{
  public interface IRunIntent : IIntent
  {
    [CanBeNull]
    string ShadowCopyPath { get; }

    bool CreateSeparateAppDomains { get; }

    ICancellation CancellationTokenSource { get; }
  }

  [Serializable]
  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  public class RunIntent : IRunIntent
  {
    public static IRunIntent Create (bool useSeparateAppDomains = true, bool shadowCopy = true, int visualStudioProcessId = -1)
    {
      var runId = Guid.NewGuid().ToString("N").Substring(startIndex: 0, length: 8);
      var identity = new Identity(runId);
      var shadowCopyPath = shadowCopy ? Path.Combine(Path.GetTempPath(), runId) : null;

      return new RunIntent(identity, useSeparateAppDomains, shadowCopyPath);
    }

    private readonly List<IIntent> _intents;

    private RunIntent (IIdentity identity, bool useSeparateAppDomains, [CanBeNull] string shadowCopyPath)
    {
      Identity = identity;
      CreateSeparateAppDomains = useSeparateAppDomains;
      ShadowCopyPath = shadowCopyPath;
      CancellationTokenSource = new CrossAppDomainCancellation();
      _intents = new List<IIntent>();
    }

    public IIdentity Identity { get; }

    [CanBeNull]
    public string ShadowCopyPath { get; }

    public bool CreateSeparateAppDomains { get; }

    public ICancellation CancellationTokenSource { get; }

    public IEnumerable<IIntent> Intents => _intents;

    public void AddIntent (IIntent intent)
    {
      Trace.Assert(intent.Identity.Parent == null);
      _intents.Add(intent);
    }
  }
}