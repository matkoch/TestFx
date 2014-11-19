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
using JetBrains.Metadata.Reader.API;

namespace TestFx.ReSharper.Utilities.Metadata
{
  public interface IAttributeDataUtility
  {
    IEnumerable<IMetadataCustomAttribute> GetAttributeDatas<T> (IMetadataEntity entity) where T : Attribute;

    [CanBeNull]
    IMetadataCustomAttribute GetAttributeData<T> (IMetadataEntity entity) where T : Attribute;
  }

  public class AttributeDataUtility : IAttributeDataUtility
  {
    public static IAttributeDataUtility Instance = new AttributeDataUtility();

    public IEnumerable<IMetadataCustomAttribute> GetAttributeDatas<T> (IMetadataEntity entity) where T : Attribute
    {
      return entity.CustomAttributes.Where(x => x.UsedConstructor.DeclaringType.Implements(typeof (T)));
    }

    [CanBeNull]
    public IMetadataCustomAttribute GetAttributeData<T> (IMetadataEntity entity) where T : Attribute
    {
      return GetAttributeDatas<T>(entity).SingleOrDefault();
    }
  }
}