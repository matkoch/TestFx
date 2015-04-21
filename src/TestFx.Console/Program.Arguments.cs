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