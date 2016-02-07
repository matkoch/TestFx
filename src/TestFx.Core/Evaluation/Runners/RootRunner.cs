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
using System.Linq;
using System.Reflection;
using System.Threading;
using JetBrains.Annotations;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.Evaluation.Utilities;

namespace TestFx.Evaluation.Runners
{
  public interface IRootRunner
  {
    IRunResult Run (IRunIntent intent);
  }

  public class RootRunner : IRootRunner
  {
    private enum TestType
    {
      WhiteBox,
      GrayBox,
      BlackBox,
      Unknown
    }

    private readonly Dictionary<string, TestType> s_testTypes =
        new Dictionary<string, TestType>
        {
            { "Unit", TestType.WhiteBox },
            { "Specs", TestType.WhiteBox },
            { "Integration", TestType.GrayBox },
            { "Component", TestType.GrayBox },
            { "Web", TestType.BlackBox },
            { "Acceptance", TestType.BlackBox },
            { "System", TestType.BlackBox }
        };

    private readonly IRunListener _listener;
    private readonly IAppDomainFactory _appDomainFactory;
    private readonly IResourceManager _resourceManager;
    private readonly IResultFactory _resultFactory;

    public RootRunner (IRunListener listener, IAppDomainFactory appDomainFactory, IResourceManager resourceManager, IResultFactory resultFactory)
    {
      _listener = listener;
      _appDomainFactory = appDomainFactory;
      _resourceManager = resourceManager;
      _resultFactory = resultFactory;
    }

    public IRunResult Run (IRunIntent intent)
    {
      _listener.OnRunStarted(intent);

      IRunResult result;
      try
      {
        var suiteResults = intent.Intents
            .Select(x => Tuple.Create(x, Assembly.LoadFrom(x.Identity.Relative)))
            //.OrderBy(x => GetTestType(x.Item2))
            //.Select(x => RunAssemblySuites(x.Item2, intent.ShadowCopyPath, intent.CancellationTokenSource, x.Item1)).ToList();
            .GroupBy(x => GetTestType(x.Item2))
            .OrderByDescending(x => x.Key)
            .SelectMany(
                group => group
#if PARALLEL
                    .AsParallel()
                    .WithCancellation(intent.CancellationTokenSource.Token)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
#endif
                    .Select(x => RunAssemblySuites(x.Item2, intent.ShadowCopyPath, intent.CancellationTokenSource, x.Item1))).ToList();
        result = _resultFactory.CreateRunResult(intent, suiteResults);
      }
      catch (Exception exception)
      {
        _listener.OnError(ExceptionDescriptor.Create(exception));
        throw;
      }

      _listener.OnRunFinished(result);

      return result;
    }

    private ISuiteResult RunAssemblySuites (
        Assembly assembly,
        [CanBeNull] string cachePath,
        CancellationTokenSource cancellationTokenSource,
        IIntent assemblyIntent)
    {
      using (var appDomain = _appDomainFactory.Create(assembly, cachePath))
      {
        var cancellation = appDomain.CreateProxy<ICancellation>(typeof (CrossAppDomainCancellation));
        var assemblyRunnerFactory = appDomain.CreateProxy<IAssemblyRunnerFactory>(typeof (AssemblyRunnerFactory));

        var assemblyRunner = assemblyRunnerFactory.Create(_listener, _resourceManager, cancellation);
        cancellationTokenSource.Token.Register(cancellation.Cancel);

        return assemblyRunner.Run(assemblyIntent);
      }
    }

    private TestType GetTestType (Assembly assembly)
    {
      foreach (var association in s_testTypes)
        if (assembly.GetName().Name.Contains(association.Key))
          return association.Value;

      return TestType.Unknown;
    }
  }
}