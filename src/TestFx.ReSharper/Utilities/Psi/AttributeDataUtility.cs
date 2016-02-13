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
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace TestFx.ReSharper.Utilities.Psi
{
  public interface IAttributeDataUtility
  {
    IEnumerable<IAttributeInstance> GetAttributeDatas<T> (IAttributesSet attributeSet) where T : Attribute;

    [CanBeNull]
    IAttributeInstance GetAttributeData<T> (IAttributesSet attributeSet) where T : Attribute;
  }

  internal class AttributeDataUtility : IAttributeDataUtility
  {
    public static IAttributeDataUtility Instance = new AttributeDataUtility();

    public IEnumerable<IAttributeInstance> GetAttributeDatas<T> (IAttributesSet attributeSet) where T : Attribute
    {
      return attributeSet.GetAttributeInstances(inherit: false).Where(x => x.GetAttributeType().Implements(typeof (T)));
    }

    [CanBeNull]
    public IAttributeInstance GetAttributeData<T> (IAttributesSet attributeSet) where T : Attribute
    {
      return GetAttributeDatas<T>(attributeSet).SingleOrDefault();
    }
  }
}