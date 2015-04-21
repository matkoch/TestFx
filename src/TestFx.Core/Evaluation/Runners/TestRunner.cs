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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.Evaluation.Utilities;
using TestFx.Extensibility.Providers;

namespace TestFx.Evaluation.Runners
{
  public interface ITestRunner
  {
    ITestResult Run (ITestIntent intent, ITestProvider provider);
  }

  public class TestRunner : ITestRunner
  {
    private readonly IResultFactory _resultFactory;
    private readonly IOperationRunner _operationRunner;
    private readonly IRunListener _listener;

    public TestRunner (IResultFactory resultFactory, IOperationRunner operationRunner, IRunListener listener)
    {
      _resultFactory = resultFactory;
      _operationRunner = operationRunner;
      _listener = listener;
    }

    public ITestResult Run (ITestIntent intent, ITestProvider provider)
    {
      if (provider.Ignored)
        return _resultFactory.CreateIgnoredTestResult(provider);

      _listener.OnTestStarted(intent);

      IOutputRecording outputRecording;
      var operationResults = new List<IOperationResult>();
      var cleanupProviderStack = new Stack<IOperationProvider>();

      using (outputRecording = _resultFactory.CreateOutputRecording())
      {
        foreach (var operationProvider in provider.OperationProviders)
        {
          Trace.Assert(
              !cleanupProviderStack.Contains(operationProvider) || operationProvider == cleanupProviderStack.Pop(),
              string.Format("Cleanup ({0}) is not in order to setup.", operationProvider.Action));

          var operationResult = _operationRunner.Run(operationProvider);
          operationResults.Add(operationResult);

          if (operationResult.State == State.Failed && operationResult.Type != OperationType.Assertion)
            break;

          if (operationProvider.CleanupProvider != null)
            cleanupProviderStack.Push(operationProvider.CleanupProvider);
        }

        Trace.Assert(
            !cleanupProviderStack.Any() || operationResults.Any(x => x.State == State.Failed),
            "Either cleanup stack must be empty, or any result must have failed.");

        operationResults.AddRange(cleanupProviderStack.Select(_operationRunner.Run));
      }

      var result = _resultFactory.CreateTestResult(provider, outputRecording, operationResults);

      _listener.OnTestFinished(result);

      return result;
    }
  }
}