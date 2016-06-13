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
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;
using TestFx.Utilities.Introspection;
using TestFx.Utilities.Reflection;

namespace TestFx.ReSharper.Utilities.Psi
{
  public interface IIntrospectionUtility
  {
    CommonType GetCommonType (IType type);
    CommonType GetCommonType (ITypeElement type);

    CommonMember GetCommonMember (ITypeOwner member);
    CommonMember GetCommonMember (IParametersOwner member);

    CommonAttribute GetCommonAttribute (IAttributeInstance attributeInstance);
  }

  internal class IntrospectionUtility : IIntrospectionUtility
  {
    public static IIntrospectionUtility Instance = new IntrospectionUtility();

    public CommonType GetCommonType (IType type)
    {
      // TODO: type can be ??? if not resolvable. Compare to TypeUtility.GetImplementedTypes
      return GetCommonType(((IDeclaredType) type).GetTypeElement().NotNull());
    }

    public CommonType GetCommonType (ITypeElement type)
    {
      var clrTypeName = type.GetClrName();
      return new CommonType(clrTypeName.ShortName, clrTypeName.FullName, type.GetImplementedTypes().Select(x => x.GetClrName().FullName));
    }

    public CommonMember GetCommonMember (ITypeOwner member)
    {
      return new CommonMember(member.ShortName, member.Type.ToCommon());
    }

    public CommonMember GetCommonMember (IParametersOwner member)
    {
      return new CommonMember(member.ShortName, member.ReturnType.ToCommon());
    }

    public CommonAttribute GetCommonAttribute (IAttributeInstance attributeInstance)
    {
      var type = GetCommonType(attributeInstance.GetAttributeType());
      var positionalArguments = attributeInstance.PositionParameters().Select(GetPositionalArgument).WhereNotNull();
      var namedArguments = attributeInstance.NamedParameters().Select(GetNamedArguments).WhereNotNull();

      return new CommonAttribute(type, positionalArguments, namedArguments);
    }

    [CanBeNull]
    private CommonPositionalArgument GetPositionalArgument (AttributeValue argument, int position)
    {
      if (argument.IsBadValue)
        return null;

      var typeAndValue = GetTypeAndValue(argument);
      return new CommonPositionalArgument(position, typeAndValue.Item1, typeAndValue.Item2);
    }

    [CanBeNull]
    private CommonNamedArgument GetNamedArguments (Pair<string, AttributeValue> argument)
    {
      if (argument.Second.IsBadValue)
        return null;

      var typeAndValue = GetTypeAndValue(argument.Second);
      return new CommonNamedArgument(argument.First, typeAndValue.Item1, typeAndValue.Item2);
    }

    private Tuple<CommonType, object> GetTypeAndValue (AttributeValue argument)
    {
      if (argument.IsType)
        return Tuple.Create(typeof (Type).ToCommon(), ConvertToCommon(argument, x => GetCommonType(x.TypeValue.NotNull())));

      if (argument.IsArray)
        throw new Exception(); // Use GetScalarType
      //return Tuple.CreateTestDeclarationProvider(GetCommonType(argument.ArrayType), (object) argument.ArrayValue.Select(GetTypeAndValue).Select(x => x.Item2).ToArray());

      // TODO: ConvertToCommon required?
      return Tuple.Create(GetCommonType(argument.ConstantValue.Type.NotNull()), ConvertToCommon(argument, x => x.ConstantValue.Value));
    }

    // TODO: ConvertToCommon handling bad values
    private object ConvertToCommon (AttributeValue argument, Func<AttributeValue, object> selector)
    {
      return !argument.IsBadValue ? selector(argument) : "???";
    }
  }
}