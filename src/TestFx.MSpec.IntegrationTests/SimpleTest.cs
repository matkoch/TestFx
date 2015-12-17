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
using FakeItEasy.Core;
using FluentAssertions;
using Machine.Specifications;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

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
    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.HasPassed();

      runResult.GetAssemblySuiteResult()
          .HasRelativeId(@"C:\TestFx\src\TestFx.MSpec.IntegrationTests\bin\Debug\TestFx.MSpec.IntegrationTests.dll")
          .HasText(@"TestFx.MSpec.IntegrationTests");

      runResult.GetClassSuiteResult()
          .HasRelativeId("TestFx.MSpec.IntegrationTests.when_adding")
          .HasText("Int32, when_adding");

      var testResults = runResult.GetTestResults();
      testResults[0].HasRelativeId("returns_three").HasText("returns three");
      testResults[1].HasRelativeId("returns_three_again").HasText("returns three again");
    }
  }
}