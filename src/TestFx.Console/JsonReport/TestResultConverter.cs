using System;
using System.Linq;
using Newtonsoft.Json;
using TestFx.Evaluation.Results;

namespace TestFx.Console.JsonReport
{
  public class TestResultConverter : ConverterBase<ITestResult>
  {
    public override void WriteJson (ITestResult value, JsonWriter writer, JsonSerializer serializer)
    {
      Write("id", value.Identity.Absolute, writer, serializer);
      Write("state", value.State, writer, serializer);
      Write("text", value.Text, writer, serializer);
      Write("duration", value.Duration, writer, serializer);
      Write("output", value.OutputEntries, writer, serializer);
      Write("operations", value.OperationResults, writer, serializer);
    }
  }
}