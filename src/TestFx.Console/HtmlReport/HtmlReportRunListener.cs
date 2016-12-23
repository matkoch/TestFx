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
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.Utilities;

namespace TestFx.Console.HtmlReport
{
  public class HtmlReportRunListener : RunListener
  {
    private const string c_defaultTemplateName = "default-report.zip";

    private readonly ReportMode _reportMode;
    private readonly Browser _browser;
    private readonly string _output;

    public HtmlReportRunListener (ReportMode reportMode, Browser browser, string output)
    {
      _reportMode = reportMode;
      _browser = browser;
      _output = output;
    }

    public override void OnRunFinished (IRunResult result)
    {
      if (_reportMode == ReportMode.None)
        return;

      ExtractTemplate();
      GenerateReport(result);

      if (_reportMode != ReportMode.OpenAlways && (_reportMode != ReportMode.OpenOnFail || result.State != State.Passed))
        return;

      OpenReportInBrowser();
    }

    private void ExtractTemplate ()
    {
      var resourceName = typeof(HtmlReportRunListener).Namespace + "." + c_defaultTemplateName;
      var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
      var archive = new ZipArchive(stream.NotNull());
      archive.ExtractToDirectory(_output, overwrite: true);
    }

    private void GenerateReport (IRunResult result)
    {
      var content = JsonConvert.SerializeObject(result, Formatting.Indented, new ResultConverter());
      Directory.CreateDirectory(_output);
      File.WriteAllText(Path.Combine(_output, "report.json"), content);
    }

    private void OpenReportInBrowser ()
    {
      switch (_browser)
      {
        case Browser.Chrome:
          Process.Start(
              @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
              $@"--disable-web-security --user-data-dir=""C:\temp-chrome-testfx"" --app=""file:///{_output}""");
          break;

        default:
          throw new NotSupportedException();
      }
    }
  }
}