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
using FluentAssertions;
using NUnit.Framework;
using TestFx.Evaluation.Results;

namespace TestFx.Specifications.IntegrationTests.Combinatorics
{
  public class PermutationsTest : TestBase<SequencesTest.DomainSpecK>
  {
    [Subject (typeof (PermutationsTest), "Test")]
    public class DomainSpecK : SpecK
    {
      int A;
      int B;

      public DomainSpecK ()
      {
        Specify (x => A + B)
            .DefaultCase (_ => _
                .WithPermutations (
                    new { A = default(int), B = default(int) },
                    x => x.A, new[] { 1, 2 },
                    x => x.B, new[] { 3, 4, 5 })
                .Given (x => A = x.Combi.A)
                .Given (x => B = x.Combi.B)
                .It ("returns result", x => x.Result.Should ().Be (5)));
      }
    }

    [Test]
    public override void Test ()
    {
      AssertTest ("A = 1, B = 3", State.Failed);
      AssertTest ("A = 1, B = 4", State.Passed);
      AssertTest ("A = 1, B = 5", State.Failed);
      AssertTest ("A = 2, B = 3", State.Passed);
      AssertTest ("A = 2, B = 4", State.Failed);
      AssertTest ("A = 2, B = 5", State.Failed);
    }
  }
}
