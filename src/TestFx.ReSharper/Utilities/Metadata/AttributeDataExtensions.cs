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
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;

namespace TestFx.ReSharper.Utilities.Metadata
{
  public static class AttributeDataExtensions
  {
    public static IEnumerable<IMetadataCustomAttribute> GetAttributeDatas<T> (this IMetadataEntity entity) where T : Attribute
    {
      return entity.GetAttributeDatas(typeof(T).FullName);
    }

    [CanBeNull]
    public static IMetadataCustomAttribute GetAttributeData<T> (this IMetadataEntity entity) where T : Attribute
    {
      return entity.GetAttributeData(typeof(T).FullName);
    }

    public static IEnumerable<IMetadataCustomAttribute> GetAttributeDatas (this IMetadataEntity entity, string attributeType)
    {
      return AttributeDataUtility.Instance.GetAttributeDatas(entity, attributeType);
    }

    [CanBeNull]
    public static IMetadataCustomAttribute GetAttributeData (this IMetadataEntity entity, string attributeType)
    {
      return AttributeDataUtility.Instance.GetAttributeData(entity, attributeType);
    }
  }
}