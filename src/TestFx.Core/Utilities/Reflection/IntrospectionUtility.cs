// Copyright 2015, 2014 Matthias Koch
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
using TestFx.Utilities.Collections;
using TestFx.Utilities.Introspection;

namespace TestFx.Utilities.Reflection
{
  public interface IIntrospectionUtility
  {
    CommonType GetCommonType (Type type);

    CommonAttribute GetCommonAttribute (CustomAttributeData customAttributeData);
  }

  public class IntrospectionUtility : IIntrospectionUtility
  {
    public static IIntrospectionUtility Instance = new IntrospectionUtility();

    public CommonType GetCommonType (Type type)
    {
      var implementedTypes = type.DescendantsAndSelf(x => x.BaseType).Concat(type.GetInterfaces());
      return new CommonType(type.Name, type.FullName, implementedTypes.Select(x => x.FullName));
    }

    public CommonAttribute GetCommonAttribute (CustomAttributeData customAttributeData)
    {
      var positionalArguments = customAttributeData.ConstructorArguments.Select(GetCommonPositionalArgument);
      var namedArguments = customAttributeData.NamedArguments.NotNull().Select(GetCommonNamedArgument);

      return new CommonAttribute(GetCommonType(customAttributeData.Constructor.DeclaringType.NotNull()), positionalArguments, namedArguments);
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
        return ((Type) value).ToCommon();

      return value;
    }
  }
}