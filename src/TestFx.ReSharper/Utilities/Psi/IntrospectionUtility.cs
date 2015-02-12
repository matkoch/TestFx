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

extern alias utils;

using System;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;
using TestFx.Utilities;
using TestFx.Utilities.Introspection;
using TestFx.Utilities.Reflection;
using TestFxUtils = utils::TestFx.Utilities;

namespace TestFx.ReSharper.Utilities.Psi
{
  public interface IIntrospectionUtility
  {
    CommonType GetCommonType (IType type);
    CommonType GetCommonType (ITypeElement type);

    CommonMemberInfo GetCommonMemberInfo (ITypeMember typeMember);
    CommonFieldInfo GetCommonFieldInfo (IField field);
    CommonConstructorInfo GetCommonConstructorInfo (IConstructor constructor);
    CommonPropertyInfo GetCommonPropertyInfo (IProperty property);
    CommonMethodInfo GetCommonMethodInfo (IMethod method);

    CommonAttribute GetCommonAttribute (IAttributeInstance attributeInstance);
  }

  public class IntrospectionUtility : IIntrospectionUtility
  {
    public static IIntrospectionUtility Instance = new IntrospectionUtility();

    public CommonType GetCommonType (IType type)
    {
      // TODO: type can be ??? if not resolvable. Compare to TypeUtility.GetImplementedTypes
      return GetCommonType(type.To<IDeclaredType>().GetTypeElement());
    }

    public CommonType GetCommonType (ITypeElement type)
    {
      var clrTypeName = type.GetClrName();
      return new CommonType(clrTypeName.ShortName, clrTypeName.FullName, type.GetImplementedTypes().Select(x => x.GetClrName().FullName));
    }

    public CommonMemberInfo GetCommonMemberInfo (ITypeMember typeMember)
    {
      if (typeMember is IField)
        return GetCommonFieldInfo(typeMember.To<IField>());
      if (typeMember is IConstructor)
        return GetCommonConstructorInfo(typeMember.To<IConstructor>());
      if (typeMember is IProperty)
        return GetCommonPropertyInfo(typeMember.To<IProperty>());
      if (typeMember is IMethod)
        return GetCommonMethodInfo(typeMember.To<IMethod>());

      throw new Exception();
    }

    public CommonAttribute GetCommonAttribute (IAttributeInstance attributeInstance)
    {
      var type = GetCommonType(attributeInstance.GetAttributeType());
      var positionalArguments = EnumerableExtensions.WhereNotNull(attributeInstance.PositionParameters().Select(GetPositionalArgument));
      var namedArguments = CollectionUtil.WhereNotNull(attributeInstance.NamedParameters().Select(GetNamedArguments));

      return new CommonAttribute(type, positionalArguments, namedArguments);
    }

    public CommonFieldInfo GetCommonFieldInfo (IField field)
    {
      return new CommonFieldInfo(field.GetContainingType().ToCommon(), field.ShortName, field.Type.ToCommon(), field.IsStatic);
    }

    public CommonConstructorInfo GetCommonConstructorInfo (IConstructor constructor)
    {
      return new CommonConstructorInfo(
          constructor.GetContainingType().ToCommon(),
          constructor.ShortName,
          constructor.ReturnType.ToCommon(),
          constructor.IsStatic);
    }

    public CommonPropertyInfo GetCommonPropertyInfo (IProperty property)
    {
      return new CommonPropertyInfo(property.GetContainingType().ToCommon(), property.ShortName, property.Type.ToCommon(), property.IsStatic);
    }

    public CommonMethodInfo GetCommonMethodInfo (IMethod method)
    {
      return new CommonMethodInfo(
          method.GetContainingType().ToCommon(),
          method.ShortName,
          method.ReturnType.ToCommon(),
          method.IsStatic,
          method.IsExtensionMethod);
    }

    private CommonPositionalArgument GetPositionalArgument (AttributeValue argument, int position)
    {
      if (argument.IsBadValue)
        return null;

      var typeAndValue = GetTypeAndValue(argument);
      return new CommonPositionalArgument(position, typeAndValue.Item1, typeAndValue.Item2);
    }

    private CommonNamedArgument GetNamedArguments (Pair<string, AttributeValue> argument)
    {
      if (argument.Second.IsBadValue)
        return null;

      var typeAndValue = GetTypeAndValue(argument.Second);
      return new CommonNamedArgument(argument.First, typeAndValue.Item1, typeAndValue.Item2);
    }

    private TestFxUtils.Tuple<CommonType, object> GetTypeAndValue (AttributeValue argument)
    {
      if (argument.IsType)
        return TestFxUtils.Tuple.Create(typeof (Type).ToCommon(), ConvertToCommon(argument, x => GetCommonType(x.TypeValue)));

      if (argument.IsArray)
        throw new Exception(); // Use GetScalarType
      //return Tuple.Create(GetCommonType(argument.ArrayType), (object) argument.ArrayValue.Select(GetTypeAndValue).Select(x => x.Item2).ToArray());

      // TODO: ConvertToCommon required?
      return TestFxUtils.Tuple.Create(GetCommonType(argument.ConstantValue.Type), ConvertToCommon(argument, x => x.ConstantValue.Value));
    }

    // TODO: ConvertToCommon handling bad values
    private object ConvertToCommon (AttributeValue argument, Func<AttributeValue, object> selector)
    {
      return !argument.IsBadValue ? selector(argument) : "???";
    }
  }
}