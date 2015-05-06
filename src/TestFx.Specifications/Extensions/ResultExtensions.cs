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
using System.Linq.Expressions;
using TestFx.Specifications.Implementation.Controllers;
using TestFx.Specifications.Implementation.Utilities;
using TestFx.Specifications.InferredApi;

namespace TestFx.Specifications
{
  public static class ResultExtensions
  {
    /// <summary>
    /// .ItReturns((DerivedType x) => x);
    /// </summary>
    public static IAssert<TSubject, TResult, TVars> ItReturns<TSubject, TResult, TVars, TDerivedResult> (
        this IAssert<TSubject, TResult, TVars> assert,
        Action<TDerivedResult> resultAssertion)
        where TDerivedResult : class, TResult
    {
      var controller = assert.Get<ITestController<TSubject, TResult, TVars>>();
      controller.AddAssertion(
          "Returns " + typeof (TDerivedResult).Name,
          x => resultAssertion(x.Result as TDerivedResult));
      return assert;
    }
    
    /// <summary>
    /// .ItReturns(x => x.Exception);
    /// </summary>
    public static IAssert<TSubject, TResult, TVars> ItReturns<TSubject, TResult, TVars> (
        this IAssert<TSubject, TResult, TVars> assert,
        Expression<Func<TVars, TResult>> resultProvider)
    {
      var controller = assert.Get<ITestController<TSubject, TResult, TVars>>();
      controller.AddAssertion(
          "Returns " + resultProvider,
          x => AssertionHelper.AssertObjectEquals("Result", resultProvider.Compile()(x.Vars), x.Result));
      return assert;
    }
  }
}