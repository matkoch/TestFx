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

namespace TestFx.Console
{
  public class Options
  {
    private static readonly Arguments s_options = new Arguments();

    public static void Load(string[] args)
    {
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

    public static RunnerEnvironment RunnerEnvironment
    {
      get
      {
        var environment = s_options.RunnerEnvironment;
        if (environment != RunnerEnvironment.Automatic)
          return environment;

        if (Environment.GetEnvironmentVariable("TEAMCITY_VERSION") != null)
          return RunnerEnvironment.TeamCity;

        return RunnerEnvironment.Local;
      }
    }

    public static bool HtmlReport => s_options.HtmlReport;

    public static bool JsonReport => s_options.JsonReport || s_options.HtmlReport;

    public static string Output => s_options.Output ?? Environment.CurrentDirectory;

    public static bool ShowLogo => !s_options.NoLogo;

    public class Arguments
    {
      [OptionList ("assemblies", Required = true, Separator = ';', HelpText = "List of assemblies separated by semicolons.")]
      public IList<string> Assemblies { get; [UsedImplicitly] set; }

      [Option ("pause", HelpText = "Enables pausing before the process is terminated.")]
      public bool Pause { get; [UsedImplicitly] set; }

      [Option ("debug", HelpText = "Enables debugging by calling Debugger.Launch().")]
      public bool Debug { get; [UsedImplicitly] set; }

      [Option ("environment", HelpText = "Specifies the runner environment. Allowed options are: Automatic, Local, TeamCity.")]
      public RunnerEnvironment RunnerEnvironment { get; [UsedImplicitly] set; }

      [Option ("html", HelpText = "Specifies that a HTML report should be generated.")]
      public bool HtmlReport { get; [UsedImplicitly] set; }

      [Option ("json", HelpText = "Specifies that a JSON report should be generated.")]
      public bool JsonReport { get; [UsedImplicitly] set; }

      [CanBeNull]
      [Option ("output", HelpText = "Specifies the output directory for the HTML report and DotCover analysis.")]
      public string Output { get; [UsedImplicitly] set; }

      [Option ("nologo", HelpText = "Suppresses display of logo text.")]
      public bool NoLogo { get; [UsedImplicitly] set; }
    }
  }
}