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
using System.Linq;
using System.Threading;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.Evaluation.Utilities;
using TestFx.Extensibility.Providers;
using TestFx.Utilities;

namespace TestFx.Evaluation.Runners
{
  public interface ISuiteRunner
  {
    ISuiteResult Run (ISuiteIntent intent, ISuiteProvider provider);
  }

  public class SuiteRunner : ISuiteRunner
  {
    private readonly IResourceManager _resourceManager;
    private readonly IIntentProviderPairer _intentProviderPairer;
    private readonly IResultFactory _resultFactory;
    private readonly IContextRunner _contextRunner;
    private readonly ITestRunner _testRunner;
    private readonly IRunListener _listener;
    private readonly ICancellationTokenSource _cancellationTokenSource;

    public SuiteRunner (
        IResourceManager resourceManager,
        IIntentProviderPairer intentProviderPairer,
        IResultFactory resultFactory,
        IContextRunner contextRunner,
        ITestRunner testRunner,
        IRunListener listener,
        ICancellationTokenSource cancellationTokenSource)
    {
      _resourceManager = resourceManager;
      _intentProviderPairer = intentProviderPairer;
      _resultFactory = resultFactory;
      _contextRunner = contextRunner;
      _testRunner = testRunner;
      _listener = listener;
      _cancellationTokenSource = cancellationTokenSource;
    }

    public ISuiteResult Run (ISuiteIntent intent, ISuiteProvider provider)
    {
      if (provider.Ignored)
        return _resultFactory.CreateIgnoredSuiteResult(provider);

      using (_resourceManager.Acquire(new string[0]))
      {
        var suitePairs = _intentProviderPairer.Pair(intent.SuiteIntents, provider.SuiteProviders, p => SuiteIntent.Create(p.Identity));
        var testPairs = _intentProviderPairer.Pair(intent.TestIntents, provider.TestProviders, p => TestIntent.Create(p.Identity));

        return RunWithResourcesAcquired(intent, provider, suitePairs, testPairs);
      }
    }

    private ISuiteResult RunWithResourcesAcquired (
        ISuiteIntent intent,
        ISuiteProvider provider,
        IEnumerable<Tuple<ISuiteIntent, ISuiteProvider>> suitePairs,
        IEnumerable<Tuple<ITestIntent, ITestProvider>> testPairs)
    {
      _listener.OnSuiteStarted(intent);

      IOutputRecording outputRecording;
      IContextScope contextScope;
      ICollection<ISuiteResult> suiteResults = new ISuiteResult[0];
      ICollection<ITestResult> testResults = new ITestResult[0];

      using (outputRecording = _resultFactory.CreateOutputRecording())
      {
        using (contextScope = _contextRunner.Run(provider.ContextProviders))
        {
          if (contextScope.SetupResults.All(x => x.State == State.Passed))
          {
            suiteResults = suitePairs.Select(x => RunRecursive(x.Item1, x.Item2)).ToList();

            testResults = testPairs.AsParallel().WithDegreeOfParallelism(1).WithCancellation(_cancellationTokenSource.Token)
                .Select(x => _testRunner.Run(x.Item1, x.Item2)).ToList();
          }
        }
      }

      var result = _resultFactory.CreateSuiteResult(
          provider,
          outputRecording,
          contextScope.SetupResults,
          contextScope.CleanupResults,
          suiteResults,
          testResults);

      _listener.OnSuiteFinished(result);

      return result;
    }

    protected virtual ISuiteResult RunRecursive (ISuiteIntent intent, ISuiteProvider provider)
    {
      return Run(intent, provider);
    }
  }
}