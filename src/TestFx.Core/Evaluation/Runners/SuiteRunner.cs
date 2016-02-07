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
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.Evaluation.Utilities;
using TestFx.Extensibility.Providers;

namespace TestFx.Evaluation.Runners
{
  public interface ISuiteRunner
  {
    ISuiteResult Run (IIntent intent, ISuiteProvider provider);
  }

  public class SuiteRunner : ISuiteRunner
  {
    private readonly IResourceManager _resourceManager;
    private readonly IResultFactory _resultFactory;
    private readonly IContextRunner _contextRunner;
    private readonly ITestRunner _testRunner;
    private readonly IRunListener _listener;
    private readonly ICancellationTokenSource _cancellationTokenSource;

    public SuiteRunner (
        IResourceManager resourceManager,
        IResultFactory resultFactory,
        IContextRunner contextRunner,
        ITestRunner testRunner,
        IRunListener listener,
        ICancellationTokenSource cancellationTokenSource)
    {
      _resourceManager = resourceManager;
      _resultFactory = resultFactory;
      _contextRunner = contextRunner;
      _testRunner = testRunner;
      _listener = listener;
      _cancellationTokenSource = cancellationTokenSource;
    }

    public ISuiteResult Run (IIntent intent, ISuiteProvider provider)
    {
      if (provider.Ignored)
        return _resultFactory.CreateIgnoredSuiteResult(provider);

      using (_resourceManager.Acquire(provider.Resources))
      {
        var suitePairs = Pair(intent.Intents, provider.SuiteProviders);
        var testPairs = Pair(intent.Intents, provider.TestProviders);

        return RunWithResourcesAcquired(intent, provider, suitePairs, testPairs);
      }
    }

    private ISuiteResult RunWithResourcesAcquired (
        IIntent intent,
        ISuiteProvider provider,
        IEnumerable<Tuple<IIntent, ISuiteProvider>> suitePairs,
        IEnumerable<Tuple<IIntent, ITestProvider>> testPairs)
    {
      _listener.OnSuiteStarted(intent, provider.Text);

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
            suiteResults = suitePairs
#if PARALLEL
                .AsParallel()
                .WithCancellation(_cancellationTokenSource.Token)
#endif
                .Select(x => Run(x.Item1, x.Item2)).ToList();

            testResults = testPairs.Select(x => _testRunner.Run(x.Item1, x.Item2)).ToList();
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

    private IEnumerable<Tuple<IIntent, TProvider>> Pair<TProvider> (IEnumerable<IIntent> intents, IEnumerable<TProvider> providers)
        where TProvider : IProvider
    {
      var providersList = providers.ToList();
      var intentsList = intents.ToList();

      var pairs = intentsList.Join(
          providersList,
          x => x.Identity.Relative,
          x => x.Identity.Relative,
          (intent, provider) => new Tuple<IIntent, TProvider>(intent, provider)).ToList();

      return intentsList.Count > 0
          ? pairs
          : providersList.Select(provider => new Tuple<IIntent, TProvider>(Intent.Create(provider.Identity), provider));
    }
  }
}