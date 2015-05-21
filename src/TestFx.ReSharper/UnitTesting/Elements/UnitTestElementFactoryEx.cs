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
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Elements;
using TestFx.ReSharper.Model;
using TestFx.ReSharper.Model.Surrogates;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.ReSharper.UnitTesting.Utilities;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.UnitTesting.Elements
{
  public interface IUnitTestElementFactoryEx
  {
    IUnitTestElement GetOrCreateClassTestRecursively (ITestEntity testEntity);

    IUnitTestElement GetOrCreateSingleElement (
        string elementTypeFullName,
        IIdentity identity,
        IProject project,
        string text,
        [CanBeNull] IUnitTestElement parentElement);
  }

  [SolutionComponent]
  public class UnitTestElementFactoryEx : IUnitTestElementFactoryEx
  {
    private readonly IUnitTestProviderEx _unitTestProvider;
    private readonly IUnitTestElementManager _unitTestElementManager;
    private readonly Dictionary<string, Func<IIdentity, IProject, string, IUnitTestElement>> _factoryMethods;

    public UnitTestElementFactoryEx (IUnitTestProviderEx unitTestProvider, IUnitTestElementManager unitTestElementManager)
    {
      _unitTestProvider = unitTestProvider;
      _unitTestElementManager = unitTestElementManager;
      _factoryMethods = new Dictionary<string, Func<IIdentity, IProject, string, IUnitTestElement>>
                        {
                            { typeof (ClassTestElement).FullName, GetOrCreateClassTest },
                            { typeof (TestElement).FullName, GetOrCreateChildTest }
                        };
    }

    public IUnitTestElement GetOrCreateClassTestRecursively (ITestEntity testEntity)
    {
      var element = GetOrCreateAndUpdateElement(
          testEntity,
          identity =>
              new ClassTestElement(
                  identity,
                  new Task[] { new RunTask(), new AssemblyTestTask(identity.Parent), new TestTask(identity) }));

      CreateAndAppendChildren(element, testEntity);
      return element;
    }

    private IUnitTestElement GetOrCreateChildTest (ITestEntity testEntity)
    {
      return GetOrCreateAndUpdateElement(
          testEntity,
          identity => new TestElement(identity, new Task[] { new TestTask(identity) }));
    }

    [CanBeNull]
    private IUnitTestElement GetOrCreateAndUpdateElement (
        ITestEntity testEntity,
        Func<IUnitTestIdentity, IUnitTestElementEx> factory)
    {
      var identity = new UnitTestIdentity(_unitTestProvider, testEntity.Project, testEntity.Identity);
      var element = _unitTestElementManager.GetElementByIdentity(identity) ?? factory(identity);

      element.Update(testEntity.Text, null, Enumerable.Empty<UnitTestElementCategory>());

      return element;
    }

    private void CreateAndAppendChildren (IUnitTestElement testElement, ITestEntity testEntity)
    {
      testEntity.TestEntities.Select(GetOrCreateChildTest).ForEach(x => x.Parent = testElement);
    }

    public IUnitTestElement GetOrCreateSingleElement (
        string elementTypeFullName,
        IIdentity identity,
        IProject project,
        string text,
        [CanBeNull] IUnitTestElement parentElement)
    {
      var factory = _factoryMethods[elementTypeFullName];
      var element = factory(identity, project, text);
      element.Parent = parentElement;
      return element;
    }

    // TODO: repetition
    private IUnitTestElement GetOrCreateClassTest (IIdentity identity, IProject project, string text)
    {
      var testEntity = new TestEntitySurrogate(identity, project, text);
      return GetOrCreateClassTestRecursively(testEntity);
    }

    private IUnitTestElement GetOrCreateChildTest (IIdentity identity, IProject project, string text)
    {
      var testEntity = new TestEntitySurrogate(identity, project, text);
      return GetOrCreateChildTest(testEntity);
    }
  }
}