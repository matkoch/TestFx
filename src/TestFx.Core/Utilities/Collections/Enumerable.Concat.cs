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
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace TestFx.Utilities.Collections
{
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public static partial class EnumerableExtensions
  {
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    [DebuggerHidden]
    public static IEnumerable<T> Concat<T> ([CanBeNull] this T obj, IEnumerable<T> enumerable)
    {
      yield return obj;

      foreach (var element in enumerable)
        yield return element;
    }

    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    [DebuggerHidden]
    public static IEnumerable<T> Concat<T> (this IEnumerable<T> enumerable, [CanBeNull] T obj)
    {
      foreach (var element in enumerable)
        yield return element;

      yield return obj;
    }
  }
}