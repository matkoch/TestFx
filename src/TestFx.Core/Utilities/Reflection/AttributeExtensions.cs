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
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace TestFx.Utilities.Reflection
{
  [PublicAPI ("Used by extensions")]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public static class AttributeExtensions
  {
    public static IEnumerable<T> GetAttributes<T> (this Assembly assembly) where T : Attribute
    {
      return AttributeUtility.Instance.GetAttributes<T>(assembly);
    }

    public static IEnumerable<T> GetAttributes<T> (this Type type) where T : Attribute
    {
      return AttributeUtility.Instance.GetAttributes<T>(type);
    }

    public static IEnumerable<T> GetAttributes<T> (this MemberInfo memberInfo) where T : Attribute
    {
      return AttributeUtility.Instance.GetAttributes<T>(memberInfo);
    }

    [CanBeNull]
    public static T GetAttribute<T> (this Assembly assembly) where T : Attribute
    {
      return AttributeUtility.Instance.GetAttribute<T>(assembly);
    }

    [CanBeNull]
    public static T GetAttribute<T> (this Type type) where T : Attribute
    {
      return AttributeUtility.Instance.GetAttribute<T>(type);
    }

    [CanBeNull]
    public static T GetAttribute<T> (this MemberInfo memberInfo) where T : Attribute
    {
      return AttributeUtility.Instance.GetAttribute<T>(memberInfo);
    }

    public static IEnumerable<Tuple<FieldInfo, TAttribute>> GetFieldsWithAttribute<TAttribute> (
        this Type type,
        BindingFlags bindingFlags = MemberBindings.All | BindingFlags.DeclaredOnly)
        where TAttribute : Attribute
    {
      return AttributeUtility.Instance.GetMembersWithAttribute<FieldInfo, TAttribute>(type, bindingFlags);
    }

    public static bool IsCompilerGenerated(this MemberInfo memberInfo)
    {
      return AttributeUtility.Instance.GetAttribute<CompilerGeneratedAttribute>(memberInfo) != null;
    }
  }
}