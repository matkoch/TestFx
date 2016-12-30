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
using TestFx.Console.JsonReport;
using TestFx.Console.TeamCity;
using TestFx.Evaluation;
using TestFx.Evaluation.Reporting;

namespace TestFx.Console
{
  public class Program
  {
    private static void Main (string[] args)
    {
      Options.Load(args);

      if (Options.ShowLogo)
      {
        System.Console.WriteLine(@"___________                __  ___________        ");
        System.Console.WriteLine(@"\__    ___/____    _______/  |_\_   _____/___  ___");
        System.Console.WriteLine(@"  |    | _/ __ \  /  ___/\   __\|    __)  \  \/  /");
        System.Console.WriteLine(@"  |    | \  ___/  \___ \  |  |  |     \    >    < ");
        System.Console.WriteLine(@"  |____|  \___  >/____  > |__|  \___  /   /__/\_ \");
        System.Console.WriteLine(@"              \/      \/            \/          \/");
      }

      if (Options.Debug)
        Debugger.Launch();

      var assemblies = Options.AssemblyPaths.Select(Assembly.LoadFrom);
      var listeners = CreateListener().ToArray();
      var result = Evaluator.Run(assemblies, listeners);

      if (Options.Pause)
      {
        System.Console.WriteLine("Press any key to terminate...");
        System.Console.ReadKey(intercept: true);
      }

      var exitCode = -1 * (int) result.State;
      Environment.Exit(exitCode);
    }

    private static IEnumerable<IRunListener> CreateListener ()
    {
      if (Options.JsonReport)
        yield return new JsonReportRunListener(Options.Output);

      if (Options.HtmlReport)
        yield return new HtmlReportRunListener(Options.Output);

      if (Options.RunnerEnvironment == RunnerEnvironment.TeamCity)
        yield return new TeamCityRunListener(new TeamCityServiceMessageWriter(System.Console.WriteLine));
    }
  }
}