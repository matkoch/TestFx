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
      if (element is ChildElementBase)
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

        var firstDisposition = first.GetDisposition();
        var secondDisposition = second.GetDisposition();
        if (firstDisposition == UnitTestElementDisposition.InvalidDisposition || secondDisposition == UnitTestElementDisposition.InvalidDisposition)
          return 0;

        return firstDisposition.Locations.First().NavigationRange.StartOffset
            .CompareTo(secondDisposition.Locations.First().NavigationRange.StartOffset);
      }
    }
  }
}