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
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
// ReSharper disable once CheckNamespace

namespace TestFx.Utilities
{
  public static partial class EnumerableExtensions
  {
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    [DebuggerHidden]
    public static IEnumerable<T> Follow<T> (
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

      foreach (var nextOrDescendant in next.Follow(selector, traverse))
        yield return nextOrDescendant;
    }
  }
}