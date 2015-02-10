using System;
using System.Linq;
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

    public override void OnTestFinished (ITestResult result)
    {
      var testName = result.Identity.Absolute;
      switch (result.State)
      {
        case State.Passed:
          _writer.WriteTestFinished(testName, TimeSpan.Zero);
          break;
        case State.Failed:
          var exception = result.OperationResults.Select(x => x.Exception).First(x => x != null);
          _writer.WriteTestFailed(testName, exception.TypeFullName + ": " + exception.Message, exception.StackTrace);
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