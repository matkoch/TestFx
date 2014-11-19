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
using TestFx.Evaluation.Results;
using TestFx.Extensibility.Providers;

namespace TestFx.Evaluation.Runners
{
  public interface IContextRunner
  {
    IContextScope Run (IEnumerable<IOperationProvider> contextProviders);
  }

  public interface IContextScope : IDisposable
  {
    IEnumerable<IOperationResult> SetupResults { get; }
    IEnumerable<IOperationResult> CleanupResults { get; }
  }

  public class ContextRunner : IContextRunner
  {
    private sealed class ContextScope : IContextScope
    {
      private readonly IEnumerable<IOperationResult> _setupResults;
      private readonly IOperationRunner _operationRunner;
      private readonly IList<IOperationProvider> _cleanupProviders;

      private readonly List<IOperationResult> _cleanupResults;

      public ContextScope (IEnumerable<IOperationResult> setupResults, IOperationRunner operationRunner, IList<IOperationProvider> cleanupProviders)
      {
        _setupResults = setupResults;
        _operationRunner = operationRunner;
        _cleanupProviders = cleanupProviders;
        _cleanupResults = new List<IOperationResult>();
      }

      public void Dispose ()
      {
        _cleanupResults.AddRange(_cleanupProviders.Select(x => _operationRunner.Run(x)));
      }

      public IEnumerable<IOperationResult> SetupResults
      {
        get { return _setupResults; }
      }

      public IEnumerable<IOperationResult> CleanupResults
      {
        get { return _cleanupResults; }
      }
    }

    private readonly IOperationRunner _operationRunner;

    public ContextRunner (IOperationRunner operationRunner)
    {
      _operationRunner = operationRunner;
    }

    public IContextScope Run (IEnumerable<IOperationProvider> contextProviders)
    {
      var setupResults = new List<IOperationResult>();
      var cleanupProviders = new Stack<IOperationProvider>();
      foreach (var contextProvider in contextProviders)
      {
        var setupResult = _operationRunner.Run(contextProvider);
        setupResults.Add(setupResult);
        if (setupResult.State == State.Passed && contextProvider.CleanupProvider != null)
          cleanupProviders.Push(contextProvider.CleanupProvider);
      }

      return new ContextScope(setupResults, _operationRunner, cleanupProviders.ToList());
    }
  }
}