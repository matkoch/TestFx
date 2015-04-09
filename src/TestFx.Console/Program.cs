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
