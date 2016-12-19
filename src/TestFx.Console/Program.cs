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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TestFx.Console.HtmlReport;
using TestFx.Console.TeamCity;
using TestFx.Evaluation;
using TestFx.Evaluation.Reporting;

namespace TestFx.Console
{
  public class Program
  {
    private static Options s_options;

    private static IEnumerable<string> AssemblyPaths => s_options.Assemblies ?? new List<string>();

    private static bool Pause => s_options.Pause;

    private static bool Debug => s_options.Debug;

    private static bool TeamCity => s_options.TeamCity || Environment.GetEnvironmentVariable("TEAMCITY_VERSION") != null;

    private static ReportMode ReportMode => s_options.ReportMode;

    private static Browser Browser => s_options.Browser;

    private static string Output => s_options.Output ?? Environment.CurrentDirectory;

    private static bool ShowLogo => !s_options.NoLogo;

    private static void Main (string[] args)
    {
      s_options = Options.Load(args);

      if (ShowLogo)
      {
        System.Console.WriteLine(@" ____  ____  ____  ____  ____  _  _ ");
        System.Console.WriteLine(@"(_  _)(  __)/ ___)(_  _)(  __)( \/ )");
        System.Console.WriteLine(@"  )(   ) _) \___ \  )(   ) _)  )  ( ");
        System.Console.WriteLine(@" (__) (____)(____/ (__) (__)  (_/\_)");
        System.Console.WriteLine();
      }

      if (Debug)
        Debugger.Launch();

      var assemblies = AssemblyPaths.Select(Assembly.LoadFrom);
      var listeners = CreateListener().ToArray();
      var result = Evaluator.Run(assemblies, listeners);

      if (Pause)
      {
        System.Console.WriteLine("Press any key to terminate...");
        System.Console.ReadKey(intercept: true);
      }

      var exitCode = -1 * (int) result.State;
      Environment.Exit(exitCode);
    }

    private static IEnumerable<IRunListener> CreateListener ()
    {
      yield return new HtmlReportRunListener(ReportMode, Browser, Output);

      if (TeamCity)
        yield return new TeamCityRunListener(new TeamCityServiceMessageWriter(System.Console.WriteLine));
    }
  }
}