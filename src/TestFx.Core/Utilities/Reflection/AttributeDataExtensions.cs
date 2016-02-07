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
using System.Reflection;
using JetBrains.Annotations;

namespace TestFx.Utilities.Reflection
{
  [PublicAPI]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public static class AttributeDataExtensions
  {
    public static IEnumerable<CustomAttributeData> GetAttributeDatas<T> (this Assembly assembly) where T : Attribute
    {
      return AttributeDataUtility.Instance.GetAttributeDatas<T>(assembly);
    }

    public static IEnumerable<CustomAttributeData> GetAttributeDatas<T> (this Type type) where T : Attribute
    {
      return AttributeDataUtility.Instance.GetAttributeDatas<T>(type);
    }

    public static IEnumerable<CustomAttributeData> GetAttributeDatas<T> (this MemberInfo memberInfo) where T : Attribute
    {
      return AttributeDataUtility.Instance.GetAttributeDatas<T>(memberInfo);
    }

    [CanBeNull]
    public static CustomAttributeData GetAttributeData<T>(this Assembly assembly) where T : Attribute
    {
      return AttributeDataUtility.Instance.GetAttributeData<T>(assembly);
    }

    [CanBeNull]
    public static CustomAttributeData GetAttributeData<T> (this Type type) where T : Attribute
    {
      return AttributeDataUtility.Instance.GetAttributeData<T>(type);
    }

    [CanBeNull]
    public static CustomAttributeData GetAttributeData<T>(this MemberInfo memberInfo) where T : Attribute
    {
      return AttributeDataUtility.Instance.GetAttributeData<T>(memberInfo);
    }
  }
}