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
using System.IO;
using System.Linq;
using System.Reflection;
using TestFx.Console.HtmlReport;
using TestFx.Console.TeamCity;
using TestFx.Evaluation;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.Console
{
  public partial class Program
  {
    private static void Main (string[] args)
    {
      InitializeOptions(args);

      if (Debug)
        Debugger.Launch();

      var assemblies = AssemblyPaths.Select(Assembly.LoadFrom);
      var appDomain = CreateAppDomain();
      var listeners = CreateListener(appDomain).ToArray();
      var result = Evaluator.Run(assemblies, listeners);
      AppDomain.Unload(appDomain);

      if (Pause)
      {
        System.Console.WriteLine("Press any key to terminate...");
        System.Console.ReadKey(intercept: true);
      }

      var exitCode = -1 * (int) result.State;
      Environment.Exit(exitCode);
    }

    private static AppDomain CreateAppDomain ()
    {
      var binPath1 = Path.GetDirectoryName(typeof(Program).Assembly.Location);
      var binPath2 = Path.GetDirectoryName(typeof(IRunListener).Assembly.Location);
      return AppDomain.CreateDomain("External", AppDomain.CurrentDomain.Evidence, new AppDomainSetup { ApplicationBase = binPath1, PrivateBinPath = binPath2 });
    }

    private static IEnumerable<IRunListener> CreateListener (AppDomain domain)
    {
      if (!string.IsNullOrWhiteSpace(HtmlReport))
        yield return domain.CreateProxy<Factory>(typeof(Factory)).Create<IRunListener>(typeof(HtmlReportRunListener), HtmlReport, Output);
      //  yield return domain.CreateProxy<IRunListener>(typeof(HtmlReportRunListener), HtmlReport, Output);
      //  yield return new HtmlReportRunListener(HtmlReport, Output);

      if (TeamCity)
        yield return new TeamCityRunListener(new TeamCityServiceMessageWriter(System.Console.WriteLine));
    }
  }
}