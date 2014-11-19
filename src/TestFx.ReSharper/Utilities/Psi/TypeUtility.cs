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
using JetBrains.ReSharper.Psi;
using TestFx.Utilities;

namespace TestFx.ReSharper.Utilities.Psi
{
  public interface ITypeUtility
  {
    IEnumerable<ITypeElement> GetImplementedTypes (IDeclaredType type);
    IEnumerable<ITypeElement> GetImplementedTypes (ITypeElement type);

    bool IsImplementingType (IDeclaredType type, Type implementedType);
  }

  public class TypeUtility : ITypeUtility
  {
    public static ITypeUtility Instance = new TypeUtility();

    public IEnumerable<ITypeElement> GetImplementedTypes (IDeclaredType type)
    {
      return GetImplementedTypes(type.GetTypeElement());
    }

    public IEnumerable<ITypeElement> GetImplementedTypes (ITypeElement type)
    {
      return type.Concat(type.Flatten(x => x.GetSuperTypes().Select(y => y.GetTypeElement())));
    }

    public bool IsImplementingType (IDeclaredType type, Type implementedType)
    {
      return GetImplementedTypes(type).Any(x => x.GetClrName().FullName == implementedType.FullName);
    }
  }
}