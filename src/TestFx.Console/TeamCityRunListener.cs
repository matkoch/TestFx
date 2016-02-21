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
  internal class TeamCityRunListener : RunListener
  {
    private readonly TeamCityServiceMessageWriter _writer;

    public TeamCityRunListener (TeamCityServiceMessageWriter writer)
        : base(new TextSymbolProvider())
    {
      _writer = writer;
    }

    public override void OnSuiteStarted (IIntent intent, string text)
    {
      _writer.WriteTestSuiteStarted(text);
    }

    public override void OnSuiteFinished (ISuiteResult result)
    {
      if (result.State == State.Failed && !result.SuiteResults.Any() && !result.TestResults.Any())
      {
        var operations = MergeSetupsAndCleanups(result).ToList();
        var exceptions = GetExceptions(operations).ToList();

        var message = GetGeneralMessage(exceptions, operations);
        var details = GetDetails(operations, result.OutputEntries);

        _writer.WriteTestFailed(result.Text, message, details);
      }

      _writer.WriteTestSuiteFinished(result.Text);
    }

    public override void OnTestStarted (IIntent intent, string text)
    {
      _writer.WriteTestStarted(text, captureStandardOutput: false);
    }

    public override void OnTestFinished (ITestResult result)
    {
      switch (result.State)
      {
        case State.Passed:
          _writer.WriteTestFinished(result.Text, result.Duration);
          break;
        case State.Failed:
          var operations = result.OperationResults.ToList();
          var exceptions = GetExceptions(operations).ToList();

          var message = GetGeneralMessage(exceptions, operations);
          var details = GetDetails(operations, result.OutputEntries, exceptions);

          _writer.WriteTestFailed(result.Text, message, details);
          _writer.WriteTestFinished(result.Text, result.Duration);
          break;
        case State.Ignored:
        case State.Inconclusive:
          _writer.WriteTestIgnored(result.Text, string.Empty);
          break;
      }
    }
  }
}