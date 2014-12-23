using System;

namespace TestFx.Utilities.Introspection
{
  public abstract class CommonExpression
  {
    private readonly CommonType _type;

    protected CommonExpression (CommonType type)
    {
      _type = type;
    }

    public CommonType Type
    {
      get { return _type; }
    }
  }
}