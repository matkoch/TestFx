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
using System.IO;
using System.Linq;
using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.Console.HtmlReport
{
  public class ResultConverter : JsonConverter
  {
    public override bool CanConvert ([NotNull] Type objectType)
    {
      return typeof(IResult).IsAssignableFrom(objectType);
    }

    public override void WriteJson ([NotNull] JsonWriter writer, [NotNull] object value, [NotNull] JsonSerializer serializer)
    {
      var result = (IResult) value;
      writer.WriteStartObject();

      writer.WritePropertyName("text");
      serializer.Serialize(writer, WebUtility.HtmlEncode(GetText(result.Identity)));

      writer.WritePropertyName("icon");
      serializer.Serialize(writer, GetIcon(result.State));

      var outputResult = value as IOutputResult;
      if (outputResult != null)
      {
        writer.WritePropertyName("output");
        serializer.Serialize(writer, WebUtility.HtmlEncode(outputResult.GetDetailedSummary()));
      }

      writer.WritePropertyName("nodes");
      var suiteResultHolder = value as ISuiteResultHolder;
      if (suiteResultHolder != null)
      {
        writer.WriteStartArray();

        suiteResultHolder.SuiteResults.ForEach(x => serializer.Serialize(writer, x));

        var suiteResult = value as ISuiteResult;
        if (suiteResult != null)
          suiteResult.TestResults.ForEach(x => serializer.Serialize(writer, x));

        writer.WriteEndArray();
      }


      writer.WriteEndObject();
    }

    private string GetText (IIdentity identity)
    {
      return File.Exists(identity.Relative)
        ? new FileInfo(identity.Relative).Name
        : identity.Relative.Substring(identity.Relative.LastIndexOf(value: '.') + 1);
    }

    private string GetIcon (State state)
    {
      switch (state)
      {
        case State.Passed:
          return "glyphicon glyphicon-ok green";
        case State.Failed:
          return "glyphicon glyphicon-remove red";
        case State.Ignored:
          return "glyphicon glyphicon-eye-close gray";
        case State.Inconclusive:
          return "glyphicon glyphicon-alert orange";
        default:
          throw new Exception("Unreachable");
      }
    }

    public override object ReadJson (
      [NotNull] JsonReader reader,
      [NotNull] Type objectType,
      [NotNull] object existingValue,
      [NotNull] JsonSerializer serializer)
    {
      throw new NotSupportedException();
    }
  }
}