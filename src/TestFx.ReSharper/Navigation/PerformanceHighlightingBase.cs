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
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

namespace TestFx.ReSharper.Navigation
{
  public abstract class PerformanceHighlightingBase : IHighlightingWithRange
  {
    private readonly string myDescription;
    private readonly DocumentRange _range;

    protected PerformanceHighlightingBase (string format, string description, DocumentRange range)
    {
      _range = range;
      myDescription = string.Format(format, description);
    }

    public bool IsValid ()
    {
      return true;
    }

    public DocumentRange CalculateRange ()
    {
      return _range;
    }

    public string ToolTip
    {
      get { return myDescription; }
    }

    public string ErrorStripeToolTip
    {
      get { return ToolTip; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }
  }
}