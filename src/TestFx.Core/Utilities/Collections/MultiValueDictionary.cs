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
  public interface IMultiValueDictionary<TKey, TValue> : IDictionary<TKey, ICollection<TValue>>
  {
    void Add (TKey key, TValue value);
    ILookup<TKey, TValue> AsLookup ();
  }

  public class MultiValueDictionary<TKey, TValue> : IMultiValueDictionary<TKey, TValue>
  {
    private readonly Dictionary<TKey, ICollection<TValue>> _dictionary;

    public MultiValueDictionary ()
        : this(EqualityComparer<TKey>.Default)
    {
    }

    public MultiValueDictionary (IEqualityComparer<TKey> comparer)
        : this(new Dictionary<TKey, ICollection<TValue>>(comparer))
    {
    }

    private MultiValueDictionary (Dictionary<TKey, ICollection<TValue>> dictionary)
    {
      _dictionary = dictionary;
    }

    public void Add (TKey key, TValue value)
    {
      ICollection<TValue> values;
      if (TryGetValue(key, out values))
      {
        values.Add(value);
      }
      else
      {
        values = new List<TValue> { value };
        _dictionary.Add(key, values);
      }
    }

    public ILookup<TKey, TValue> AsLookup ()
    {
      return new LookupFromDictionary<TKey, TValue>(this);
    }

    public ICollection<TValue> this [TKey key]
    {
      get { return _dictionary[key]; }
      set { _dictionary[key] = value; }
    }

    public ICollection<TKey> Keys
    {
      get { return _dictionary.Keys; }
    }

    public ICollection<ICollection<TValue>> Values
    {
      get { return _dictionary.Values; }
    }

    public int Count
    {
      get { return _dictionary.Count; }
    }

    public bool IsReadOnly
    {
      get { return ((IDictionary) _dictionary).IsReadOnly; }
    }

    public bool ContainsKey (TKey key)
    {
      return _dictionary.ContainsKey(key);
    }

    public void Add (TKey key, ICollection<TValue> value)
    {
      _dictionary.Add(key, value);
    }

    public bool Remove (TKey key)
    {
      return _dictionary.Remove(key);
    }

    public bool TryGetValue (TKey key, out ICollection<TValue> value)
    {
      return _dictionary.TryGetValue(key, out value);
    }

    public IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator ()
    {
      return _dictionary.GetEnumerator();
    }

    public void Add (KeyValuePair<TKey, ICollection<TValue>> item)
    {
      _dictionary.Add(item.Key, item.Value);
    }

    public void Clear ()
    {
      _dictionary.Clear();
    }

    public bool Contains (KeyValuePair<TKey, ICollection<TValue>> item)
    {
      throw new NotSupportedException();
    }

    public void CopyTo (KeyValuePair<TKey, ICollection<TValue>>[] array, int arrayIndex)
    {
      ((IDictionary) _dictionary).CopyTo(array, arrayIndex);
    }

    public bool Remove (KeyValuePair<TKey, ICollection<TValue>> item)
    {
      throw new NotSupportedException();
    }

    IEnumerator IEnumerable.GetEnumerator ()
    {
      return GetEnumerator();
    }
  }
}