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
  [DebuggerNonUserCode]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public static class MaybeMonadExtensions
  {
    public static Maybe<T> Maybe<T> (this T value)
    {
      return value;
    }

    public static Maybe<T> Maybe<T> (this T value, Func<T, bool> hasValue)
    {
      return new Maybe<T>(value, hasValue(value));
    }

    public static Maybe<TResult> Maybe<TInput, TResult> (this TInput value, Func<TInput, TResult> k)
    {
      return k(value.Maybe());
    }

    public static Maybe<TResult> Maybe<TInput, TResult> (this Maybe<TInput> maybe, Func<TInput, TResult> k)
    {
      return maybe.HasValue ? k(maybe).Maybe() : Monad.Maybe<TResult>.Nothing;
    }

    #region Query extensions

    public static Maybe<T> Where<T> (this Maybe<T> maybe, Func<T, bool> k)
    {
      return maybe.HasValue && k(maybe) ? maybe : Monad.Maybe<T>.Nothing;
    }

    public static Maybe<TResult> Select<TInput, TResult> (this Maybe<TInput> maybe, Func<TInput, TResult> k)
    {
      return maybe.HasValue ? k(maybe).Maybe() : Monad.Maybe<TResult>.Nothing;
    }

    public static Maybe<TResult> SelectMany<TInput1, TInput2, TResult> (
        this Maybe<TInput1> maybe,
        Func<TInput1, Maybe<TInput2>> k,
        Func<TInput1, TInput2, TResult> s)
    {
      if (!maybe.HasValue)
        return Monad.Maybe<TResult>.Nothing;

      var maybe2 = k(maybe);
      if (!maybe2.HasValue)
        return Monad.Maybe<TResult>.Nothing;

      return s(maybe, maybe2).Maybe();
    }

    #endregion
  }
}