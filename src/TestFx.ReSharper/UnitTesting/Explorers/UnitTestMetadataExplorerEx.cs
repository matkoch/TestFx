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
using System.Linq;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.Extensibility;
using TestFx.ReSharper.Model.Metadata.Aggregation;
using TestFx.ReSharper.UnitTesting.Elements;
using TestFx.Utilities.Collections;
#if R8
using JetBrains.Application;
#elif R9
using JetBrains.ReSharper.Resources.Shell;
#endif

namespace TestFx.ReSharper.UnitTesting.Explorers
{
  public partial interface IUnitTestMetadataExplorerEx
  {
    void Explore (
        IProject project,
        IMetadataAssembly assembly,
        Action<IUnitTestElement> consumer,
        Func<bool> notInterrupted);
  }

  public partial class UnitTestMetadataExplorerEx : IUnitTestMetadataExplorerEx
  {
    private readonly IUnitTestElementFactoryEx _unitTestElementFactory;

    public void Explore (IProject project, IMetadataAssembly assembly, Action<IUnitTestElement> consumer, Func<bool> notInterrupted)
    {
      // TODO: ILMerge / embedded reference
      //if (!referencedAssemblies.Any(x => x.StartsWith("TestFx")))
      var frameworkPrefix = typeof (ISuite).Assembly.GetName().Name;
      var referencedAssemblies = assembly.ReferencedAssembliesNames.Select(x => x.Name);
      if (!referencedAssemblies.Any(x => x.StartsWith(frameworkPrefix)))
        return;

      using (ReadLockCookie.Create())
      {
        var assemblySuite = assembly.ToAssemblySuite(project, notInterrupted);
        var suiteElements = assemblySuite.SuiteEntities.Select(_unitTestElementFactory.GetOrCreateClassSuiteRecursively);
        suiteElements.SelectMany(x => x.DescendantsAndSelf(y => y.Children)).ForEach(consumer);
      }
    }
  }
}