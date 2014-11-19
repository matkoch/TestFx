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
using JetBrains.Metadata.Access;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Utils;
using JetBrains.Util;

namespace TestFx.ReSharper.Model.Metadata.Wrapper
{
  public abstract class MetadataAssemblyBase : MetadataEntityBase, IMetadataAssembly
  {
    private readonly IMetadataAssembly _metadataAssembly;

    protected MetadataAssemblyBase (IMetadataAssembly metadataAssembly)
        : base(metadataAssembly)
    {
      _metadataAssembly = metadataAssembly;
    }

    public IMetadataSecurityRow[] Security
    {
      get { return _metadataAssembly.Security; }
    }

    public string[] SecurityAttributesTypeName
    {
      get { return _metadataAssembly.SecurityAttributesTypeName; }
    }

    public bool HasSecurity
    {
      get { return _metadataAssembly.HasSecurity; }
    }

    public IMetadataTypeInfo GetTypeInfoFromQualifiedName (string name, AssemblyNameInfo assemblyName, bool searchReferencedAssemblies)
    {
      return _metadataAssembly.GetTypeInfoFromQualifiedName(name, assemblyName, searchReferencedAssemblies);
    }

    public IMetadataTypeInfo GetTypeInfoFromQualifiedName (string qualifiedName, bool searchReferencedAssemblies)
    {
      return _metadataAssembly.GetTypeInfoFromQualifiedName(qualifiedName, searchReferencedAssemblies);
    }

    public IMetadataTypeInfo GetTypeInfoFromToken (MetadataToken token)
    {
      return _metadataAssembly.GetTypeInfoFromToken(token);
    }

    public IMetadataType GetTypeFromQualifiedName (string qualifiedName, bool searchReferencedAssemblies)
    {
      return _metadataAssembly.GetTypeFromQualifiedName(qualifiedName, searchReferencedAssemblies);
    }

    public IMetadataTypeInfo[] GetTypes ()
    {
      return _metadataAssembly.GetTypes();
    }

    public IMetadataTypeInfo[] GetExportedTypes ()
    {
      return _metadataAssembly.GetExportedTypes();
    }

    public IDictionary<string, AssemblyNameInfo> GetForwardedTypes ()
    {
      return _metadataAssembly.GetForwardedTypes();
    }

    public IMetadataManifestResource[] GetManifestResources ()
    {
      return _metadataAssembly.GetManifestResources();
    }

    public IImageBodyReader CreateImageBodyReader ()
    {
      return _metadataAssembly.CreateImageBodyReader();
    }

    public IMethodBodyUsagesFinder CreateUsagesFinder ()
    {
      return _metadataAssembly.CreateUsagesFinder();
    }

    public IMetadataAccess MetadataAccess
    {
      get { return _metadataAssembly.MetadataAccess; }
    }

    public AssemblyNameInfo AssemblyName
    {
      get { return _metadataAssembly.AssemblyName; }
    }

    public Guid Mvid
    {
      get { return _metadataAssembly.Mvid; }
    }

    public IEnumerable<AssemblyNameInfo> ReferencedAssembliesNames
    {
      get { return _metadataAssembly.ReferencedAssembliesNames; }
    }

    public FileSystemPath Location
    {
      get { return _metadataAssembly.Location; }
    }

    public IMetadataAssemblyInternals Internals
    {
      get { return _metadataAssembly.Internals; }
    }

    public IMetadataCustomAttribute[] ModuleAttributes
    {
      get { return _metadataAssembly.ModuleAttributes; }
    }
  }
}