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
  [TestFixture]
  public class SimpleTest : TestBase<SimpleTest.when_adding>
  {
    [Subject (typeof (int))]
    public class when_adding
    {
      static int A;
      static int B;
      static int Result;

      Establish ctx = () =>
      {
        A = 1;
        B = 2;
      };

      Because of = () => Result = A + B;
      It returns_three = () => Result.Should ().Be (3);
      It returns_three_again = () => Result.Should ().Be (3);
    }

    [Test]
    public override void Test ()
    {
      var assemblyResult = RunResult.SuiteResults.Single ();
      var suiteResult = assemblyResult.SuiteResults.Single ();
      AssertResult(suiteResult, "TestFx.MSpec.IntegrationTests.SimpleTest+when_adding", "Int32", State.Passed);

      var setupResult = suiteResult.SetupResults.Single ();
      var cleanupResult = suiteResult.CleanupResults.Single ();
      AssertResult (setupResult, "Establish", State.Passed);
      AssertResult (cleanupResult, "Cleanup", State.Passed);

      AssertResult(TestResults[0], "returns three", "returns three", State.Passed);
      AssertResult(TestResults[1], "returns three again", "returns three again", State.Passed);
    }
  }
}