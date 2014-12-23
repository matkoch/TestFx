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
using System.Linq;
using System.Reflection;
using TestFx.Utilities.Introspection;

namespace TestFx.Utilities.Reflection
{
  public interface IIntrospectionUtility
  {
    CommonType GetCommonType (Type type);

    CommonMemberInfo GetCommonMemberInfo (MemberInfo memberInfo);
    CommonFieldInfo GetCommonFieldInfo (FieldInfo fieldInfo);
    CommonConstructorInfo GetCommonConstructorInfo (ConstructorInfo constructorInfo);
    CommonPropertyInfo GetCommonPropertyInfo (PropertyInfo propertyInfo);
    CommonMethodInfo GetCommonMethodInfo (MethodInfo methodInfo);

    CommonAttribute GetCommonAttribute (CustomAttributeData customAttributeData);
  }

  public class IntrospectionUtility : IIntrospectionUtility
  {
    public static IIntrospectionUtility Instance = new IntrospectionUtility();

    public CommonType GetCommonType (Type type)
    {
      var implementedTypes = type.Follow(x => x.BaseType).Concat(type.GetInterfaces());
      return new CommonType(type.Name, type.FullName, implementedTypes.Select(x => x.FullName));
    }

    public CommonMemberInfo GetCommonMemberInfo (MemberInfo memberInfo)
    {
      if (memberInfo is FieldInfo)
        return GetCommonFieldInfo(memberInfo.To<FieldInfo>());
      if (memberInfo is ConstructorInfo)
        return GetCommonConstructorInfo(memberInfo.To<ConstructorInfo>());
      if (memberInfo is PropertyInfo)
        return GetCommonPropertyInfo(memberInfo.To<PropertyInfo>());
      if (memberInfo is MethodInfo)
        return GetCommonMethodInfo(memberInfo.To<MethodInfo>());

      throw new Exception();
    }

    public CommonFieldInfo GetCommonFieldInfo (FieldInfo fieldInfo)
    {
      return new CommonFieldInfo(
          GetCommonType(fieldInfo.DeclaringType),
          fieldInfo.Name,
          GetCommonType(fieldInfo.FieldType),
          fieldInfo.IsStatic);
    }

    public CommonConstructorInfo GetCommonConstructorInfo (ConstructorInfo constructorInfo)
    {
      return new CommonConstructorInfo(
          GetCommonType(constructorInfo.DeclaringType),
          constructorInfo.Name,
          GetCommonType(typeof (void)),
          constructorInfo.IsStatic);
    }

    public CommonPropertyInfo GetCommonPropertyInfo (PropertyInfo propertyInfo)
    {
      return new CommonPropertyInfo(
          GetCommonType(propertyInfo.DeclaringType),
          propertyInfo.Name,
          GetCommonType(propertyInfo.PropertyType),
          propertyInfo.GetGetMethod(true).IsStatic);
    }

    public CommonMethodInfo GetCommonMethodInfo (MethodInfo methodInfo)
    {
      return new CommonMethodInfo(
          GetCommonType(methodInfo.DeclaringType),
          methodInfo.Name,
          GetCommonType(methodInfo.ReturnType),
          methodInfo.IsStatic,
          methodInfo.IsExtensionMethod());
    }

    public CommonAttribute GetCommonAttribute (CustomAttributeData customAttributeData)
    {
      var positionalArguments = customAttributeData.ConstructorArguments.Select(GetCommonPositionalArgument);
      var namedArguments = customAttributeData.NamedArguments.AssertNotNull().Select(GetCommonNamedArgument);

      return new CommonAttribute(GetCommonType(customAttributeData.Constructor.DeclaringType), positionalArguments, namedArguments);
    }

    private CommonNamedArgument GetCommonNamedArgument (CustomAttributeNamedArgument argument)
    {
      return new CommonNamedArgument(argument.MemberInfo.Name, GetCommonType(argument.TypedValue.ArgumentType), GetArgumentValue(argument.TypedValue));
    }

    private CommonPositionalArgument GetCommonPositionalArgument (CustomAttributeTypedArgument argument, int position)
    {
      return new CommonPositionalArgument(position, GetCommonType(argument.ArgumentType), GetArgumentValue(argument));
    }

    private object GetArgumentValue (CustomAttributeTypedArgument argument)
    {
      var value = argument.GetValue();

      var valueAsTypeArray = value as Type[];
      if (valueAsTypeArray != null)
        return valueAsTypeArray.Select(GetCommonType).ToArray();

      if (value is Type)
        return value.To<Type>().ToCommon();

      return value;
    }
  }
}