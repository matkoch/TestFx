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
using FakeItEasy.Core;
using FluentAssertions;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.Tests.Combinatorics
{
  public class PermutationsTest : TestBase<PermutationsTest.DomainSpec>
  {
    [Subject (typeof (PermutationsTest), "Test")]
    public class DomainSpec : Spec
    {
      int A;
      int B;

      public DomainSpec ()
      {
        Specify (x => A + B)
            .DefaultCase (_ => _
                .WithPermutations (
                    new { Object = default(object), A = default(int), B = default(int) },
                    x => x.Object, new[] { new object () },
                    x => x.A, new[] { 1, 2 },
                    x => x.B, new[] { 3, 4 })
                .Given (x => A = x.Sequence.A)
                .Given (x => B = x.Sequence.B)
                .It ("returns result", x => x.Result.Should ().Be (5)));
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      var testResult = runResult.GetTestResults ();
      testResult[0].HasFailed ().HasRelativeId ("Object = Object, A = 1, B = 3");
      testResult[1].HasPassed ().HasRelativeId ("Object = Object, A = 1, B = 4");
      testResult[2].HasPassed ().HasRelativeId ("Object = Object, A = 2, B = 3");
      testResult[3].HasFailed ().HasRelativeId ("Object = Object, A = 2, B = 4");
    }
  }
}