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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestFx.Utilities.Reflection
{
  public interface ITypeUtility
  {
    bool HasDefaultConstructor (Type type);
    bool IsInstantiatable (Type type, Type targetType);
    object CreateInstance (Type type, object[] args);
    IEnumerable<Type> GetDirectInterfaces (Type type);
    IEnumerable<Type> GetDirectDerivedTypes (Type type, Type implementedType);
  }

  public class TypeUtility : ITypeUtility
  {
    public static ITypeUtility Instance = new TypeUtility();

    public bool HasDefaultConstructor (Type type)
    {
      return type.GetConstructor(MemberBindings.Instance, null, new Type[0], new ParameterModifier[0]) != null;
    }

    public bool IsInstantiatable (Type type, Type targetType)
    {
      return !type.IsAbstract &&
             !type.IsGenericType &&
             type.HasDefaultConstructor() &&
             targetType.IsAssignableFrom(type);
    }

    public object CreateInstance (Type type, object[] args)
    {
      return Activator.CreateInstance(type, MemberBindings.Instance, null, args, null);
    }

    public IEnumerable<Type> GetDirectInterfaces (Type type)
    {
      var allInterfaces = type.GetInterfaces();
      return allInterfaces.Except(allInterfaces.SelectMany(x => x.GetInterfaces()));
    }

    public IEnumerable<Type> GetDirectDerivedTypes (Type type, Type implementedType)
    {
      return type.Follow(x => x.BaseType).Concat(type.GetInterfaces())
          .Where(x => x.BaseType == implementedType || x.GetDirectInterfaces().Contains(implementedType));
    }
  }
}