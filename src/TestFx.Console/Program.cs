// Copyright 2015, 2014 Matthias Koch
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
using TestFx.Evaluation;
using TestFx.Evaluation.Reporting;

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
      var listeners = CreateListener().ToArray();
      var result = Evaluator.Run(assemblies, listeners);

      if (Pause)
      {
        System.Console.WriteLine("Press <enter> to terminate...");
        System.Console.Read();
      }

      var exitCode = -1 * (int) result.State;
      Environment.Exit(exitCode);
    }

    private static IEnumerable<IRunListener> CreateListener ()
    {
      if (TeamCity)
        yield return new TeamCityListener(new TeamCityServiceMessageWriter(System.Console.WriteLine));
    }
  }
}