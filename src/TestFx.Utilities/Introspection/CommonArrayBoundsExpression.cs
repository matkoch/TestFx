using System;

namespace TestFx.Utilities.Introspection
{
  public class CommonArrayBoundsExpression : CommonExpression
  {
    private readonly int[] _ranks;

    public CommonArrayBoundsExpression (int[] ranks, CommonType type)
        : base(type)
    {
      _ranks = ranks;
    }

    public int[] Ranks
    {
      get { return _ranks; }
    }
  }
}