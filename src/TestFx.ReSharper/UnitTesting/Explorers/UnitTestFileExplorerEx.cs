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
using System.Linq;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.ReSharper.Model.Tree.Aggregation;
using TestFx.ReSharper.UnitTesting.Elements;
using TestFx.Utilities;

namespace TestFx.ReSharper.UnitTesting.Explorers
{
  public partial interface IUnitTestFileExplorerEx
  {
    void Explore (IFile psiFile, Action<UnitTestElementDisposition> consumer, Func<bool> notInterrupted);
  }

  public partial class UnitTestFileExplorerEx : IUnitTestFileExplorerEx
  {
    private readonly IUnitTestElementFactoryEx _unitTestElementFactory;

    public void Explore (IFile psiFile, Action<UnitTestElementDisposition> consumer, Func<bool> notInterrupted)
    {
      var file = psiFile.ToSuiteFile(notInterrupted);
      if (file == null)
        return;

      var suiteElements = file.SuiteDeclarations.Select(_unitTestElementFactory.GetOrCreateClassSuiteRecursively);
      var allElements = suiteElements.SelectMany(x => x.Flatten(y => y.Children)).Cast<IUnitTestElementEx>();
      var dispositions = allElements.Select(x => x.GetDisposition(file)).ToList();
      dispositions.ForEach(consumer);
    }
  }
}