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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TestFx.Utilities.Collections
{
  public class LookupFromDictionary<TKey, TValue> : ILookup<TKey, TValue>
  {
    private struct Grouping : IGrouping<TKey, TValue>
    {
      private readonly TKey _key;
      private readonly ICollection<TValue> _values;

      public Grouping (TKey key, ICollection<TValue> values)
      {
        _key = key;
        _values = values;
      }

      public IEnumerator<TValue> GetEnumerator ()
      {
        return _values.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator ()
      {
        return GetEnumerator();
      }

      public TKey Key
      {
        get { return _key; }
      }
    }

    private struct LookupEnumerator : IEnumerator<IGrouping<TKey, TValue>>, IDictionaryEnumerator
    {
      private readonly IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> _enumerator;

      public LookupEnumerator (IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> enumerator)
      {
        _enumerator = enumerator;
      }

      object IDictionaryEnumerator.Key
      {
        get { return _enumerator.Current.Key; }
      }

      object IDictionaryEnumerator.Value
      {
        get { return _enumerator.Current.Value; }
      }

      DictionaryEntry IDictionaryEnumerator.Entry
      {
        get { return new DictionaryEntry(_enumerator.Current.Key, _enumerator.Current.Value); }
      }

      public IGrouping<TKey, TValue> Current
      {
        get { return new Grouping(_enumerator.Current.Key, _enumerator.Current.Value); }
      }

      object IEnumerator.Current
      {
        get { return Current; }
      }

      public void Dispose ()
      {
        _enumerator.Dispose();
      }

      public bool MoveNext ()
      {
        return _enumerator.MoveNext();
      }

      public void Reset ()
      {
        _enumerator.Reset();
      }
    }

    private readonly IMultiValueDictionary<TKey, TValue> _multiValueDictionary;

    public LookupFromDictionary (IMultiValueDictionary<TKey, TValue> multiValueDictionary)
    {
      _multiValueDictionary = multiValueDictionary;
    }

    public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator ()
    {
      return new LookupEnumerator(_multiValueDictionary.GetEnumerator());
    }

    IEnumerator IEnumerable.GetEnumerator ()
    {
      return GetEnumerator();
    }

    public bool Contains (TKey key)
    {
      return _multiValueDictionary.ContainsKey(key);
    }

    public int Count
    {
      get { return _multiValueDictionary.Count; }
    }

    public IEnumerable<TValue> this [TKey key]
    {
      get { return _multiValueDictionary[key]; }
    }
  }
}