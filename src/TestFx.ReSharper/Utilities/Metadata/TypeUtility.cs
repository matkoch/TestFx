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
using System.Linq;
using JetBrains.Metadata.Reader.API;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.Utilities.Metadata
{
  public interface ITypeUtility
  {
    IEnumerable<IMetadataTypeInfo> GetImplementedTypes (IMetadataTypeInfo type);

    bool IsImplementingType (IMetadataTypeInfo type, Type implementedType);
  }

  public class TypeUtility : ITypeUtility
  {
    public static ITypeUtility Instance = new TypeUtility();

    public IEnumerable<IMetadataTypeInfo> GetImplementedTypes (IMetadataTypeInfo type)
    {
      return type.DescendantsAndSelf(x => x.Base.NotNull().Type, x => x.Base != null).Concat(type.Interfaces.Select(x => x.Type));
    }

    public bool IsImplementingType (IMetadataTypeInfo type, Type implementedType)
    {
      return GetImplementedTypes(type).Any(x => x.FullyQualifiedName == implementedType.FullName);
    }
  }
}