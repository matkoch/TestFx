using System;
using System.Linq;
using Newtonsoft.Json;
using TestFx.Evaluation.Results;

namespace TestFx.Console.JsonReport
{
  public class SuiteResultConverter : ConverterBase<ISuiteResult>
  {
    public override void WriteJson (ISuiteResult value, JsonWriter writer, JsonSerializer serializer)
    {
      Write("id", value.Identity.Absolute, writer, serializer);
      Write("state", value.State, writer, serializer);
      Write("text", value.Text, writer, serializer);
      Write("output", value.OutputEntries, writer, serializer);
      Write("setups", value.SetupResults, writer, serializer);
      Write("cleanups", value.CleanupResults, writer, serializer);
      Write("suites", value.SuiteResults, writer, serializer);
      Write("tests", value.TestResults, writer, serializer);
    }
  }
}