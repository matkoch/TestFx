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
using Newtonsoft.Json;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;

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
      //var settings =
      //    new JsonSerializerSettings
      //    {
      //      ContractResolver = new CustomContractResolver(),
      //      Converters =
      //          new JsonConverter[]
      //          {
      //            new StringEnumConverter(),
      //            new IdentityConverter(),
      //            new HtmlStringConverter()
      //          }
      //    };
      var content = JsonConvert.SerializeObject(result, Formatting.Indented,
        new RunResultConverter(),
        new SuiteResultConverter(),
        new TestResultConverter(),
        new OperationResultConverter(),
        new ExceptionDescriptorConverter());
      Directory.CreateDirectory(_output);
      File.WriteAllText(Path.Combine(_output, "resultData.json"), content);
    }
  }
}