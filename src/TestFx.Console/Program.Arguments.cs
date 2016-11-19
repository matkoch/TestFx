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
using System.Collections.Generic;
using CommandLine;
using JetBrains.Annotations;
using TestFx.Console.HtmlReport;

namespace TestFx.Console
{
  public static partial class Program
  {
    private class Options
    {
      [OptionList ("assemblies", Required = true, Separator = ';', HelpText = "List of assemblies separated by semicolons.")]
      public IList<string> Assemblies { get; [UsedImplicitly] set; }

      [Option ("pause", HelpText = "Enables pausing before the process is terminated.")]
      public bool Pause { get; [UsedImplicitly] set; }

      [Option ("debug",
          HelpText = "Enables debugging by calling Debugger.Launch().")]
      public bool Debug { get; [UsedImplicitly] set; }

      [Option ("teamCity", HelpText = "Forces output for JetBrains TeamCity server. Disables standard output.")]
      public bool TeamCity { get; [UsedImplicitly] set; }
      
      [Option("reportMode", HelpText = "Specifies the HTML report mode. Allowed options are: None, Silent, OpenOnFail, OpenAlways.")]
      public ReportMode ReportMode { get; [UsedImplicitly] set; }

      [Option("brower", HelpText = "Specifies the browser that is used to show the report. Allowed options: Chrome.")]
      public Browser Browser { get; [UsedImplicitly] set; }

      [Option("output", HelpText = "Specifies the output directory for the HTML report and DotCover analysis.")]
      public string Output { get; [UsedImplicitly] set; }
    }

    private static Options s_options;

    private static void InitializeOptions (string[] args)
    {
      s_options = new Options();
      var parser = new Parser(
          x =>
          {
            x.MutuallyExclusive = true;
            x.HelpWriter = System.Console.Error;
          });
      parser.ParseArgumentsStrict(args, s_options, () => System.Console.ReadKey());
    }

    public static IEnumerable<string> AssemblyPaths => s_options.Assemblies ?? new List<string>();

    public static bool Pause => s_options.Pause;

    public static bool Debug => s_options.Debug;

    public static bool TeamCity => s_options.TeamCity || Environment.GetEnvironmentVariable("TEAMCITY_VERSION") != null;

    public static ReportMode ReportMode => s_options.ReportMode;

    public static Browser Browser => s_options.Browser;

    public static string Output => s_options.Output ?? Environment.CurrentDirectory;
  }
}