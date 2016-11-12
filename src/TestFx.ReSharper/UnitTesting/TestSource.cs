// Copyright 2016, 2015, 2014 Matthias Koch
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
using System.Threading;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Assemblies.AssemblyToAssemblyResolvers;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Exploration;
using JetBrains.Util;
using TestFx.ReSharper.UnitTesting.Explorers;

namespace TestFx.ReSharper.UnitTesting
{
  [SolutionComponent]
  internal class TestSource : UnitTestExplorerFrom.DotNetArtefacts, IUnitTestExplorerFromFile
  {
    private readonly ITestMetadataExplorer _testMetadataExplorer;
    private readonly ITestFileExplorer _testFileExplorer;
    private readonly ILogger _logger;

    public TestSource (
      ITestMetadataExplorer testMetadataExplorer,
      ITestFileExplorer testFileExplorer,
      ISolution solution,
      IUnitTestProvider provider,
      AssemblyToAssemblyReferencesResolveManager resolveManager,
      ILogger logger)
      : base(solution, provider, resolveManager, logger)
    {
      _testMetadataExplorer = testMetadataExplorer;
      _testFileExplorer = testFileExplorer;
      _logger = logger;
    }

    public override void ProcessProject (
      [NotNull] IProject project,
      [NotNull] FileSystemPath assemblyPath,
      [NotNull] MetadataLoader loader,
      [NotNull] IUnitTestElementsObserver observer,
      CancellationToken token)
    {
      MetadataElementsSource.ExploreProject(
        project,
        assemblyPath,
        loader,
        observer,
        _logger,
        token,
        metadataAssembly => _testMetadataExplorer.Explore(project, metadataAssembly, observer, token));
      observer.OnCompleted();
    }

    public void ProcessFile (IFile psiFile, IUnitTestElementsObserver observer, Func<bool> interrupted)
    {
      _testFileExplorer.Explore(
          psiFile,
          x =>
          {
            // TODO: ask Jenya
            observer.OnUnitTestElementDisposition(x);
            observer.OnUnitTestElementChanged(x.UnitTestElement);
          },
          () => !interrupted());
      observer.OnCompleted();
    }
  }
}