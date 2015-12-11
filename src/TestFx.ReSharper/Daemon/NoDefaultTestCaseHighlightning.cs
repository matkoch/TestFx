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
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using TestFx.ReSharper.Daemon;
using TestFx.ReSharper.Model.Tree;
using TestFx.Utilities.Collections;

[assembly: RegisterConfigurableSeverity (
    NoDefaultTestCaseHighlightning.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    NoDefaultTestCaseHighlightning.Message,
    NoDefaultTestCaseHighlightning.Description,
    Severity.WARNING,
    /*SolutionAnalysisRequired:*/ false)]

namespace TestFx.ReSharper.Daemon
{
  [ConfigurableSeverityHighlighting (
      SeverityId,
      "CSHARP",
      OverlapResolve = OverlapResolveKind.WARNING,
      ToolTipFormatString = Message)]
  public class NoDefaultTestCaseHighlightning : SimpleTreeNodeHighlightingBase
  {
    public const string SeverityId = "NoDefaultTest";
    public const string Message = "Test suite has no default test case.";

    public const string Description =
        "Warns about a test suite not having a default test case, which is supposed to be best practise.";

    public NoDefaultTestCaseHighlightning (ITreeNode treeNode)
        : base(treeNode, Message)
    {
    }
  }

  [PsiComponent]
  public class NoDefaultTestCaseTestFileAnalyzer : ITestFileAnalyzer
  {
    public IEnumerable<INavigatableHighlighting> GetHighlightings(ITestFile file)
    {
      foreach (var classDeclaration in file.TestDeclarations)
      {
        var hasDefault = classDeclaration.DescendantsAndSelf(x => x.TestDeclarations).Any(x => x.Text == "<Default>");
        if (hasDefault)
          continue;

        classDeclaration.AssertIsValid();
        yield return new NoDefaultTestCaseHighlightning(classDeclaration);
      }
    }
  }
}