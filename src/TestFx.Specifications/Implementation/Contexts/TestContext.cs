using System;
using TestFx.Extensibility.Contexts;
using TestFx.Specifications.InferredApi;

namespace TestFx.Specifications.Implementation.Contexts
{
  public abstract class TestContext<TSubject, TResult, TVars> : TestContext, ITestContext<TSubject, TResult, TVars>
  {
    public abstract TSubject Subject { get; set; }
    public abstract TResult Result { get; set; }
    public abstract TVars Vars { get; set; }
    public abstract Exception Exception { get; set; }
    public abstract TimeSpan Duration { get; set; }
    public abstract bool ExpectsException { get; set; }
  }
}