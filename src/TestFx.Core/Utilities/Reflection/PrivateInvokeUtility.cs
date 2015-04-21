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
using JetBrains.Annotations;
using TestFx.Utilities.Collections;

namespace TestFx.Utilities.Reflection
{
  public interface IPrivateInvokeUtility
  {
    [CanBeNull]
    object InvokeMethod (Type type, object instance, string methodName, object[] args, Type[] typeArgs, BindingFlags bindingFlags);

    [CanBeNull]
    object GetMemberValue (Type type, object instance, string memberName, object[] args, BindingFlags bindingFlags);

    void SetMemberValue (Type type, object instance, string memberName, object value, object[] args, BindingFlags bindingFlags);
  }

  public class PrivateInvokeUtility : IPrivateInvokeUtility
  {
    public static IPrivateInvokeUtility Instance = new PrivateInvokeUtility();

    [CanBeNull]
    public object InvokeMethod (Type type, object instance, string methodName, object[] args, Type[] typeArgs, BindingFlags bindingFlags)
    {
      var method = GetMethod(type, methodName, args.Select(x => x.GetType()).ToArray(), bindingFlags);
      if (typeArgs.Length != 0)
        method = method.MakeGenericMethod(typeArgs);
      return method.Invoke(instance, args);
    }

    [CanBeNull]
    public object GetMemberValue (Type type, object instance, string memberName, object[] args, BindingFlags bindingFlags)
    {
      try
      {
        var field = GetField(type, memberName, bindingFlags);
        return field.GetValue(instance);
      }
      catch (MissingFieldException)
      {
        var property = GetProperty(type, memberName, bindingFlags);
        return property.GetValue(instance, args);
      }
    }

    public void SetMemberValue (Type type, object instance, string memberName, object value, object[] args, BindingFlags bindingFlags)
    {
      try
      {
        var field = GetField(type, memberName, bindingFlags);
        field.SetValue(instance, value);
      }
      catch (MissingFieldException)
      {
        var property = GetProperty(type, memberName, bindingFlags);
        property.SetValue(instance, value, args);
      }
    }

    private MethodInfo GetMethod (Type type, string methodName, Type[] types, BindingFlags bindingFlags)
    {
      return GetMember(
          t => t.GetMethod(methodName, bindingFlags, null, types, new ParameterModifier[0]),
          type,
          () => new MissingMethodException(type.FullName, methodName));
    }

    private FieldInfo GetField (Type type, string fieldName, BindingFlags bindingFlags)
    {
      return GetMember(
          t => t.GetField(fieldName, bindingFlags),
          type,
          () => new MissingFieldException(type.FullName, fieldName));
    }

    private PropertyInfo GetProperty (Type type, string propertyName, BindingFlags bindingFlags)
    {
      return GetMember(
          t => t.GetProperty(propertyName, bindingFlags),
          type,
          () => new MissingMemberException(type.FullName, propertyName));
    }

    private T GetMember<T> (Func<Type, T> memberSelector, Type type, Func<Exception> exceptionProvider)
        where T : MemberInfo
    {
      var member = type.DescendantsAndSelf(x => x.BaseType).Select(memberSelector).WhereNotNull().FirstOrDefault();
      if (member != null)
        return member;

      throw exceptionProvider();
    }
  }
}