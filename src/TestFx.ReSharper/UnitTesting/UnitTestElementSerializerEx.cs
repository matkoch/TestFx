﻿// Copyright 2014, 2013 Matthias Koch
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
using System.Xml;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.ReSharper.UnitTesting.Elements;
using TestFx.Utilities;

namespace TestFx.ReSharper.UnitTesting
{
  public interface IUnitTestElementSerializerEx : IUnitTestElementSerializer
  {
  }

  [SolutionComponent]
  public class UnitTestElementSerializerEx : IUnitTestElementSerializerEx
  {
    private const string c_projectId = "projectId";
    private const string c_elementType = "elementType";
    private const string c_text = "text";
    private const string c_absoluteId = "absoluteId";

    private readonly IUnitTestElementFactoryEx _unitTestElementFactory;
    private readonly IUnitTestProviderEx _unitTestProvider;
    private readonly ISolution _solution;

    public UnitTestElementSerializerEx (IUnitTestElementFactoryEx unitTestElementFactory, IUnitTestProviderEx unitTestProvider, ISolution solution)
    {
      _unitTestElementFactory = unitTestElementFactory;
      _unitTestProvider = unitTestProvider;
      _solution = solution;
    }

    public void SerializeElement (XmlElement xmlElement, IUnitTestElement element)
    {
      xmlElement.SetAttribute(c_elementType, element.GetType().FullName);
      xmlElement.SetAttribute(c_absoluteId, element.Id);
      xmlElement.SetAttribute(c_projectId, ((IUnitTestElementEx) element).GetProject().AssertNotNull().GetPersistentID());
      xmlElement.SetAttribute(c_text, element.GetPresentation());
    }

#if R9
    public IUnitTestElement DeserializeElement (XmlElement parent, string id, [CanBeNull] IUnitTestElement parentElement, IProject project, PersistentProjectId projectId)
    {
      return DeserializeElement(parent, parentElement);
    }
#endif

    public IUnitTestElement DeserializeElement (XmlElement xmlElement, [CanBeNull] IUnitTestElement parentElement)
    {
      var elementTypeFullName = xmlElement.GetAttribute(c_elementType);
      var absoluteId = xmlElement.GetAttribute(c_absoluteId);
      var projectId = xmlElement.GetAttribute(c_projectId);
      var text = xmlElement.GetAttribute(c_text);

      var identity = Identity.Parse(absoluteId);
      var project = ProjectUtil.FindProjectElementByPersistentID(_solution, projectId).GetProject();

      return _unitTestElementFactory.GetOrCreateSingleElement(elementTypeFullName, identity, project, text, parentElement);
    }

    public IUnitTestProvider Provider
    {
      get { return _unitTestProvider; }
    }
  }
}