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
using JetBrains.Annotations;

namespace TestFx.Utilities
{
  public static class Tuple
  {
    public static Tuple<T1, T2> Create<T1, T2> ([CanBeNull] T1 item1, [CanBeNull] T2 item2)
    {
      return new Tuple<T1, T2>(item1, item2);
    }
  }

  public struct Tuple<T1, T2>
  {
    private readonly T1 _item1;
    private readonly T2 _item2;

    public Tuple ([CanBeNull] T1 item1, [CanBeNull] T2 item2)
    {
      _item1 = item1;
      _item2 = item2;
    }

    [CanBeNull]
    public T1 Item1
    {
      get { return _item1; }
    }

    [CanBeNull]
    public T2 Item2
    {
      get { return _item2; }
    }
  }
}