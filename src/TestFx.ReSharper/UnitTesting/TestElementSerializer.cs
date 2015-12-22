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
using System.Xml;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Util;
using TestFx.ReSharper.Model;
using TestFx.ReSharper.UnitTesting.Elements;
using TestFx.Utilities;

namespace TestFx.ReSharper.UnitTesting
{
  public interface ITestElementSerializer : IUnitTestElementSerializer
  {
  }

  [SolutionComponent]
  public class TestElementSerializer : ITestElementSerializer
  {
    private const string c_projectId = "projectId";
    private const string c_elementType = "elementType";
    private const string c_categories = "categories";
    private const string c_text = "text";
    private const string c_absoluteId = "absoluteId";

    private readonly ITestElementFactory _testElementFactory;
    private readonly ITestProvider _testProvider;
    private readonly ISolution _solution;

    public TestElementSerializer (ITestElementFactory testElementFactory, ITestProvider testProvider, ISolution solution)
    {
      _testElementFactory = testElementFactory;
      _testProvider = testProvider;
      _solution = solution;
    }

    public void SerializeElement (XmlElement xmlElement, IUnitTestElement element)
    {
      xmlElement.SetAttribute(c_elementType, element.GetType().FullName);
      xmlElement.SetAttribute(c_absoluteId, element.Id);
      xmlElement.SetAttribute(c_projectId, ((ITestElement) element).GetProject().AssertNotNull().GetPersistentID());
      xmlElement.SetAttribute(c_text, element.GetPresentation());
      xmlElement.SetAttribute(c_categories, element.Categories.Select(x => x.Name).Join("|"));
    }

    public IUnitTestElement DeserializeElement (
        XmlElement parent,
        string id,
        [CanBeNull] IUnitTestElement parentElement,
        IProject project)
    {
      return DeserializeElement(parent, parentElement);
    }

    public IUnitTestElement DeserializeElement (XmlElement xmlElement, [CanBeNull] IUnitTestElement parentElement)
    {
      var elementTypeFullName = xmlElement.GetAttribute(c_elementType);
      var absoluteId = xmlElement.GetAttribute(c_absoluteId);
      var projectId = xmlElement.GetAttribute(c_projectId);
      var text = xmlElement.GetAttribute(c_text);
      var categories = xmlElement.GetAttribute(c_categories).Split('|');

      var identity = Identity.Parse(absoluteId);
      var project = ProjectUtil.FindProjectElementByPersistentID(_solution, projectId).GetProject();
      var entity = new TestEntitySurrogate(identity, project, categories, text);

      return _testElementFactory.GetOrCreateTestElement(elementTypeFullName, entity, parentElement);
    }

    public IUnitTestProvider Provider
    {
      get { return _testProvider; }
    }
  }
}