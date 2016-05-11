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
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.Util;

namespace TestFx.ReSharper.Utilities.Metadata
{
  public interface IAttributeDataUtility
  {
    IEnumerable<IMetadataCustomAttribute> GetAttributeDatas (IMetadataEntity entity, string attributeType);

    [CanBeNull]
    IMetadataCustomAttribute GetAttributeData (IMetadataEntity entity, string attributeType);
  }

  internal class AttributeDataUtility : IAttributeDataUtility
  {
    public static IAttributeDataUtility Instance = new AttributeDataUtility();

    public IEnumerable<IMetadataCustomAttribute> GetAttributeDatas (IMetadataEntity entity, string attributeType)
    {
      return entity.CustomAttributes.Where(x => x.UsedConstructor.NotNull().DeclaringType.Implements(attributeType));
    }

    [CanBeNull]
    public IMetadataCustomAttribute GetAttributeData (IMetadataEntity entity, string attributeType)
    {
      return GetAttributeDatas(entity, attributeType).SingleOrDefault();
    }
  }
}