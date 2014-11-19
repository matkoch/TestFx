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
using JetBrains.ReSharper.Psi;

namespace TestFx.ReSharper.Utilities.Psi
{
  public static class TypeExtensions
  {
    public static IEnumerable<ITypeElement> GetImplementedTypes (this IDeclaredType type)
    {
      return TypeUtility.Instance.GetImplementedTypes(type);
    }

    public static IEnumerable<ITypeElement> GetImplementedTypes (this ITypeElement type)
    {
      return TypeUtility.Instance.GetImplementedTypes(type);
    }

    public static bool Implements (this IDeclaredType type, Type implementedType)
    {
      return TypeUtility.Instance.IsImplementingType(type, implementedType);
    }
  }
}