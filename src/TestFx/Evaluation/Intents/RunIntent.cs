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