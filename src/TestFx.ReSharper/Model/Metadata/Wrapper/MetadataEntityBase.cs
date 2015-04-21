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
using JetBrains.Metadata.Access;
using JetBrains.Metadata.Reader.API;

namespace TestFx.ReSharper.Model.Metadata.Wrapper
{
  public abstract class MetadataEntityBase : IMetadataEntity
  {
    private readonly IMetadataEntity _metadataEntity;

    protected MetadataEntityBase (IMetadataEntity metadataEntity)
    {
      _metadataEntity = metadataEntity;
    }

    public bool Equals (IMetadataEntity other)
    {
      return _metadataEntity.Equals(other);
    }

    public IList<IMetadataCustomAttribute> GetCustomAttributes (string attributeClassFullyQualifiedName)
    {
      return _metadataEntity.GetCustomAttributes(attributeClassFullyQualifiedName);
    }

    public bool HasCustomAttribute (string attributeClassFullyQualifiedName)
    {
      return _metadataEntity.HasCustomAttribute(attributeClassFullyQualifiedName);
    }

    public IMetadataAssembly Assembly
    {
      get { return _metadataEntity.Assembly; }
    }

    public MetadataToken Token
    {
      get { return _metadataEntity.Token; }
    }

    public IMetadataCustomAttribute[] CustomAttributes
    {
      get { return _metadataEntity.CustomAttributes; }
    }

    public MetadataTypeReference[] CustomAttributesTypeNames
    {
      get { return _metadataEntity.CustomAttributesTypeNames; }
    }

    public bool IsResolved
    {
      get { return _metadataEntity.IsResolved; }
    }
  }
}