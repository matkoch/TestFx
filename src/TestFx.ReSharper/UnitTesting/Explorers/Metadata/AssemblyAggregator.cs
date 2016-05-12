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
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using TestFx.ReSharper.Model.Metadata;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.UnitTesting.Explorers.Metadata
{
  public interface IAssemblyAggregator
  {
    [CanBeNull]
    ITestAssembly Aggregate (IMetadataAssembly metadataAssembly, IProject project, [CanBeNull] Func<bool> notInterrupted);
  }

  [PsiComponent]
  public class AssemblyAggregator : IAssemblyAggregator
  {
    private readonly List<ITestMetadataProviderFactory> _testMetadataProviderFactories;

    public AssemblyAggregator (IEnumerable<ITestMetadataProviderFactory> testMetadataProviderFactories)
    {
      _testMetadataProviderFactories = testMetadataProviderFactories.ToList();
    }

    #region IAssemblyAggregator

    [CanBeNull]
    public ITestAssembly Aggregate (IMetadataAssembly metadataAssembly, IProject project, [CanBeNull] Func<bool> notInterrupted)
    {
      notInterrupted = notInterrupted ?? (() => true);

      var assemblyIdentity = new Identity(project.GetOutputFilePath().FullPath);
      var testMetadataProviders = _testMetadataProviderFactories.Select(x => x.CreateTestMetadataProvider(assemblyIdentity, project, notInterrupted)).ToList();
      var metadataTypeInfos = metadataAssembly.GetTypes();
      var testMetadata = GetTestMetadata(testMetadataProviders, metadataTypeInfos).TakeWhile(notInterrupted).WhereNotNull().ToList();

      if (testMetadata.Count == 0)
        return null;

      return new TestAssembly(testMetadata, metadataAssembly);
    }

    #endregion

    #region Privates

    private IEnumerable<ITestMetadata> GetTestMetadata (
        IList<ITestMetadataProvider> testMetadataProviders,
        IList<IMetadataTypeInfo> metadataTypeInfos)
    {
      return
          from testMetadataProvider in testMetadataProviders
          from metadataTypeInfo in metadataTypeInfos
          select testMetadataProvider.GetTestMetadata(metadataTypeInfo);
    }

    #endregion
  }
}