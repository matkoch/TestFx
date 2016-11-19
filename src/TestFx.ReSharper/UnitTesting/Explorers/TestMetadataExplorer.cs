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
using System.Linq;
using System.Threading;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.ReSharper.UnitTesting.Elements;
using TestFx.Utilities.Collections;
using JetBrains.ReSharper.Resources.Shell;
using TestFx.Evaluation.Runners;
using TestFx.ReSharper.UnitTesting.Explorers.Metadata;

namespace TestFx.ReSharper.UnitTesting.Explorers
{
  public interface ITestMetadataExplorer
  {
    void Explore (
      IProject project,
      IMetadataAssembly assembly,
      IUnitTestElementsObserver observer,
      CancellationToken cancellationToken);
  }

  [SolutionComponent]
  public class TestMetadataExplorer : ITestMetadataExplorer
  {
    private readonly ITestElementFactory _testElementFactory;
    
    public TestMetadataExplorer (ITestElementFactory testElementFactory)
    {
      _testElementFactory = testElementFactory;
    }

    public void Explore (IProject project, IMetadataAssembly assembly, IUnitTestElementsObserver observer, CancellationToken cancellationToken)
    {
      // TODO: ILMerge / embedded reference
      //if (!referencedAssemblies.Any(x => x.StartsWith("TestFx")))
      var frameworkPrefix = typeof (IRootRunner).Assembly.GetName().Name;
      var referencedAssemblies = assembly.ReferencedAssembliesNames.Select(x => x.Name);
      if (!referencedAssemblies.Any(x => x.StartsWith(frameworkPrefix)))
        return;

      using (ReadLockCookie.Create())
      {
        var testAssembly = assembly.ToTestAssembly(project, notInterrupted: () => !cancellationToken.IsCancellationRequested);
        if (testAssembly == null)
          return;

        var testElements = testAssembly.TestMetadatas.Select(_testElementFactory.GetOrCreateClassTestElementRecursively);
        var allTestElements = testElements.SelectMany(x => x.DescendantsAndSelf(y => y.Children)).ToList();
        foreach (var testElement in allTestElements)
        {
          observer.OnUnitTestElement(testElement);
          observer.OnUnitTestElementChanged(testElement);
        }
        observer.OnCompleted();
      }
    }
  }
}