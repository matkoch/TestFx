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
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace TestFx.Console.Tests
{
  [TestFixture]
  public class ConsoleTest
  {
    private const string c_speckTestAssembly = "TestFx.SpecK.Tests.dll";
    private const string c_mspecTestAssembly = "TestFx.MSpec.Tests.dll";

    private readonly Dictionary<string, string> _replacements =
        new Dictionary<string, string>
        {
            { "duration='(?<duration>[0-9]+)'", "duration='xx'" }
        };

    [Test]
    [TestCase ("gold01", new[] { c_speckTestAssembly })]
    [TestCase ("gold02", new[] { c_mspecTestAssembly })]
    [TestCase ("gold03", new[] { c_speckTestAssembly, c_mspecTestAssembly })]
    public void Test (string goldFile, string[] assemblies)
    {
      RunTest(goldFile, assemblies);
    }

    private void RunTest (string goldFile, string[] assemblies, string additionalArguments = null)
    {
      var output = GetOutput("--assemblies " + string.Join(";", assemblies) + " --teamCity " + additionalArguments);

      Compare(goldFile + ".standard.gold", output.StandardFileName);
      Compare(goldFile + ".error.gold", output.ErrorFileName);
    }

    private void Compare (string goldFileName, string actualFileName)
    {
      var copyBatch = GetRandomFile(extension: "cmd");
      var goldFileFullName = Path.Combine(Environment.CurrentDirectory, "..", "..", goldFileName);
      File.WriteAllText(copyBatch, string.Format("copy {0} {1}", actualFileName, goldFileFullName));
      var copyGoldText = "\r\n\r\nCopy gold: " + new Uri(copyBatch, UriKind.Absolute) + "\r\n\r\n";

      if (!File.Exists(goldFileFullName))
        throw new Exception("Gold file '" + goldFileName + "' does not exist." + copyGoldText);

      var goldText = File.ReadAllText(goldFileFullName);
      var actualText = File.ReadAllText(actualFileName);

      var diff = DiffFormatter.GetFormattedDiff(goldText, actualText);
      if (!string.IsNullOrEmpty(diff))
        throw new Exception("Gold file '" + goldFileName + "' differs from actual text." + copyGoldText + diff);
    }

    private OutputFiles GetOutput (string arguments)
    {
      var standardFile = GetRandomFile();
      var errorFile = GetRandomFile();
      using (var standardStream = new StreamWriter(File.Open(standardFile, FileMode.CreateNew)))
      using (var errorStream = new StreamWriter(File.Open(errorFile, FileMode.CreateNew)))
      {
        var processStartInfo = new ProcessStartInfo
                               {
                                   FileName = "TestFx.exe",
                                   Arguments = arguments,
                                   UseShellExecute = false,
                                   RedirectStandardOutput = true,
                                   RedirectStandardError = true,
                                   CreateNoWindow = true
                               };
        var process = new Process { StartInfo = processStartInfo };
        // ReSharper disable AccessToDisposedClosure
        process.OutputDataReceived += (s, e) => standardStream.WriteLine(GetPersistent(e.Data));
        process.ErrorDataReceived += (s, e) => errorStream.WriteLine(GetPersistent(e.Data));
        // ReSharper restore AccessToDisposedClosure
        process.EnableRaisingEvents = true;
        process.Start();
        process.BeginOutputReadLine();

        process.WaitForExit();
      }

      return new OutputFiles { StandardFileName = standardFile, ErrorFileName = errorFile };
    }

    private string GetRandomFile (string extension = null)
    {
      var fileName = Path.GetRandomFileName();
      if (extension != null)
        fileName = Path.ChangeExtension(fileName, extension);
      return Path.Combine(Path.GetTempPath(), fileName);
    }

    private string GetPersistent (string value)
    {
      return _replacements.Aggregate(value, (current, pair) => Regex.Replace(current, pair.Key, pair.Value, RegexOptions.Compiled));
    }

    public struct OutputFiles
    {
      public string StandardFileName;
      public string ErrorFileName;
    }
  }
}