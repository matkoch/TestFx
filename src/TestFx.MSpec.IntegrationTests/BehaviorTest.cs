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

namespace TestFx.MSpec.IntegrationTests
{
  [TestFixture]
  public class BehaviorTest : TestBase<BehaviorTest.when_calling>
  {
    [Subject (typeof (int))]
    public class when_calling
    {
      static object Result;

      Because of = () => Result = new object ();

      Behaves_like<MyBehavior> _;
    }

    public class MyBehavior
    {
      static object Result;

      Behaves_like<MyBehavior2> _;
    }

    public class MyBehavior2
    {
      static object Result;

      It asserts = () => Result.Should().NotBeNull();
    }

    [Test]
    public override void Test ()
    {
      AssertTest ("asserts", State.Passed);
    }
  }
}