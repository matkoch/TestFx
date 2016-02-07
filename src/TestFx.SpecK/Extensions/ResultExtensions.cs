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
using System.Linq.Expressions;
using JetBrains.Annotations;
using TestFx.SpecK.Implementation.Containers;
using TestFx.SpecK.Implementation.Utilities;
using TestFx.SpecK.InferredApi;

// ReSharper disable once CheckNamespace

namespace TestFx.SpecK
{
  [PublicAPI]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public static class ResultExtensions
  {
    /// <summary>
    ///   .ItReturns((DerivedType x) => x);
    /// </summary>
    public static IAssert<TSubject, TResult, TVars, TSequence> ItReturns<TSubject, TResult, TVars, TSequence, TDerivedResult> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        Action<TDerivedResult> resultAssertion)
        where TDerivedResult : class, TResult
    {
      var controller = assert.GetTestController();
      controller.AddAssertion(
          "Returns " + typeof (TDerivedResult).Name,
          x => resultAssertion(x.Result as TDerivedResult));
      return assert;
    }

    /// <summary>
    ///   .ItReturns(x => x.Exception);
    /// </summary>
    public static IAssert<TSubject, TResult, TVars, TSequence> ItReturns<TSubject, TResult, TVars, TSequence> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        Expression<Func<TVars, TResult>> resultProvider)
    {
      var controller = assert.GetTestController();
      controller.AddAssertion(
          "Returns " + resultProvider,
          x => AssertionHelper.AssertObjectEquals("Result", resultProvider.Compile()(x.Vars), x.Result));
      return assert;
    }
  }
}