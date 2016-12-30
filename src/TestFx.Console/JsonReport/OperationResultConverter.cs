using System;
using System.Linq;
using Newtonsoft.Json;
using TestFx.Evaluation.Results;

namespace TestFx.Console.JsonReport
{
  public class OperationResultConverter : ConverterBase<IOperationResult>
  {
    public override void WriteJson (IOperationResult value, JsonWriter writer, JsonSerializer serializer)
    {
      Write("id", value.Identity.Absolute, writer, serializer);
      Write("state", value.State, writer, serializer);
      Write("text", value.Text, writer, serializer);
      Write("type", value.Type, writer, serializer);
      Write("exception", value.Exception, writer, serializer);
    }
  }
}