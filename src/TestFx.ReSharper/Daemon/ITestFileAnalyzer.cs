using System;
using System.Collections.Generic;
using TestFx.ReSharper.Model.Tree;

namespace TestFx.ReSharper.Daemon
{
  public interface ITestFileAnalyzer
  {
    IEnumerable<INavigatableHighlighting> GetHighlightings(ITestFile file);
  }
}