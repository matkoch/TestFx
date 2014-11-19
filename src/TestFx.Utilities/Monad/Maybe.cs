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
using System.Diagnostics;
using JetBrains.Annotations;

namespace TestFx.Utilities.Monad
{
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  [DebuggerDisplay ("HasValue = {HasValue}, Value = {Value}")]
  public struct Maybe<T>
  {
    public static readonly Maybe<T> Nothing;

    private readonly T _value;
    private readonly bool _hasValue;

    public Maybe (T value, bool hasValue = true)
    {
      _value = value;
      _hasValue = hasValue;
    }

    public T Value
    {
      get { return _value; }
    }

    public bool HasValue
    {
      get { return _hasValue; }
    }

    public static implicit operator T (Maybe<T> maybeMonad)
    {
      return maybeMonad.Value;
    }

    public static implicit operator Maybe<T> (T value)
    {
      return new Maybe<T>(value, !ReferenceEquals(value, null));
    }
  }
}