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
using FakeItEasy;
using FakeItEasy.Core;
using JetBrains.Annotations;
using TestFx.SpecK.Implementation;
using TestFx.SpecK.Implementation.Containers;
using TestFx.SpecK.InferredApi;

namespace TestFx.FakeItEasy
{
  [PublicAPI]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public static class AssertionExtensions
  {
    private const string c_key = "FakeItEasy.OrderedAssertions";

    public static IAssert<TSubject, TResult, TVars, TSequence> ItCallsInOrder<TSubject, TResult, TVars, TSequence> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        Assertion<TSubject, TResult, TVars, TSequence> orderedAssertion)
    {
      return assert.ItCallsInOrder(string.Empty, orderedAssertion);
    }

    public static IAssert<TSubject, TResult, TVars, TSequence> ItCallsInOrder<TSubject, TResult, TVars, TSequence> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        string text,
        Assertion<TSubject, TResult, TVars, TSequence> orderedAssertion)
    {
      var controller = assert.GetTestController();
      controller.Replace<Act>(
          (x, originalAction) =>
          {
            var scope = Fake.CreateScope();
            x[c_key] = scope;
            using (scope)
            {
              originalAction();
            }
          });
      controller.AddAssertion(
          "calls in order " + text,
          x =>
          {
            var scope = (IFakeScope) x[c_key];
            using (scope.OrderedAssertions())
            {
              orderedAssertion(x);
            }
          });
      return assert;
    }
  }
}