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
using System.Reflection;
using JetBrains.Annotations;

namespace TestFx.Utilities.Reflection
{
  [PublicAPI ("Used by extensions")]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public static class MemberInfoExtensions
  {
    [CanBeNull]
    public static PropertyInfo GetPropertyInfo (this MethodInfo methodInfo)
    {
      return MemberInfoUtility.Instance.GetRelatedPropertyInfo(methodInfo);
    }

    [CanBeNull]
    public static EventInfo GetEventInfo (this MethodInfo methodInfo)
    {
      return MemberInfoUtility.Instance.GetRelatedEventInfo(methodInfo);
    }

    public static bool IsExtensionMethod (this MethodInfo methodInfo)
    {
      return MemberInfoUtility.Instance.IsExtensionMethod(methodInfo);
    }
  }
}