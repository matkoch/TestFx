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
using JetBrains.ReSharper.Psi;
using TestFx.Utilities.Introspection;

namespace TestFx.ReSharper.Utilities.Psi
{
  public static class IntrospectionExtensions
  {
    public static CommonType ToCommon (this IType type)
    {
      return IntrospectionUtility.Instance.GetCommonType(type);
    }

    public static CommonType ToCommon (this ITypeElement type)
    {
      return IntrospectionUtility.Instance.GetCommonType(type);
    }

    public static CommonMemberInfo ToCommon (this ITypeMember typeMember)
    {
      return IntrospectionUtility.Instance.GetCommonMemberInfo(typeMember);
    }

    public static CommonFieldInfo ToCommon (this IField fieldInfo)
    {
      return IntrospectionUtility.Instance.GetCommonFieldInfo(fieldInfo);
    }

    public static CommonConstructorInfo ToCommon (this IConstructor constructorInfo)
    {
      return IntrospectionUtility.Instance.GetCommonConstructorInfo(constructorInfo);
    }

    public static CommonPropertyInfo ToCommon (this IProperty propertyInfo)
    {
      return IntrospectionUtility.Instance.GetCommonPropertyInfo(propertyInfo);
    }

    public static CommonMethodInfo ToCommon (this IMethod methodInfo)
    {
      return IntrospectionUtility.Instance.GetCommonMethodInfo(methodInfo);
    }

    public static CommonAttribute ToCommon (this IAttributeInstance attributeInstance)
    {
      return IntrospectionUtility.Instance.GetCommonAttribute(attributeInstance);
    }
  }
}