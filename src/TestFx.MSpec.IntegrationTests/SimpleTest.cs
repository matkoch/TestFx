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
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TestFx.Evaluation.Results;

namespace TestFx.MSpec.IntegrationTests
{
  [Subject (typeof (int))]
  public class when_adding
  {
    static int Result;

    Because of = () => Result = 1 + 2;
    It returns_three = () => Result.Should().Be(3);
    It returns_three_again = () => Result.Should().Be(3);
  }

  [TestFixture]
  public class SimpleTest : TestBase<when_adding>
  {
    protected override void AssertResults ()
    {
      var assemblyResult = RunResult.SuiteResults.Single();
      var suiteResult = assemblyResult.SuiteResults.Single();
      AssertResult(suiteResult, "TestFx.MSpec.IntegrationTests.when_adding", "Int32, when_adding", State.Passed);
      AssertResult(TestResults[0], "returns_three", "returns three", State.Passed);
      AssertResult(TestResults[1], "returns_three_again", "returns three again", State.Passed);
    }
  }
}