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
using System.Diagnostics;
using System.Linq;
using JetBrains.Metadata.Reader.API;
using JetBrains.Util;
using TestFx.Utilities.Introspection;

namespace TestFx.ReSharper.Utilities.Metadata
{
  public interface IIntrospectionUtility
  {
    CommonType GetCommonType (IMetadataTypeInfo type);
    CommonType GetCommonType (IMetadataType type);

    CommonAttribute GetCommonAttribute (IMetadataCustomAttribute metadataCustomAttribute);
  }

  public class IntrospectionUtility : IIntrospectionUtility
  {
    public static IIntrospectionUtility Instance = new IntrospectionUtility();

    public CommonType GetCommonType (IMetadataTypeInfo type)
    {
      return new CommonType(type.Name, type.FullyQualifiedName, type.GetImplementedTypes().Select(x => x.FullyQualifiedName));
    }

    public CommonType GetCommonType (IMetadataType type)
    {
      if (type is IMetadataArrayType)
        return GetCommonType(((IMetadataArrayType) type).TypeInfo);
      if (type is IMetadataClassType)
        return GetCommonType(((IMetadataClassType) type).Type);

      Trace.Fail(string.Format("Instance of type {0} cannot be converted to CommonType", type));
      throw new Exception();
    }

    public CommonAttribute GetCommonAttribute (IMetadataCustomAttribute metadataCustomAttribute)
    {
      var type = GetCommonType(metadataCustomAttribute.UsedConstructor.DeclaringType);
      var positionalArguments = metadataCustomAttribute.ConstructorArguments.Select(GetPositionalArgument).WhereNotNull();
      var namedArguments = metadataCustomAttribute.InitializedFields.Select(GetNamedArgument)
          .Concat(metadataCustomAttribute.InitializedProperties.Select(GetNamedArgument)).WhereNotNull();

      return new CommonAttribute(type, positionalArguments, namedArguments);
    }

    private CommonPositionalArgument GetPositionalArgument (MetadataAttributeValue argument, int position)
    {
      if (argument.IsBadValue())
        return null;

      return new CommonPositionalArgument(position, GetCommonType(argument.Type), GetValue(argument));
    }

    private CommonNamedArgument GetNamedArgument (IMetadataCustomAttributeFieldInitialization argument)
    {
      return new CommonNamedArgument(argument.Field.Name, GetCommonType(argument.Field.Type), GetValue(argument.Value));
    }

    private CommonNamedArgument GetNamedArgument (IMetadataCustomAttributePropertyInitialization argument)
    {
      return new CommonNamedArgument(argument.Property.Name, GetCommonType(argument.Property.Type), GetValue(argument.Value));
    }

    private object GetValue (MetadataAttributeValue argument)
    {
      Trace.Assert(!argument.IsBadValue(), "Values in MetadataCustomAttributes can be bad.");

      if (argument.ValuesArray != null)
        return argument.ValuesArray.Select(GetValue).ToArray();
      if (argument.Value is IMetadataType)
        return GetCommonType(((IMetadataType) argument.Value));
      return argument.Value;
    }
  }
}