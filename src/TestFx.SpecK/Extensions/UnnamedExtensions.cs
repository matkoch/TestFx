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
using TestFx.Extensibility;
using TestFx.SpecK.InferredApi;

namespace TestFx.SpecK
{
  public static class UnnamedExtensions
  {
    [DisplayFormat ("<Default>")]
    public static IIgnoreOrCase<TSubject, TResult> DefaultCase<TSubject, TResult> (
        this ICase<TSubject, TResult> @case,
        Func<ICombineOrArrangeOrAssert<TSubject, TResult, Dummy, Dummy>, IAssert> succession)
    {
      return @case.Case("<Default>", succession);
    }

    public static IArrangeOrAssert<TSubject, TResult, TVars, TSequence> Given<TSubject, TResult, TVars, TSequence> (
        this IArrange<TSubject, TResult, TVars, TSequence> arrange,
        Arrangement<TSubject, TResult, TVars, TSequence> arrangement)
    {
      return arrange.Given("<Arrangement>", arrangement);
    }
  }
}