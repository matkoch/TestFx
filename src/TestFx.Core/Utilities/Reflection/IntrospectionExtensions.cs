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
using System.Reflection;
using TestFx.Utilities.Introspection;

namespace TestFx.Utilities.Reflection
{
  public static class IntrospectionExtensions
  {
    public static CommonType ToCommon (this Type type)
    {
      return IntrospectionUtility.Instance.GetCommonType(type);
    }

    public static CommonAttribute ToCommon (this CustomAttributeData customAttributeData)
    {
      return IntrospectionUtility.Instance.GetCommonAttribute(customAttributeData);
    }
  }
}