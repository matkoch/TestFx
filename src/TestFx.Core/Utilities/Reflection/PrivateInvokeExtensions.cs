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
using JetBrains.Annotations;

namespace TestFx.Utilities.Reflection
{
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public static class PrivateInvokeExtensions
  {
    [CanBeNull]
    public static object InvokeMethod (this object obj, string methodName, object[] args)
    {
      return PrivateInvokeUtility.Instance.InvokeMethod(obj.GetType(), obj, methodName, args, new Type[0], MemberBindings.Instance);
    }

    [CanBeNull]
    public static T InvokeMethod<T> (this object obj, string methodName, object[] args)
    {
      return (T) InvokeMethod(obj, methodName, args);
    }

    [CanBeNull]
    public static object InvokeGenericMethod (this object obj, string methodName, object[] args, Type[] genericArgs)
    {
      return PrivateInvokeUtility.Instance.InvokeMethod(obj.GetType(), obj, methodName, args, genericArgs, MemberBindings.Instance);
    }

    [CanBeNull]
    public static T InvokeGenericMethod<T> (this object obj, string methodName, object[] args, Type[] genericArgs)
    {
      return (T) InvokeGenericMethod(obj, methodName, args, genericArgs);
    }

    [CanBeNull]
    public static T GetMemberValue<T> (this object obj, string memberName)
    {
      return (T) PrivateInvokeUtility.Instance.GetMemberValue(obj.GetType(), obj, memberName, new object[0], MemberBindings.Instance);
    }

    public static void SetMemberValue (this object obj, string memberName, object value)
    {
      PrivateInvokeUtility.Instance.SetMemberValue(obj.GetType(), obj, memberName, value, new object[0], MemberBindings.Instance);
    }
  }
}