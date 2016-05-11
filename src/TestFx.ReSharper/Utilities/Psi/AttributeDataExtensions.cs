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
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace TestFx.ReSharper.Utilities.Psi
{
  public static class AttributeDataExtensions
  {
    public static IEnumerable<IAttributeInstance> GetAttributeDatas<T> (this IAttributesSet attributeSet) where T : Attribute
    {
      return attributeSet.GetAttributeDatas(typeof(T).FullName);
    }

    public static IEnumerable<IAttributeInstance> GetAttributeDatas (this IAttributesSet attributeSet, string attributeType)
    {
      return AttributeDataUtility.Instance.GetAttributeDatas(attributeSet, attributeType);
    }

    [CanBeNull]
    public static IAttributeInstance GetAttributeData<T> (this IAttributesSet attributeSet) where T : Attribute
    {
      return attributeSet.GetAttributeData(typeof(T).FullName);
    }

    [CanBeNull]
    public static IAttributeInstance GetAttributeData (this IAttributesSet attributeSet, string attributeType)
    {
      return AttributeDataUtility.Instance.GetAttributeData(attributeSet, attributeType);
    }
  }
}