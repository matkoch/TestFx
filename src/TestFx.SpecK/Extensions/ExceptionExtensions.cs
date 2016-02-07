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
using TestFx.Utilities;

// ReSharper disable once CheckNamespace

namespace TestFx.SpecK
{
  [PublicAPI]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public static class ExceptionExtensions
  {
    private const bool c_expectException = true;

    public static IAssert<TSubject, TResult, TVars, TSequence> ItThrows<TSubject, TResult, TVars, TSequence> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        Type exceptionType,
        string message)
    {
      return assert.ItThrows(exceptionType, x => message);
    }

    public static IAssert<TSubject, TResult, TVars, TSequence> ItThrows<TSubject, TResult, TVars, TSequence> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        Type exceptionType,
        [CanBeNull] Func<TVars, string> messageProvider = null,
        [CanBeNull] Func<TVars, Exception> innerExceptionProvider = null)
    {
      var controller = assert.GetTestController();
      controller.AddAssertion(
          "Throws " + exceptionType.Name,
          x =>
          {
            AssertionHelper.AssertInstanceOfType("Exception", exceptionType, x.Exception);
            if (messageProvider != null)
              AssertionHelper.AssertExceptionMessage(messageProvider(x.Vars), x.Exception);
            if (innerExceptionProvider != null)
              AssertionHelper.AssertObjectEquals("InnerException", innerExceptionProvider(x.Vars), x.Exception.NotNull().InnerException);
          },
          c_expectException);
      return assert;
    }

    public static IAssert<TSubject, TResult, TVars, TSequence> ItThrows<TSubject, TResult, TVars, TSequence, TException> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        Action<TException> exceptionAssertion)
        where TException : Exception
    {
      var controller = assert.GetTestController();
      controller.AddAssertion(
          "Throws " + typeof (TException).Name,
          x => exceptionAssertion(x.Exception as TException),
          c_expectException);
      return assert;
    }

    public static IAssert<TSubject, TResult, TVars, TSequence> ItThrows<TSubject, TResult, TVars, TSequence, TException> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        Expression<Func<TVars, TException>> exceptionProvider)
        where TException : Exception
    {
      var controller = assert.GetTestController();
      controller.AddAssertion(
          "Throws " + exceptionProvider,
          x => AssertionHelper.AssertObjectEquals("Exception", exceptionProvider.Compile()(x.Vars), x.Exception),
          c_expectException);
      return assert;
    }

    public static IAssert<TSubject, TResult, TVars, TSequence> ItThrows<TSubject, TResult, TVars, TSequence> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        string text,
        Assertion<TSubject, TResult, TVars, TSequence> assertion)
    {
      var controller = assert.GetTestController();
      controller.AddAssertion("Throws " + text, assertion, c_expectException);
      return assert;
    }
  }
}