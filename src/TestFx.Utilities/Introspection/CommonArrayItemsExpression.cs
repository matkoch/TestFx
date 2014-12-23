using System;
using System.Collections.Generic;

namespace TestFx.Utilities.Introspection
{
  public class CommonArrayItemsExpression : CommonExpression
  {
    private readonly IEnumerable<CommonExpression> _items;

    public CommonArrayItemsExpression (IEnumerable<CommonExpression> items, CommonType type)
        : base(type)
    {
      _items = items;
    }

    public IEnumerable<CommonExpression> Items
    {
      get { return _items; }
    }
  }
}