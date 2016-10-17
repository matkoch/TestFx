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
using TestFx.Utilities.Introspection;

namespace TestFx.ReSharper.Extensions.MSpec
{
  public class MSpecUtility
  {
    public const string ItDelegateFullName = "Machine.Specifications.It";
    public const string BehavesLikeDelegateFullName = "Machine.Specifications.Behaves_like`1";
    public const string BehaviorsAttributeFullName = "Machine.Specifications.BehaviorsAttribute";
    public const string SubjectAttributeFullName = "Machine.Specifications.SubjectAttribute";

    public static string CreateText (CommonType concernType, [CanBeNull] CommonType subjectType, [CanBeNull] string subjectText)
    {
      var subject = subjectType == null
        ? subjectText
        : subjectText == null
          ? subjectType.Name
          : $"{subjectType.Name} {subjectText}";

      return $"{subject}, {concernType.Name.Replace(oldChar: '_', newChar: ' ')}";
    }
  }
}