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
using JetBrains.ReSharper.Feature.Services.Daemon;
using TestFx.ReSharper.Daemon;
using TestFx.ReSharper.Model.Tree;

[assembly: RegisterConfigurableSeverity (
    DuplicatedTestHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CompilerWarnings,
    DuplicatedTestHighlighting.Message,
    DuplicatedTestHighlighting.Description,
    Severity.ERROR,
    /*SolutionAnalysisRequired:*/ false)]

namespace TestFx.ReSharper.Daemon
{
  [ConfigurableSeverityHighlighting (
      SeverityId,
      "CSHARP",
      OverlapResolve = OverlapResolveKind.ERROR,
      ToolTipFormatString = Message)]
  internal class DuplicatedTestHighlighting : SimpleTestDeclarationHighlightingBase
  {
    public const string SeverityId = "DuplicatedTest";
    public const string Message = "Test is duplicated.";

    public const string Description = "Warns about a test that is duplicated.";

    public DuplicatedTestHighlighting(ITestDeclaration treeNode)
        : base(treeNode, Message)
    {
    }
  }
}