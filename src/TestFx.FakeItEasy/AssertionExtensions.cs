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
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using FakeItEasy.Core;
using TestFx.Extensibility;
using TestFx.FakeItEasy.TestExtensions;
using TestFx.Specifications.Implementation.Controllers;
using TestFx.Specifications.InferredApi;
using TestFx.Utilities;
using TestFx.Utilities.Expressions;

namespace TestFx.FakeItEasy
{
  public static class AssertionExtensions
  {
    public static IAssert<TSubject, TResult, TVars> ItMakesOrderedCallsTo<TSubject, TResult, TVars> (
        this IAssert<TSubject, TResult, TVars> assert,
        Expression<Func<Dummy, object[]>> fakesProvider)
    {
      var controller = assert.Get<ITestController<TSubject, TResult, TVars>>();
      var text = "CallsInOrder " + fakesProvider.ParseExcept(new[] { typeof (ISuite) });
      controller.AddAssertion(
          text,
          x =>
          {
            var scope = x[typeof (FakeScopeTestExtension).FullName].To<IFakeScope>();
            using (scope.OrderedAssertions())
            {
              fakesProvider.Compile()(null).ForEach(f => A.CallTo(f).MustHaveHappened());
            }
          });
      return assert;
    }

    public static IAssert<TSubject, TResult, TVars> ItMakesOrderedCallsWithAnyArguments<TSubject, TResult, TVars> (
        this IAssert<TSubject, TResult, TVars> assert,
        params Expression<Action<TVars>>[] callExpressions)
    {
      var controller = assert.Get<ITestController<TSubject, TResult, TVars>>();
      // TODO: parse without TVars _ONLY_ if they were declared (consider 'Define(x => new object())')
      var text = "CallsInOrder [ " + string.Join(" , ", callExpressions.Select(x => x.ParseExcept(new[] { typeof(ISuite) })).ToArray()) + " ]";
      controller.AddAssertion(
          text,
          x =>
          {
            var scope = x[typeof (FakeScopeTestExtension).FullName].To<IFakeScope>();
            using (scope.OrderedAssertions())
            {
              foreach (var callExpression in callExpressions)
                A.CallTo(Expression.Lambda<Action>(callExpression.Body)).WithAnyArguments().MustHaveHappened();
            }
          });
      return assert;
    }
  }
}