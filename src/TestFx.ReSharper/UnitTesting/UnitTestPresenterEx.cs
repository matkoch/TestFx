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
using JetBrains.CommonControls;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.TreeModels;
using JetBrains.UI.TreeView;
using TestFx.ReSharper.UnitTesting.Elements;

namespace TestFx.ReSharper.UnitTesting
{
  [UnitTestPresenter]
  public class UnitTestPresenterEx : IUnitTestPresenter
  {
    private readonly IComparer<TreeModelNode> _comparer = new NodeComparer();

    public void Present (IUnitTestElement element, IPresentableItem item, TreeModelNode node, PresentationState state)
    {
      if (element is TestElement)
        node.Model.Comparer = _comparer;
    }

    private class NodeComparer : IComparer<TreeModelNode>
    {
      public int Compare (TreeModelNode x, TreeModelNode y)
      {
        var first = x.DataValue as IUnitTestElementEx;
        var second = y.DataValue as IUnitTestElementEx;
        if (first == null || second == null)
          return 0;

        // TODO: Performance critical. should cache test file
        var firstLocation = first.GetDisposition().Locations.SingleOrDefault();
        var secondLocation = second.GetDisposition().Locations.SingleOrDefault();
        if (firstLocation == null || secondLocation == null)
          return 0;

        return firstLocation.NavigationRange.StartOffset
            .CompareTo(secondLocation.NavigationRange.StartOffset);
      }
    }
  }
}