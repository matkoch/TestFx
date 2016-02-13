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
using TestFx.Evaluation.Results;
using TestFx.Extensibility.Providers;

namespace TestFx.Evaluation.Runners
{
  internal class ContextRunnerBase
  {
    protected class ContextResult
    {
      public List<IOperationResult> OperationResults;
      public Stack<IOperationProvider> CleanupProviders;
    }

    private readonly IOperationRunner _operationRunner;

    protected ContextRunnerBase (IOperationRunner operationRunner)
    {
      _operationRunner = operationRunner;
    }

    protected ContextResult Run (IEnumerable<IOperationProvider> operationProviders)
    {
      var operationResults = new List<IOperationResult>();
      var cleanupProviderStack = new Stack<IOperationProvider>();

      foreach (var operationProvider in operationProviders)
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

      return new ContextResult
             {
                 OperationResults = operationResults,
                 CleanupProviders = cleanupProviderStack
             };
    }
  }
}