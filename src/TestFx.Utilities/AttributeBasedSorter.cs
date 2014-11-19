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
using System.Linq;
using TestFx.Utilities.Reflection;

namespace TestFx.Utilities
{
  public interface IAttributeBasedSorter
  {
    IEnumerable<T> Sort<T> (IEnumerable<T> items, bool throwIfOrderIsUndefined = false)
        where T : class;

    IEnumerable<T> Sort<T, TKey> (IEnumerable<T> items, Func<T, TKey> keyProvider, bool throwIfOrderIsUndefined = false)
        where T : class;

    IEnumerable<T> Sort<T> (IEnumerable<T> items, Func<T, Type> keyProvider, bool throwIfOrderIsUndefined = false)
        where T : class;
  }

  public class AttributeBasedSorter : IAttributeBasedSorter
  {
    public IEnumerable<T> Sort<T> (IEnumerable<T> items, bool throwIfOrderIsUndefined = false)
        where T : class
    {
      return Sort(items, x => x, throwIfOrderIsUndefined);
    }
    
    public IEnumerable<T> Sort<T, TKey> (IEnumerable<T> items, Func<T, TKey> keyProvider, bool throwIfOrderIsUndefined = false) where T : class
    {
      return items.SortTopologically((a, b) => HasAttributeDependency(keyProvider(a), keyProvider(b)), throwIfOrderIsUndefined);
    }

    public IEnumerable<T> Sort<T> (IEnumerable<T> items, Func<T, Type> keyProvider, bool throwIfOrderIsUndefined = false) where T : class
    {
      return items.SortTopologically((a, b) => HasAttributeDependency(keyProvider(a), keyProvider(b)), throwIfOrderIsUndefined);
    }

    private bool HasAttributeDependency<T> (T obj, T other)
    {
      var objType = obj.GetType();
      var otherType = other.GetType();
      return HasAttributeDependency(objType, otherType);
    }

    private static bool HasAttributeDependency (Type objType, Type otherType)
    {
      var beforeAttributes = objType.GetAttributes<BeforeAttribute>();
      var afterAttributes = otherType.GetAttributes<AfterAttribute>();

      return beforeAttributes.Any(x => x.Type.IsAssignableFrom(otherType)) ||
             afterAttributes.Any(x => x.Type.IsAssignableFrom(objType));
    }
  }
}