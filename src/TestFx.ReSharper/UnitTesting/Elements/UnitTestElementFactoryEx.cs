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
    IUnitTestElement GetOrCreateClassSuiteRecursively (ISuiteEntity suiteEntity);

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
                            { typeof (ClassSuiteElement).FullName, GetOrCreateClassSuite },
                            { typeof (SuiteElement).FullName, GetOrCreateSuite },
                            { typeof (TestElement).FullName, GetOrCreateTest }
                        };
    }

    public IUnitTestElement GetOrCreateClassSuiteRecursively (ISuiteEntity suiteEntity)
    {
      var element = GetOrCreateAndUpdateElement(
          suiteEntity,
          identity =>
              new ClassSuiteElement(
                  identity,
                  new Task[] { new RunTask(), new AssemblySuiteTask(identity.Parent), new SuiteTask(identity) }));

      CreateAndAppendChildren(element, suiteEntity);
      return element;
    }

    private IUnitTestElement GetOrCreateSuite (ISuiteEntity suiteEntity)
    {
      var element = GetOrCreateAndUpdateElement(
          suiteEntity,
          identity => new SuiteElement(identity, new Task[] { new SuiteTask(identity) }));

      CreateAndAppendChildren(element, suiteEntity);
      return element;
    }

    private IUnitTestElement GetOrCreateTest (ITestEntity testEntity)
    {
      return GetOrCreateAndUpdateElement(
          testEntity,
          identity => new TestElement(identity, new Task[] { new TestTask(identity) }));
    }

    [CanBeNull]
    private IUnitTestElement GetOrCreateAndUpdateElement (
        IUnitTestEntity unitTestEntity,
        Func<IUnitTestIdentity, IUnitTestElementEx> factory)
    {
      var identity = new UnitTestIdentity(_unitTestProvider, unitTestEntity.Project, unitTestEntity.Identity);
      var element = _unitTestElementManager.GetElementByIdentity(identity) ?? factory(identity);

      element.Update(unitTestEntity.Text, null, Enumerable.Empty<UnitTestElementCategory>());

      return element;
    }

    private void CreateAndAppendChildren (IUnitTestElement suiteElement, ISuiteEntity suiteEntity)
    {
      suiteEntity.SuiteEntities.Select(GetOrCreateSuite).ForEach(x => x.Parent = suiteElement);
      suiteEntity.TestEntities.Select(GetOrCreateTest).ForEach(x => x.Parent = suiteElement);
    }

    public IUnitTestElement GetOrCreateSingleElement (
        string elementTypeFullName,
        IIdentity identity,
        IProject project,
        string text,
        [CanBeNull] IUnitTestElement parentElement)
    {
      var element = _factoryMethods[elementTypeFullName](identity, project, text);
      element.Parent = parentElement;
      return element;
    }

    private IUnitTestElement GetOrCreateClassSuite (IIdentity identity, IProject project, string text)
    {
      var suiteEntity = new SuiteEntitySurrogate(identity, project, text);
      return GetOrCreateClassSuiteRecursively(suiteEntity);
    }

    private IUnitTestElement GetOrCreateSuite (IIdentity identity, IProject project, string text)
    {
      var suiteEntity = new SuiteEntitySurrogate(identity, project, text);
      return GetOrCreateSuite(suiteEntity);
    }

    private IUnitTestElement GetOrCreateTest (IIdentity identity, IProject project, string text)
    {
      var testEntity = new TestEntitySurrogate(identity, project, text);
      return GetOrCreateTest(testEntity);
    }
  }
}