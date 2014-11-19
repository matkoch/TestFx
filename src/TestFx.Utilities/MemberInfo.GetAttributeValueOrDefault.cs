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
using System.Reflection;
using TestFx.Utilities.Reflection;

namespace TestFx.Utilities
{
  public static partial class MemberInfoExtensions
  {
    public static TValue GetAttributeValueOrDefault<TAttribute, TValue> (
        this MemberInfo member,
        Func<TAttribute, TValue> selector,
        Func<TValue> defaultProvider = null)
        where TAttribute : Attribute
    {
      defaultProvider = defaultProvider ?? (() => default(TValue));
      var attribute = member.GetAttribute<TAttribute>();
      return attribute != null ? selector(attribute) : defaultProvider();
    }
  }
}