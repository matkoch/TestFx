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
    public static IDictionary<TKey, TValue> ToDictionary<T, TKey, TValue> (
        this IEnumerable<T> enumerable,
        [InstantHandle] Func<T, TKey> keySelector,
        [InstantHandle] Func<T, TValue> valueSelector,
        IEqualityComparer<TKey> comparer = null,
        Func<ArgumentException, TKey, Exception> exceptionFactory = null)
    {
      var list = enumerable.ToList();
      var dictionary = new Dictionary<TKey, TValue>(list.Count, comparer);

      foreach (var item in list)
      {
        var key = keySelector(item);
        try
        {
          dictionary.Add(key, valueSelector(item));
        }
        catch (ArgumentException exception)
        {
          exceptionFactory = exceptionFactory ?? ((ex, k) => ex);
          throw exceptionFactory(exception, key);
        }
      }

      return dictionary;
    }
  }
}