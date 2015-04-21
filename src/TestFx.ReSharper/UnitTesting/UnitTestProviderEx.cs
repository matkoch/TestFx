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
using System.Diagnostics;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.Extensibility;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.ReSharper.UnitTesting.Elements;
using TestFx.ReSharper.Utilities.Psi;
using TestFx.Utilities;
using RecursiveRemoteTaskRunner = TestFx.ReSharper.Runner.RecursiveRemoteTaskRunner;

namespace TestFx.ReSharper.UnitTesting
{
  public interface IUnitTestProviderEx : IUnitTestProvider, IDynamicUnitTestProvider
  {
    IUnitTestElement GetDynamicElement (RemoteTask remoteTask, Func<string, IUnitTestElementEx> elementProvider);
  }

  [UnitTestProvider]
  public partial class UnitTestProviderEx : IUnitTestProviderEx
  {
    private readonly UnitTestElementComparer _unitTestElementComparer;
    private readonly Dictionary<string, string> _taskTypeToElementType;

    public UnitTestProviderEx ()
    {
      _unitTestElementComparer = new UnitTestElementComparer(typeof (ClassSuiteElement), typeof (SuiteElement), typeof (TestElement));
      _taskTypeToElementType = new Dictionary<string, string>
                               {
                                   { typeof (SuiteTask).FullName, typeof (SuiteElement).FullName },
                                   { typeof (TestTask).FullName, typeof (TestElement).FullName }
                               };
    }

    public string ID
    {
      get { return RecursiveRemoteTaskRunner.ID; }
    }

    public string Name
    {
      get { return ID; }
    }

    public bool IsElementOfKind (IDeclaredElement declaredElement, UnitTestElementKind elementKind)
    {
      var clazz = declaredElement as ITypeElement;
      if (clazz == null)
      {
        var member = declaredElement as ITypeMember;
        if (member == null)
          return false;

        clazz = member.GetContainingType();
      }

      return clazz.GetAttributeData<SubjectAttributeBase>() != null;
    }

    public bool IsElementOfKind (IUnitTestElement element, UnitTestElementKind elementKind)
    {
      var testElement = element as IUnitTestElementEx;
      return testElement != null && testElement.ElementKind == elementKind;
    }

    public bool IsSupported (IHostProvider hostProvider)
    {
      return true;
    }

    public int CompareUnitTestElements (IUnitTestElement x, IUnitTestElement y)
    {
      return _unitTestElementComparer.Compare(x, y);
    }

    public IUnitTestElement GetDynamicElement (RemoteTask remoteTask, Func<string, IUnitTestElementEx> elementProvider)
    {
      var dynamicTask = (DynamicTask) remoteTask;
      var parentElement = elementProvider(dynamicTask.ParentAbsoluteId);
      Trace.Assert(parentElement != null, "parentElement != null");

      var project = parentElement.GetProject().AssertNotNull();
      var elementFactory = project.GetComponent<IUnitTestElementFactoryEx>();

      var elementTypeFullName = _taskTypeToElementType[dynamicTask.TaskTypeFullName];
      var element = elementFactory.GetOrCreateSingleElement(elementTypeFullName, dynamicTask.Identity, project, dynamicTask.Text, parentElement);
      // TODO: parameter for elementFactory instead?
      //element.State = UnitTestElementState.Dynamic;

      return element;
    }
  }
}