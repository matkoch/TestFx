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
using Machine.Specifications;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;
using TestFx.Utilities;

namespace TestFx.MSpec.Tests
{
  [Subject (typeof(int), "is great")]
  internal class simple_context
  {
    internal class when_adding
    {
      static int Result;

      Because of = () => Result = 1 + 2;
      It returns_three = () => Result.Should().Be(3);
      It returns_four = () => Result.Should().Be(4);
    }
  }

  [TestFixture]
  internal class SimpleTest : TestBase<simple_context.when_adding>
  {
    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.GetAssemblySuiteResult()
          .HasRelativeId(typeof (SimpleTest).Assembly.Location)
          .HasText(@"TestFx.MSpec.Tests");

      runResult.GetClassSuiteResult()
          .HasRelativeId("TestFx.MSpec.Tests.simple_context+when_adding")
          .HasText("Int32 is great, when adding");

      var testResults = runResult.GetTestResults();
      testResults[0].HasPassed().HasRelativeId("returns_three").HasText("returns three");
      testResults[1].HasFailed().HasRelativeId("returns_four").HasText("returns four");
    }
  }
}