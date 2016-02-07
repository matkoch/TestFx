// Copyright 2016, 2015, 2014 Matthias Koch
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
using TestFx.SpecK.Implementation;
using TestFx.SpecK.Implementation.Containers;
using TestFx.SpecK.InferredApi;

// ReSharper disable once CheckNamespace

namespace TestFx.SpecK
{
  [PublicAPI]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public static class UnimplementedExtensions
  {
    public static IArrangeOrAssert<TSubject, TResult, TVars, TSequence> Given<TSubject, TResult, TVars, TSequence> (
        this IArrange<TSubject, TResult, TVars, TSequence> arrange,
        string text)
    {
      var controller = arrange.GetTestController();
      controller.AddNotImplementedAction<Arrange>(text);
      return (IArrangeOrAssert<TSubject, TResult, TVars, TSequence>) arrange;
    }

    public static IAssert<TSubject, TResult, TVars, TSequence> It<TSubject, TResult, TVars, TSequence> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        string text)
    {
      var controller = assert.GetTestController();
      controller.AddNotImplementedAssertion<Assert>(text);
      return assert;
    }
  }
}