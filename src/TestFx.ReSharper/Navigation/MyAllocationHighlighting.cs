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
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl.DocumentMarkup;
using TestFx.ReSharper.Navigation;

[assembly: RegisterHighlighter(
  id: MyAllocationHighlighting.HIGHLIGHTING_ID,
  EffectColor = "Blue",
  EffectType = EffectType.SOLID_UNDERLINE,
  Layer = HighlighterLayer.SYNTAX,
  VSPriority = VSPriority.IDENTIFIERS)]

//[assembly: RegisterConfigurableSeverity (
//    MyAllocationHighlighting.SEVERITY_ID, null, AllocationHighlightingGroupIds.Configurable,
//    "Object allocation", "Highlights language construct or expression where object allocation happens",
//    Severity.HINT, solutionAnalysisRequired: false)]

namespace TestFx.ReSharper.Navigation
{
  //[ConfigurableSeverityHighlighting (
  //    SEVERITY_ID, CSharpLanguage.Name, AttributeId = HIGHLIGHTING_ID,
  //    ShowToolTipInStatusBar = false, ToolTipFormatString = MESSAGE)]
  [StaticSeverityHighlighting(
    severity: Severity.INFO,
    group: AllocationHighlightingGroupIds.Static,
    OverlapResolve = OverlapResolveKind.NONE,
    AttributeId = HIGHLIGHTING_ID,
    ShowToolTipInStatusBar = true)]
  public class MyAllocationHighlighting : PerformanceHighlightingBase
  {
    public const string HIGHLIGHTING_ID = "ReSharper Heap Allocation";
    public const string SEVERITY_ID = "HeapView.ObjectAllocation2";
    public const string MESSAGE = "Object allocation: {0}";

    public MyAllocationHighlighting (DocumentRange range, string description)
        : base(MESSAGE, description, range)
    {
    }
  }
}