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
using System.Linq.Expressions;
using JetBrains.Annotations;
using TestFx.Specifications.Implementation.Controllers;
using TestFx.Specifications.Implementation.Utilities;
using TestFx.Specifications.InferredApi;
using TestFx.Utilities;
using TestFx.Utilities.Expressions;

namespace TestFx.Specifications
{
  public static class ExceptionExtensions
  {
    private const bool c_expectException = true;

    public static IAssert ItThrows<TSubject, TResult, TVars, TException> (
        this IAssert<TSubject, TResult, TVars> assert,
        [CanBeNull] Func<TVars, string> messageProvider,
        [CanBeNull] Func<TVars, Exception> innerExceptionProvider = null)
        where TException : Exception
    {
      var controller = assert.Get<ITestController<TSubject, TResult, TVars>>();
      controller.AddAssertion(
          "Throws " + typeof (TException).Name,
          x =>
          {
            AssertionHelper.AssertInstanceOfType("Exception", typeof (TException), x.Exception);
            if (messageProvider != null)
              AssertionHelper.AssertExceptionMessage(messageProvider(x.Vars), x.Exception);
            if (innerExceptionProvider != null)
              AssertionHelper.AssertReferenceEquals("InnerException", innerExceptionProvider(x.Vars), x.Exception.AssertNotNull().InnerException);
          },
          c_expectException);
      return assert;
    }

    public static IAssert ItThrows<TSubject, TResult, TVars, TException> (
        this IAssert<TSubject, TResult, TVars> assert,
        Action<TException> exceptionAssertion)
        where TException : Exception
    {
      var controller = assert.Get<ITestController<TSubject, TResult, TVars>>();
      controller.AddAssertion(
          "Throws " + typeof (TException).Name,
          x => exceptionAssertion(x.Exception.As<TException>()),
          c_expectException);
      return assert;
    }

    public static IAssert ItThrows<TSubject, TResult, TVars, TException> (
        this IAssert<TSubject, TResult, TVars> assert,
        Expression<Func<TVars, TException>> exceptionProvider)
        where TException : Exception
    {
      var controller = assert.Get<ITestController<TSubject, TResult, TVars>>();
      controller.AddAssertion(
          "Throws " + exceptionProvider.ParseExcept(new[] { typeof (TVars) }),
          x => AssertionHelper.AssertReferenceEquals("Exception", exceptionProvider.Compile()(x.Vars), x.Exception),
          c_expectException);
      return assert;
    }
  }
}