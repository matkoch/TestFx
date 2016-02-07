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
using TestFx.Evaluation.Utilities;
using TestFx.Extensibility.Providers;
using TestFx.Utilities;

namespace TestFx.Evaluation.Results
{
  public interface IResultFactory
  {
    IOutputRecording CreateOutputRecording ();

    IRunResult CreateRunResult (IRunIntent intent, IEnumerable<ISuiteResult> suiteResults);

    ISuiteResult CreateSuiteResult (
        ISuiteProvider provider,
        IEnumerable<OutputEntry> outputEntries,
        IEnumerable<IOperationResult> setupResults,
        IEnumerable<IOperationResult> cleanupResults,
        IEnumerable<ISuiteResult> suiteResults,
        IEnumerable<ITestResult> testResults);

    ISuiteResult CreateIgnoredSuiteResult (ISuiteProvider provider);

    ITestResult CreateTestResult (ITestProvider provider, TimeSpan duration, IEnumerable<OutputEntry> outputEntries, IEnumerable<IOperationResult> operationResults);

    ITestResult CreateIgnoredTestResult (ITestProvider provider);

    IOperationResult CreatePassedOperationResult (IOperationProvider provider);
    IOperationResult CreateFailedOperationResult (IOperationProvider provider, Exception exception);
    IOperationResult CreateInconclusiveOperationResult (IOperationProvider provider);
  }

  public class ResultFactory : IResultFactory
  {
    public IOutputRecording CreateOutputRecording ()
    {
      return new OutputRecording();
    }

    public IRunResult CreateRunResult (IRunIntent intent, IEnumerable<ISuiteResult> suiteResults)
    {
      var suiteResultsList = suiteResults.ToList();
      var state = GetOverallState(Convert(suiteResultsList));

      return new RunResult(intent.Identity, "You're so great!", state, suiteResultsList);
    }

    public ISuiteResult CreateSuiteResult (
        ISuiteProvider provider,
        IEnumerable<OutputEntry> outputEntries,
        IEnumerable<IOperationResult> setupResults,
        IEnumerable<IOperationResult> cleanupResults,
        IEnumerable<ISuiteResult> suiteResults,
        IEnumerable<ITestResult> testResults)
    {
      var setupResultsList = setupResults.ToList();
      var cleanupResultsList = cleanupResults.ToList();
      var suiteResultsList = suiteResults.ToList();
      var testResultsList = testResults.ToList();

      var allResults = Convert(setupResultsList)
          .Concat(Convert(cleanupResultsList))
          .Concat(Convert(suiteResultsList))
          .Concat(Convert(testResultsList));
      var state = GetOverallState(allResults);

      return new SuiteResult(
          provider.Identity,
          provider.Text,
          state,
          outputEntries.ToList(),
          setupResultsList,
          cleanupResultsList,
          suiteResultsList,
          testResultsList);
    }

    public ISuiteResult CreateIgnoredSuiteResult (ISuiteProvider provider)
    {
      return new SuiteResult(
          provider.Identity,
          provider.Text,
          State.Ignored,
          new OutputEntry[0],
          new IOperationResult[0],
          new IOperationResult[0],
          new ISuiteResult[0],
          new ITestResult[0]);
    }

    public ITestResult CreateTestResult (
        ITestProvider provider,
        TimeSpan duration,
        IEnumerable<OutputEntry> outputEntries,
        IEnumerable<IOperationResult> operationResults)
    {
      var operationResultsList = operationResults.ToList();
      var state = GetOverallState(Convert(operationResultsList));

      return new TestResult(provider.Identity, provider.Text, state, duration, outputEntries.ToList(), operationResultsList);
    }

    public ITestResult CreateIgnoredTestResult (ITestProvider provider)
    {
      return new TestResult(provider.Identity, provider.Text, State.Ignored, TimeSpan.Zero, new OutputEntry[0], new IOperationResult[0]);
    }

    public IOperationResult CreatePassedOperationResult (IOperationProvider provider)
    {
      return new OperationResult(
          provider.Identity,
          provider.Text,
          provider.Type,
          State.Passed,
          ExceptionDescriptor.None);
    }

    public IOperationResult CreateFailedOperationResult (IOperationProvider provider, Exception exception)
    {
      return new OperationResult(
          provider.Identity,
          provider.Text,
          provider.Type,
          State.Failed,
          ExceptionDescriptor.Create(exception));
    }

    public IOperationResult CreateInconclusiveOperationResult (IOperationProvider provider)
    {
      return new OperationResult(
          provider.Identity,
          provider.Text,
          provider.Type,
          State.Inconclusive,
          ExceptionDescriptor.None);
    }

    private IEnumerable<IResult> Convert<T> (IEnumerable<T> results) where T : IResult
    {
      return results.Cast<IResult>();
    }

    private State GetOverallState (IEnumerable<IResult> results)
    {
      return results.FirstOrDefault(x => x.State != State.Passed).GetValueOrDefault(x => x.State, () => State.Passed);
    }
  }
}