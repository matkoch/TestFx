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
using JetBrains.Annotations;

namespace TestFx.Utilities.Reflection
{
  public static class TypeExtensions
  {
    public static bool HasDefaultConstructor (this Type type)
    {
      return TypeUtility.Instance.HasDefaultConstructor(type);
    }

    public static bool IsInstantiatable<T> (this Type type)
    {
      return TypeUtility.Instance.IsInstantiatable(type, typeof (T));
    }

    public static T CreateInstance<T> (this Type type, IEnumerable<object> args)
    {
      return CreateInstance<T>(type, args.ToArray());
    }

    public static T CreateInstance<T> (this Type type, params object[] args)
    {
      return (T) TypeUtility.Instance.CreateInstance(type, args);
    }

    public static IEnumerable<Type> GetImmediateInterfaces (this Type type)
    {
      return TypeUtility.Instance.GetImmediateInterfaces(type);
    }

    public static IEnumerable<Type> GetImmediateDerivedTypesOf<T> (this Type type)
    {
      return TypeUtility.Instance.GetImmediateDerivedTypes(type, typeof (T));
    }

    [CanBeNull]
    public static Type GetClosedTypeOf (this Type type, Type openType)
    {
      return TypeUtility.Instance.GetSingleClosedType(type, openType);
    }

    [CanBeNull]
    public static object GetDefaultValue(this Type type)
    {
      return TypeUtility.Instance.GetDefaultValueFor(type);
    }
  }
}