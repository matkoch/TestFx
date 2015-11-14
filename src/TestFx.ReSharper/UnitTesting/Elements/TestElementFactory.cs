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
using TestFx.ReSharper.Runner.Tasks;
using TestFx.ReSharper.UnitTesting.Utilities;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.UnitTesting.Elements
{
  public interface ITestElementFactory
  {
    IUnitTestElement GetOrCreateClassTestElementRecursively (ITestEntity testEntity);

    IUnitTestElement GetOrCreateTestElement (string elementTypeFullName, ITestEntity entity, [CanBeNull] IUnitTestElement parentElement);
  }

  [SolutionComponent]
  public class TestElementFactory : ITestElementFactory
  {
    private readonly ITestProvider _testProvider;
    private readonly IUnitTestElementManager _unitTestElementManager;
    private readonly IUnitTestElementIdFactory _unitTestElementIdFactory;
    private readonly Dictionary<string, Func<ITestEntity, IUnitTestElement>> _factoryMethods;

    public TestElementFactory (ITestProvider testProvider, IUnitTestElementManager unitTestElementManager, IUnitTestElementIdFactory unitTestElementIdFactory)
    {
      _testProvider = testProvider;
      _unitTestElementManager = unitTestElementManager;
      _unitTestElementIdFactory = unitTestElementIdFactory;
      _factoryMethods = new Dictionary<string, Func<ITestEntity, IUnitTestElement>>
                        {
                            { typeof (ClassTestElement).FullName, GetOrCreateClassTestElementRecursively },
                            { typeof (ChildTestElement).FullName, GetOrCreateChildTest }
                        };
    }

    public IUnitTestElement GetOrCreateClassTestElementRecursively (ITestEntity testEntity)
    {
      var element = GetOrCreateAndUpdateElement(
          testEntity,
          identity =>
              new ClassTestElement(
                  identity,
                  new Task[] { new RunTask(), new AssemblyTask(identity.Parent), new TestTask(identity) }));

      CreateAndAppendChildren(element, testEntity);
      return element;
    }

    private IUnitTestElement GetOrCreateChildTest (ITestEntity testEntity)
    {
      return GetOrCreateAndUpdateElement(
          testEntity,
          identity => new ChildTestElement(identity, new Task[] { new TestTask(identity) }));
    }

    [CanBeNull]
    private IUnitTestElement GetOrCreateAndUpdateElement (
        ITestEntity testEntity,
        Func<ITestIdentity, ITestElement> factory)
    {
      var elementId = _unitTestElementIdFactory.Create(_testProvider, testEntity.Project, testEntity.Identity.Absolute);
      var identity = new TestIdentity(elementId, testEntity.Identity);
      var element = _unitTestElementManager.GetElementByIdentity(identity) ?? factory(identity);

      element.Update(testEntity.Text, null, Enumerable.Empty<UnitTestElementCategory>());

      return element;
    }

    private void CreateAndAppendChildren (IUnitTestElement testElement, ITestEntity testEntity)
    {
      testEntity.TestEntities.Select(GetOrCreateChildTest).ForEach(x => x.Parent = testElement);
    }

    public IUnitTestElement GetOrCreateTestElement (
        string elementTypeFullName,
        ITestEntity entity,
        [CanBeNull] IUnitTestElement parentElement)
    {
      var factory = _factoryMethods[elementTypeFullName];
      var element = factory(entity);
      element.Parent = parentElement;
      return element;
    }
  }
}