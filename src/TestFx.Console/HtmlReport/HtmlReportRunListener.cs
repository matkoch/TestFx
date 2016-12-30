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
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using TestFx.Console.JsonReport;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.Utilities;

namespace TestFx.Console.HtmlReport
{
  public class HtmlReportRunListener : RunListener
  {
    private const string c_defaultTemplateName = "default-report.zip";

    private readonly string _output;

    public HtmlReportRunListener (string output)
    {
      _output = output;
    }

    public override void OnRunFinished (IRunResult result)
    {
      var resourceName = typeof(HtmlReportRunListener).Namespace + "." + c_defaultTemplateName;
      var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
      var archive = new ZipArchive(stream.NotNull());
      archive.ExtractToDirectory(_output, overwrite: true);
    }
  }
}