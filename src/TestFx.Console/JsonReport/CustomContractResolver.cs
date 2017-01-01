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
using System.Collections;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TestFx.Console.JsonReport
{
  public class CustomContractResolver : DefaultContractResolver
  {
    protected override string ResolvePropertyName ([NotNull] string propertyName)
    {
      if (propertyName == "Identity")
        return "id";

      return propertyName.ToLower();
    }

    protected override JsonProperty CreateProperty ([NotNull] MemberInfo member, MemberSerialization memberSerialization)
    {
      var property = base.CreateProperty(member, memberSerialization);
      if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType)
          && property.PropertyType != typeof(string))
        property.ShouldSerialize = x =>
        {
          var propertyInfo = member as PropertyInfo;
          if (propertyInfo == null)
            return true;

          var collection = propertyInfo.GetValue(x) as ICollection;
          return collection == null || collection.Count > 0;
        };

      return property;
    }
  }
}