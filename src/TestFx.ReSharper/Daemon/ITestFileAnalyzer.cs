using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.Daemon;
using TestFx.ReSharper.Model.Tree;

namespace TestFx.ReSharper.Daemon
{
  public interface ITestFileAnalyzer
  {
    IEnumerable<IHighlighting> GetHighlightings(ITestFile file);
  }
}