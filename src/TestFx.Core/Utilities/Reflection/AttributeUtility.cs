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
using System.Reflection;
using JetBrains.Annotations;
using TestFx.Utilities.Collections;

namespace TestFx.Utilities.Reflection
{
  public interface IAttributeUtility
  {
    IEnumerable<T> GetAttributes<T> (Assembly assembly) where T : Attribute;
    IEnumerable<T> GetAttributes<T> (Type type) where T : Attribute;
    IEnumerable<T> GetAttributes<T> (MemberInfo memberInfo) where T : Attribute;

    [CanBeNull]
    T GetAttribute<T> (Assembly assembly) where T : Attribute;

    [CanBeNull]
    T GetAttribute<T> (Type type) where T : Attribute;

    [CanBeNull]
    T GetAttribute<T> (MemberInfo memberInfo) where T : Attribute;

    IEnumerable<Tuple<TMember, TAttribute>> GetMembersWithAttribute<TMember, TAttribute> (Type type, BindingFlags bindingFlags)
        where TMember : MemberInfo
        where TAttribute : Attribute;
  }

  public class AttributeUtility : IAttributeUtility
  {
    public static IAttributeUtility Instance = new AttributeUtility();

    public IEnumerable<T> GetAttributes<T> (Assembly assembly) where T : Attribute
    {
      return Attribute.GetCustomAttributes(assembly, typeof (T), inherit: true).Cast<T>();
    }

    public IEnumerable<T> GetAttributes<T> (Type type) where T : Attribute
    {
      return Attribute.GetCustomAttributes(type, typeof (T), inherit: true).Cast<T>();
    }

    public IEnumerable<T> GetAttributes<T> (MemberInfo memberInfo) where T : Attribute
    {
      return Attribute.GetCustomAttributes(memberInfo, typeof (T), inherit: true).Cast<T>();
    }

    [CanBeNull]
    public T GetAttribute<T> (Assembly assembly) where T : Attribute
    {
      return GetAttributes<T>(assembly).SingleOrDefault();
    }

    [CanBeNull]
    public T GetAttribute<T> (Type type) where T : Attribute
    {
      return GetAttributes<T>(type).SingleOrDefault();
    }

    [CanBeNull]
    public T GetAttribute<T> (MemberInfo memberInfo) where T : Attribute
    {
      return GetAttributes<T>(memberInfo).SingleOrDefault();
    }

    public IEnumerable<Tuple<TMember, TAttribute>> GetMembersWithAttribute<TMember, TAttribute> (Type type, BindingFlags bindingFlags)
        where TMember : MemberInfo
        where TAttribute : Attribute
    {
      return type.DescendantsAndSelf(x => x.BaseType)
          .SelectMany(x => x.GetMembers(bindingFlags))
          .OfType<TMember>()
          .Select(x => Tuple.Create(x, GetAttribute<TAttribute>(x)))
          .Where(x => x.Item2 != null);
    }
  }
}