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
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Utils;
// ReSharper disable All

namespace TestFx.ReSharper.Model.Metadata.Wrapper
{
  public abstract class MetadataTypeInfoBase : MetadataEntityBase, IMetadataTypeInfo
  {
    private readonly IMetadataTypeInfo _metadataTypeInfo;

    protected MetadataTypeInfoBase (IMetadataTypeInfo metadataTypeInfo)
        : base(metadataTypeInfo)
    {
      _metadataTypeInfo = metadataTypeInfo;
    }

    public IMetadataSecurityRow[] Security
    {
      get { return _metadataTypeInfo.Security; }
    }

    public string[] SecurityAttributesTypeName
    {
      get { return _metadataTypeInfo.SecurityAttributesTypeName; }
    }

    public bool HasSecurity
    {
      get { return _metadataTypeInfo.HasSecurity; }
    }

    public string Name
    {
      get { return _metadataTypeInfo.Name; }
    }

    public IMetadataTypeInfo DeclaringType
    {
      get { return _metadataTypeInfo.DeclaringType; }
    }

    public bool IsSpecialName
    {
      get { return _metadataTypeInfo.IsSpecialName; }
    }

    public bool IsRuntimeSpecialName
    {
      get { return _metadataTypeInfo.IsRuntimeSpecialName; }
    }

    public IEnumerable<MemberInfo> GetMemberInfos ()
    {
      return _metadataTypeInfo.GetMemberInfos();
    }

    public IMetadataMethod[] GetMethods ()
    {
      return _metadataTypeInfo.GetMethods();
    }

    public IMetadataField[] GetFields ()
    {
      return _metadataTypeInfo.GetFields();
    }

    public IMetadataProperty[] GetProperties ()
    {
      return _metadataTypeInfo.GetProperties();
    }

    public IMetadataEvent[] GetEvents ()
    {
      return _metadataTypeInfo.GetEvents();
    }

    public IMetadataTypeInfo[] GetNestedTypes ()
    {
      return _metadataTypeInfo.GetNestedTypes();
    }

    public bool HasExtensionMethods ()
    {
      return _metadataTypeInfo.HasExtensionMethods();
    }

    public string FullyQualifiedName
    {
      get { return _metadataTypeInfo.FullyQualifiedName; }
    }

    public string AssemblyQualifiedName
    {
      get { return _metadataTypeInfo.AssemblyQualifiedName; }
    }

    public string NamespaceName
    {
      get { return _metadataTypeInfo.NamespaceName; }
    }

    public string TypeName
    {
      get { return _metadataTypeInfo.TypeName; }
    }

    public AssemblyNameInfo DeclaringAssemblyName
    {
      get { return _metadataTypeInfo.DeclaringAssemblyName; }
    }

    public IMetadataClassType Base
    {
      get { return _metadataTypeInfo.Base; }
    }

    public IMetadataClassType[] Interfaces
    {
      get { return _metadataTypeInfo.Interfaces; }
    }

    public IMetadataGenericArgument[] GenericParameters
    {
      get { return _metadataTypeInfo.GenericParameters; }
    }

    public bool IsAbstract
    {
      get { return _metadataTypeInfo.IsAbstract; }
    }

    public bool IsSealed
    {
      get { return _metadataTypeInfo.IsSealed; }
    }

    public bool IsImported
    {
      get { return _metadataTypeInfo.IsImported; }
    }

    public ClassLayoutType Layout
    {
      get { return _metadataTypeInfo.Layout; }
    }

    public PInvokeInfo.CharSetSpec InteropStringFormat
    {
      get { return _metadataTypeInfo.InteropStringFormat; }
    }

    public bool IsBeforeFieldInit
    {
      get { return _metadataTypeInfo.IsBeforeFieldInit; }
    }

    public bool IsClass
    {
      get { return _metadataTypeInfo.IsClass; }
    }

    public bool IsInterface
    {
      get { return _metadataTypeInfo.IsInterface; }
    }

    public bool IsSerializable
    {
      get { return _metadataTypeInfo.IsSerializable; }
    }

    public bool IsWindowsRuntime
    {
      get { return _metadataTypeInfo.IsWindowsRuntime; }
    }

    public bool IsPublic
    {
      get { return _metadataTypeInfo.IsPublic; }
    }

    public bool IsNotPublic
    {
      get { return _metadataTypeInfo.IsNotPublic; }
    }

    public bool IsNested
    {
      get { return _metadataTypeInfo.IsNested; }
    }

    public bool IsNestedPublic
    {
      get { return _metadataTypeInfo.IsNestedPublic; }
    }

    public bool IsNestedPrivate
    {
      get { return _metadataTypeInfo.IsNestedPrivate; }
    }

    public bool IsNestedFamily
    {
      get { return _metadataTypeInfo.IsNestedFamily; }
    }

    public bool IsNestedAssembly
    {
      get { return _metadataTypeInfo.IsNestedAssembly; }
    }

    public bool IsNestedFamilyAndAssembly
    {
      get { return _metadataTypeInfo.IsNestedFamilyAndAssembly; }
    }

    public bool IsNestedFamilyOrAssembly
    {
      get { return _metadataTypeInfo.IsNestedFamilyOrAssembly; }
    }

    public int PackingSize
    {
      get { return _metadataTypeInfo.PackingSize; }
    }

    public int ClassSize
    {
      get { return _metadataTypeInfo.ClassSize; }
    }
  }
}