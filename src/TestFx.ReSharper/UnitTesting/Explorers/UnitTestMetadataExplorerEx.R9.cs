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
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.ReSharper.UnitTesting.Elements;

namespace TestFx.ReSharper.UnitTesting.Explorers
{
  public partial interface IUnitTestMetadataExplorerEx
  {
    void Explore (
        IProject project,
        IMetadataAssembly assembly,
        IUnitTestElementsObserver observer);
  }

  [SolutionComponent]
  public partial class UnitTestMetadataExplorerEx
  {
    public UnitTestMetadataExplorerEx (IUnitTestElementFactoryEx unitTestElementFactory)
    {
      _unitTestElementFactory = unitTestElementFactory;
    }

    public void Explore (IProject project, IMetadataAssembly assembly, IUnitTestElementsObserver observer)
    {
      Explore(
          project,
          assembly,
          x =>
          {
            observer.OnUnitTestElement(x);
            observer.OnUnitTestElementChanged(x);
          },
          () => true);
    }
  }
}