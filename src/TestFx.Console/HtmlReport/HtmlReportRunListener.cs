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
using RazorEngine;
using RazorEngine.Templating;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.Utilities;

namespace TestFx.Console.HtmlReport
{
  public class HtmlReportRunListener : RunListener
  {
    private const string c_defaultTemplateName = "default-report.zip";
    private const string c_templateKey = "templateKey";

    private readonly string _templateFile;
    private readonly string _reportFile;

    public HtmlReportRunListener (string htmlTemplate, string output)
    {
      try
      {
        var source = new ZipArchive(GetHtmlTemplateStream(htmlTemplate));
        source.ExtractToDirectory(output);
      }
      catch
      {
        // TODO: Maybe warn / delete files?
      }

      _templateFile = Path.Combine(output, "template.cshtml");
      _reportFile = Path.Combine(output, "index.html");
    }

    private static Stream GetHtmlTemplateStream (string htmlTemplate)
    {
      if (htmlTemplate == "default")
        return Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(HtmlReportRunListener).Namespace + ".default-report.zip").NotNull();

      return File.OpenRead(htmlTemplate);
    }

    public override void OnRunFinished (IRunResult result)
    {
      var template = File.ReadAllText(_templateFile);
      var reportContent = Engine.Razor.RunCompile(template, c_templateKey, typeof(IRunResult), result);
      File.WriteAllText(_reportFile, reportContent);
    }
  }
}