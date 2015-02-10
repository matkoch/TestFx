using System;
using System.Collections.Generic;
using CommandLine;

namespace TestFx.Console
{
  public static partial class Program
  {
    private class Options
    {
      [OptionList ("assemblies", Required = true, Separator = ';', HelpText = "List of assemblies separated by semicolons.")]
      public IList<string> Assemblies { get; set; }

      [Option ("pause", HelpText = "Enables pausing before the process is terminated.")]
      public bool Pause { get; set; }

      [Option ("debug",
          HelpText = "Enables debugging by calling Debugger.Launch().")]
      public bool Debug { get; set; }

      [Option ("teamCity", HelpText = "Enables output for JetBrains TeamCity server (auto-detected). Disables standard output.")]
      public bool TeamCity { get; set; }
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
      parser.ParseArgumentsStrict(args, s_options);
    }

    public static IEnumerable<string> AssemblyPaths
    {
      get { return s_options.Assemblies ?? new List<string>(); }
    }

    public static bool Pause
    {
      get { return s_options.Pause; }
    }

    public static bool Debug
    {
      get { return s_options.Debug; }
    }

    public static bool TeamCity
    {
      get { return s_options.TeamCity || Environment.GetEnvironmentVariable("TEAMCITY_VERSION") != null; }
    }
  }
}
