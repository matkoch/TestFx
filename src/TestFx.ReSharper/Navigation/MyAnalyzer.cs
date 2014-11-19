// Copyright 2014, 2013 Matthias Koch
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
using System.Diagnostics;
using System.Linq;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using TestFx.ReSharper.Model.Tree.Aggregation;
using TestFx.ReSharper.Utilities.Psi.Tree;

namespace TestFx.ReSharper.Navigation
{
  [ElementProblemAnalyzer (
      elementTypes: new[] { typeof (IFile) },
      HighlightingTypes = new[] { typeof (MyAllocationHighlighting) })]
  public class MyAnalyzer : ElementProblemAnalyzer<IFile>
  {
    protected override void Run (IFile file, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
    {
      return;
      //IFile file = element.GetContainingFile();

      //consumer.AddHighlighting(new MyAllocationHighlighting(element.GetNavigationRange(), "bla"), file);
      //return;

      var suiteFile = file.ToSuiteFile();

      var suites = suiteFile.SuiteDeclarations.SelectMany(x => x.SuiteDeclarations).SelectMany(x => x.TestDeclarations).ToList();
      Debugger.Launch();
      var i = 0;
      foreach (var suite in suites)
      {
        i++;
        if (i % 2 == 0)
          continue;

        var unitTestElementLocation = suite.GetUnitTestElementLocation();
        var documentRange = unitTestElementLocation.ContainingRange.CreateDocumentRange(file.GetSourceFile().ToProjectFile());
        consumer.AddHighlighting(new MyAllocationHighlighting(documentRange, "test"), file);
      }
    }
  }
}