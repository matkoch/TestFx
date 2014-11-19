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
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.ReSharper.UnitTesting.Elements;

namespace TestFx.ReSharper.UnitTesting.Explorers
{
  public partial interface IUnitTestFileExplorerEx : IUnitTestFileExplorer
  {
  }

  [FileUnitTestExplorer]
  public partial class UnitTestFileExplorerEx
  {
    private readonly IUnitTestProviderEx _unitTestProvider;

    public UnitTestFileExplorerEx (IUnitTestProviderEx unitTestProvider, IUnitTestElementFactoryEx unitTestElementFactory)
    {
      _unitTestProvider = unitTestProvider;
      _unitTestElementFactory = unitTestElementFactory;
    }

    public void ExploreFile (IFile psiFile, UnitTestElementLocationConsumer consumer, Func<bool> interrupted)
    {
      Explore(psiFile, x => consumer(x), () => !interrupted());
    }

    public IUnitTestProvider Provider
    {
      get { return _unitTestProvider; }
    }
  }
}