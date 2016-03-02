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
using TestFx.Utilities;

namespace TestFx.Evaluation.Results
{
  public interface ITestResult : IOutputResult
  {
    TimeSpan Duration { get; }
    IEnumerable<IOperationResult> OperationResults { get; }
  }

  [Serializable]
  internal class TestResult : OutputResult, ITestResult
  {
    private readonly ICollection<IOperationResult> _operationResults;

    public TestResult (
        IIdentity identity,
        string text,
        State state,
        TimeSpan duration,
        ICollection<OutputEntry> outputEntries,
        ICollection<IOperationResult> operationResults)
        : base(identity, text, state, outputEntries)
    {
      Duration = duration;
      _operationResults = operationResults;
    }

    public TimeSpan Duration { get; }

    public IEnumerable<IOperationResult> OperationResults => _operationResults;
  }
}