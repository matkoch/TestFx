// Copyright 2014, 2013 Matthias Koch
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
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.ReSharper.UnitTesting.Elements;

namespace TestFx.ReSharper.UnitTesting.Explorers
{
  public partial interface IUnitTestMetadataExplorerEx : IUnitTestMetadataExplorer
  {
  }

  [MetadataUnitTestExplorer]
  public partial class UnitTestMetadataExplorerEx
  {
    private readonly IUnitTestProviderEx _unitTestProvider;

    public UnitTestMetadataExplorerEx (IUnitTestProviderEx unitTestProvider, IUnitTestElementFactoryEx unitTestElementFactory)
    {
      _unitTestProvider = unitTestProvider;
      _unitTestElementFactory = unitTestElementFactory;
    }

    public void ExploreAssembly (IProject project, IMetadataAssembly assembly, UnitTestElementConsumer consumer, ManualResetEvent exitEvent)
    {
      Explore(project, assembly, x => consumer(x), () => !exitEvent.WaitOne(0));
    }

    public IUnitTestProvider Provider
    {
      get { return _unitTestProvider; }
    }
  }
}