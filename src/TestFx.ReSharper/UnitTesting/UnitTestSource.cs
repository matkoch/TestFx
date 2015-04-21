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
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Util;
using TestFx.ReSharper.UnitTesting.Explorers;

namespace TestFx.ReSharper.UnitTesting
{
  [SolutionComponent]
  public class UnitTestSource : IUnitTestElementsSource
  {
    private readonly IUnitTestMetadataExplorerEx _unitTestMetadataExplorer;
    private readonly IUnitTestFileExplorerEx _unitTestFileExplorer;
    private readonly IMetadataElementsSourceEx _metadataElementsSource;
    private readonly IUnitTestProviderEx _unitTestProvider;

    public UnitTestSource (
        IUnitTestMetadataExplorerEx unitTestMetadataExplorer,
        IUnitTestFileExplorerEx unitTestFileExplorer,
        IMetadataElementsSourceEx metadataElementsSource,
        IUnitTestProviderEx unitTestProvider)
    {
      _unitTestMetadataExplorer = unitTestMetadataExplorer;
      _unitTestFileExplorer = unitTestFileExplorer;
      _metadataElementsSource = metadataElementsSource;
      _unitTestProvider = unitTestProvider;
    }

    public void ExploreSolution (IUnitTestElementsObserver observer)
    {
    }

    public void ExploreProjects (IDictionary<IProject, FileSystemPath> projects, MetadataLoader loader, IUnitTestElementsObserver observer)
    {
      _metadataElementsSource.ExploreProjects(projects, loader, observer, _unitTestMetadataExplorer.Explore);
      observer.OnCompleted();
    }

    public void ExploreFile (IFile psiFile, IUnitTestElementsObserver observer, Func<bool> interrupted)
    {
      _unitTestFileExplorer.Explore(psiFile, observer.OnUnitTestElementDisposition, () => !interrupted());
      observer.OnCompleted();
    }

    public IUnitTestProvider Provider
    {
      get { return _unitTestProvider; }
    }
  }
}