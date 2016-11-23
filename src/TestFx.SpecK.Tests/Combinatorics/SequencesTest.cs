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

// ReSharper disable ArgumentsStyleLiteral

namespace TestFx.SpecK.Tests.Combinatorics
{
  internal class SequencesTest : TestBase<SequencesTest.DomainSpec>
  {
    [Subject (typeof (SequencesTest))]
    internal class DomainSpec : Spec
    {
      int A;
      int B;

      public DomainSpec ()
      {
        Specify (x => A + B)
            .DefaultCase (_ => _
                .WithSequences (
                    "First sequence", new { A = 1, B = 2, Result = 3 },
                    "Second sequence", new { A = 2, B = 3, Result = 6 })
                .Given (x => A = x.Sequence.A)
                .Given (x => B = x.Sequence.B)
                .It ("returns result", x => x.Result.Should ().Be (x.Sequence.Result)));
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      var testResult = runResult.GetTestResults ();
      testResult[0].HasPassed ().HasRelativeId ("First sequence");
      testResult[1].HasFailed ().HasRelativeId ("Second sequence");
    }
  }
}