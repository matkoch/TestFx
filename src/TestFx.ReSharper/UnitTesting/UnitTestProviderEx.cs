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
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.ReSharper.UnitTesting.Elements;
using TestFx.Utilities;

namespace TestFx.ReSharper.UnitTesting
{
  public interface IUnitTestProviderEx : IUnitTestProvider
  {
  }

  [UnitTestProvider]
  public partial class UnitTestProviderEx : IUnitTestProviderEx
  {
    private readonly UnitTestElementComparer _unitTestElementComparer;

    public UnitTestProviderEx ()
    {
      _unitTestElementComparer = new UnitTestElementComparer(typeof (ClassSuiteElement), typeof (SuiteElement), typeof (TestElement));
    }

    public string ID
    {
      get { return Runner.RecursiveRemoteTaskRunner.ID; }
    }

    public string Name
    {
      get { return ID; }
    }

    public bool IsElementOfKind (IDeclaredElement declaredElement, UnitTestElementKind elementKind)
    {
      throw new NotSupportedException();
    }

    public bool IsElementOfKind (IUnitTestElement element, UnitTestElementKind elementKind)
    {
      var testElement = element.As<IUnitTestElementEx>();
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
  }
}