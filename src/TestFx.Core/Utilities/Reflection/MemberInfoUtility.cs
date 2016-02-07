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
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TestFx.Utilities.Collections;

namespace TestFx.Utilities.Reflection
{
  public interface IMemberInfoUtility
  {
    [CanBeNull]
    PropertyInfo GetRelatedPropertyInfo (MethodInfo methodInfo);

    [CanBeNull]
    EventInfo GetRelatedEventInfo (MethodInfo methodInfo);

    bool IsExtensionMethod (MethodInfo methodInfo);
  }

  public class MemberInfoUtility : IMemberInfoUtility
  {
    public static IMemberInfoUtility Instance = new MemberInfoUtility();

    [CanBeNull]
    public PropertyInfo GetRelatedPropertyInfo (MethodInfo methodInfo)
    {
      var methodName = methodInfo.Name;
      if (!methodName.StartsWith("get_") && !methodName.StartsWith("set_"))
        return null;

      var types = methodInfo.DeclaringType.NotNull().DescendantsAndSelf(x => x.BaseType);
      var properties = types.SelectMany(x => x.GetProperties(MemberBindings.All)).Where(x => methodName.EndsWith(x.Name));

      return
          properties.FirstOrDefault(
              x => EqualsBaseDefinition(methodInfo, x.GetGetMethod(true)) || EqualsBaseDefinition(methodInfo, x.GetSetMethod(true)));
    }

    [CanBeNull]
    public EventInfo GetRelatedEventInfo (MethodInfo methodInfo)
    {
      var methodName = methodInfo.Name;
      if (!methodName.StartsWith("add_") && !methodName.StartsWith("remove_"))
        return null;

      var types = methodInfo.DeclaringType.NotNull().DescendantsAndSelf(x => x.BaseType);
      var events = types.SelectMany(x => x.GetEvents(MemberBindings.All)).Where(x => methodName.EndsWith(x.Name));

      return
          events.FirstOrDefault(
              x => EqualsBaseDefinition(methodInfo, x.GetAddMethod(true)) || EqualsBaseDefinition(methodInfo, x.GetRemoveMethod(true)));
    }

    public bool IsExtensionMethod (MethodInfo methodInfo)
    {
      return methodInfo.GetAttribute<ExtensionAttribute>() != null;
    }

    // TODO: better EqualsWithoutReflectedType
    private bool EqualsBaseDefinition (MethodInfo method1, MethodInfo method2)
    {
      return method1 != null && method2 != null && method1.GetBaseDefinition() == method2.GetBaseDefinition();
    }
  }
}