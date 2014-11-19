using System;
using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace TestFx.ReSharper.UnitTesting
{
  public interface IMetadataElementsSourceEx
  {
    void ExploreProjects (
        IDictionary<IProject, FileSystemPath> projects,
        MetadataLoader loader,
        IUnitTestElementsObserver observer,
        Action<IProject, IMetadataAssembly, IUnitTestElementsObserver> exploreAssembly);
  }

  [SolutionComponent]
  public class MetadataElementsSourceEx : MetadataElementsSource, IMetadataElementsSourceEx
  {
    public MetadataElementsSourceEx (IShellLocks shellLocks)
        : base(Logger.GetLogger(typeof (MetadataElementsSourceEx)), shellLocks)
    {
    }
  }
}