using System;
using System.Collections.Generic;
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

    public override void OnSuiteStarted (ISuiteIntent intent)
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
          _writer.WriteTestFinished(testName, TimeSpan.Zero);
          break;
        case State.Failed:
          var operations = result.OperationResults.ToList();
          var exceptions = GetExceptions(operations).ToList();

          var message = GetGeneralMessage(exceptions, operations);
          var details = GetDetails(operations, result.OutputEntries, exceptions);

          _writer.WriteTestFailed(testName, message, details);
          break;
        case State.NotImplemented:
          _writer.WriteTestIgnored(testName, string.Empty);
          break;
        case State.Ignored:
          _writer.WriteTestIgnored(testName, string.Empty);
          break;
      }
    }
  }
}