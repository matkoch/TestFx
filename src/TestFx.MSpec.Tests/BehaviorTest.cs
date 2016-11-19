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

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable NotAccessedField.Local

namespace TestFx.MSpec.Tests
{
  [Subject (typeof (int))]
  internal class when_calling
  {
    static object Result;

    Because of = () => Result = new object();

    Behaves_like<MyBehavior> _;
  }

  [Behaviors]
  internal class MyBehavior
  {
    Behaves_like<MyBehavior2> _;
  }

  [Behaviors]
  internal class MyBehavior2
  {
    static object Result;

    It asserts = () => Result.Should().NotBeNull();
  }

  [TestFixture]
  internal class BehaviorTest : TestBase<when_calling>
  {
    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.GetTestResult()
          .HasPassed()
          .HasText("asserts");
    }
  }
}