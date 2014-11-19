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
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Evaluation.Results
{
  public interface ITestResult : IOutputResult
  {
    IEnumerable<IOperationResult> OperationResults { get; }
  }

  [Serializable]
  public class TestResult : OutputResult, ITestResult
  {
    private readonly ICollection<IOperationResult> _operationResults;

    public TestResult (
        IIdentity identity,
        string text,
        State state,
        ICollection<OutputEntry> outputEntries,
        ICollection<IOperationResult> operationResults)
        : base(identity, text, state, outputEntries)
    {
      _operationResults = operationResults;
    }

    public IEnumerable<IOperationResult> OperationResults
    {
      get { return _operationResults; }
    }
  }
}