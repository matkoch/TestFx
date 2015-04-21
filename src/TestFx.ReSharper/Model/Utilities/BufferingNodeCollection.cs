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
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.Model.Utilities
{
  public abstract class BufferingNodeCollection<TSource, TDestination> : IEnumerable<TDestination>
      where TDestination : class
  {
    private readonly IEnumerable<TSource> _sources;
    private readonly Func<TSource, TDestination> _converter;
    private readonly Func<bool> _notInterrupted;

    private IList<TDestination> _collectionBuffer;

    protected BufferingNodeCollection (IEnumerable<TSource> sources, Func<TSource, TDestination> converter, Func<bool> notInterrupted)
    {
      _sources = sources;
      _converter = converter;
      _notInterrupted = notInterrupted;
    }

    protected abstract bool IsInvalid (TDestination item);

    public virtual IEnumerator<TDestination> GetEnumerator ()
    {
      if (_collectionBuffer == null || _collectionBuffer.Any(IsInvalid))
        _collectionBuffer = _sources.TakeWhile(_notInterrupted).Select(_converter).WhereNotNull().ToList();
      return _collectionBuffer.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator ()
    {
      return GetEnumerator();
    }
  }
}