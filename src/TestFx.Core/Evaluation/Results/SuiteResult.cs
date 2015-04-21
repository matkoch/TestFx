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
using TestFx.Utilities;

namespace TestFx.Evaluation.Results
{
  public interface ISuiteResult : ISuiteResultHolder, IOutputResult
  {
    IEnumerable<IOperationResult> SetupResults { get; }
    IEnumerable<IOperationResult> CleanupResults { get; }

    IEnumerable<ITestResult> TestResults { get; }
  }

  [Serializable]
  public class SuiteResult : OutputResult, ISuiteResult
  {
    private readonly ICollection<IOperationResult> _setupResults;
    private readonly ICollection<IOperationResult> _cleanupResults;
    private readonly ICollection<ISuiteResult> _suiteResults;
    private readonly ICollection<ITestResult> _testResults;

    public SuiteResult (
        IIdentity identity,
        string text,
        State state,
        ICollection<OutputEntry> outputEntries,
        ICollection<IOperationResult> setupResults,
        ICollection<IOperationResult> cleanupResults,
        ICollection<ISuiteResult> suiteResults,
        ICollection<ITestResult> testResults)
        : base(identity, text, state, outputEntries)
    {
      _setupResults = setupResults;
      _cleanupResults = cleanupResults;
      _suiteResults = suiteResults;
      _testResults = testResults;
    }

    public IEnumerable<IOperationResult> SetupResults
    {
      get { return _setupResults; }
    }

    public IEnumerable<IOperationResult> CleanupResults
    {
      get { return _cleanupResults; }
    }

    public IEnumerable<ISuiteResult> SuiteResults
    {
      get { return _suiteResults; }
    }

    public IEnumerable<ITestResult> TestResults
    {
      get { return _testResults; }
    }
  }
}