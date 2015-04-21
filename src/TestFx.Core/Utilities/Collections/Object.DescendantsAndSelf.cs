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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace TestFx.Utilities.Collections
{
  public static partial class EnumerableExtensions
  {
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    [DebuggerHidden]
    public static IEnumerable<T> DescendantsAndSelf<T> (
        this T obj,
        Func<T, T> selector,
        [CanBeNull] Func<T, bool> traverse = null)
    {
      yield return obj;

      if (traverse != null && !traverse(obj))
        yield break;

      var next = selector(obj);
      if (traverse == null && Equals(next, default(T)))
        yield break;

      foreach (var nextOrDescendant in next.DescendantsAndSelf(selector, traverse))
        yield return nextOrDescendant;
    }

    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    [DebuggerHidden]
    public static IEnumerable<T> DescendantsAndSelf<T> (
        this T obj,
        Func<T, IEnumerable<T>> selector,
        [CanBeNull] Func<T, bool> traverse = null)
    {
      yield return obj;

      foreach (var child in selector(obj).Where(x => traverse == null || traverse(x)))
        foreach (var childOrDescendant in child.DescendantsAndSelf(selector, traverse))
          yield return childOrDescendant;
    }
  }
}