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
using System.Linq;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;

namespace TestFx.Console
{
  public class TeamCityListener : RunListener
  {
    private readonly TeamCityServiceMessageWriter _writer;

    public TeamCityListener (TeamCityServiceMessageWriter writer)
    {
      _writer = writer;
    }

    public override void OnSuiteStarted (IIntent intent)
    {
      _writer.WriteTestSuiteStarted(intent.Identity.Absolute);
    }

    public override void OnSuiteFinished (ISuiteResult result)
    {
      if (result.State == State.Failed && !result.SuiteResults.Any() && !result.TestResults.Any())
      {
        var operations = MergeSetupsAndCleanups(result).ToList();
        var exceptions = GetExceptions(operations).ToList();

        var message = GetGeneralMessage(exceptions, operations);
        var details = GetDetails(operations, result.OutputEntries);

        _writer.WriteTestFailed(result.Identity.Absolute, message, details);
      }

      _writer.WriteTestSuiteFinished(result.Identity.Absolute);
    }

    public override void OnTestFinished (ITestResult result)
    {
      var testName = result.Identity.Absolute;
      switch (result.State)
      {
        case State.Passed:
          _writer.WriteTestFinished(testName, result.Duration);
          break;
        case State.Failed:
          var operations = result.OperationResults.ToList();
          var exceptions = GetExceptions(operations).ToList();

          var message = GetGeneralMessage(exceptions, operations);
          var details = GetDetails(operations, result.OutputEntries, exceptions);

          _writer.WriteTestFailed(testName, message, details);
          break;
        case State.Ignored:
        case State.Inconclusive:
          _writer.WriteTestIgnored(testName, string.Empty);
          break;
      }
    }
  }
}