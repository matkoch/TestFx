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
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Elements;
using TestFx.ReSharper.Model;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.ReSharper.UnitTesting.Serialization;
using TestFx.ReSharper.UnitTesting.Utilities;
using TestFx.Utilities;

namespace TestFx.ReSharper.UnitTesting.Elements
{
  public interface IUnitTestElementFactoryEx
  {
    IUnitTestElement GetOrCreateClassSuite (IIdentity identity, IProject project, string text);
    IUnitTestElement GetOrCreateSuite (IIdentity identity, IProject project, string text);
    IUnitTestElement GetOrCreateTest (IIdentity identity, IProject project, string text);

    IUnitTestElement GetOrCreateClassSuite (ISuiteEntity suiteEntity);
    IUnitTestElement GetOrCreateSuite (ISuiteEntity suiteEntity);
    IUnitTestElement GetOrCreateTest (ITestEntity testEntity);
  }

  [SolutionComponent]
  public class UnitTestElementFactoryEx : IUnitTestElementFactoryEx
  {
    private readonly IUnitTestProviderEx _unitTestProvider;
    private readonly IUnitTestElementManager _unitTestElementManager;

    public UnitTestElementFactoryEx (IUnitTestProviderEx unitTestProvider, IUnitTestElementManager unitTestElementManager)
    {
      _unitTestProvider = unitTestProvider;
      _unitTestElementManager = unitTestElementManager;
    }

    public IUnitTestElement GetOrCreateClassSuite (IIdentity identity, IProject project, string text)
    {
      var suiteEntity = new SuiteEntitySurrogate(identity, project, text);
      return GetOrCreateClassSuite(suiteEntity);
    }

    public IUnitTestElement GetOrCreateSuite (IIdentity identity, IProject project, string text)
    {
      var suiteEntity = new SuiteEntitySurrogate(identity, project, text);
      return GetOrCreateSuite(suiteEntity);
    }

    public IUnitTestElement GetOrCreateTest (IIdentity identity, IProject project, string text)
    {
      var testEntity = new TestEntitySurrogate(identity, project, text);
      return GetOrCreateTest(testEntity);
    }

    public IUnitTestElement GetOrCreateClassSuite (ISuiteEntity suiteEntity)
    {
      var element = GetOrCreateAndUpdateElement(
          suiteEntity,
          identity =>
              new ClassSuiteElement(
                  identity,
                  new Task[] { new RunTask(), new AssemblySuiteTask(identity.Parent), new SuiteTask(identity) }));

      AppendChildren(element, suiteEntity);
      return element;
    }

    public IUnitTestElement GetOrCreateSuite (ISuiteEntity suiteEntity)
    {
      var element = GetOrCreateAndUpdateElement(
          suiteEntity,
          identity => new SuiteElement(identity, new Task[] { new SuiteTask(identity) }));

      AppendChildren(element, suiteEntity);
      return element;
    }

    public IUnitTestElement GetOrCreateTest (ITestEntity testEntity)
    {
      return GetOrCreateAndUpdateElement(
          testEntity,
          identity => new TestElement(identity, new Task[] { new TestTask(identity, testEntity.Text) }));
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

    private void AppendChildren (IUnitTestElement suiteElement, ISuiteEntity suiteEntity)
    {
      suiteEntity.SuiteEntities.Select(GetOrCreateSuite).ForEach(x => x.Parent = suiteElement);
      suiteEntity.TestEntities.Select(GetOrCreateTest).ForEach(x => x.Parent = suiteElement);
    }
  }
}