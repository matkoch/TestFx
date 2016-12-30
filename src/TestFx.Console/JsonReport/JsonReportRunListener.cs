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
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.Utilities;

namespace TestFx.Console.JsonReport
{
  public class JsonReportRunListener : RunListener
  {
    private readonly string _output;

    public JsonReportRunListener (string output)
    {
      _output = output;
    }

    public override void OnRunFinished (IRunResult result)
    {
      var settings =
          new JsonSerializerSettings
          {
            ContractResolver = new LowercaseContractResolver(),
            Converters =
                new JsonConverter[]
                {
                  new StringEnumConverter(),
                  new IdentityConverter(),
                  new HtmlStringConverter()
                }
          };
      var content = JsonConvert.SerializeObject(result, Formatting.Indented, settings);
      Directory.CreateDirectory(_output);
      File.WriteAllText(Path.Combine(_output, "resultData.json"), content);
    }
  }

  public class IdentityConverter : ConverterBase<IIdentity>
  {
    public override void WriteJson (IIdentity value, JsonWriter writer, JsonSerializer serializer)
    {
      serializer.Serialize(writer, value.Absolute);
    }
  }

  public class HtmlStringConverter : ConverterBase<string>
  {
    public override void WriteJson (string value, JsonWriter writer, JsonSerializer serializer)
    {
      writer.WriteValue(WebUtility.HtmlEncode(value));
    }
  }

  public class LowercaseContractResolver : DefaultContractResolver
  {
    protected override string ResolvePropertyName ([NotNull] string propertyName)
    {
      return propertyName.ToLower();
    }

    protected override JsonProperty CreateProperty ([NotNull] MemberInfo member, MemberSerialization memberSerialization)
    {
      var property = base.CreateProperty(member, memberSerialization);
      if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType)
          && property.PropertyType != typeof(string))
        property.ShouldSerialize = x =>
        {
          var propertyInfo = member as PropertyInfo;
          if (propertyInfo == null)
            return true;

          var collection = propertyInfo.GetValue(x) as ICollection;
          return collection == null || collection.Count > 0;
        };

      return property;
    }
  }
}