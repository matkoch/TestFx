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

namespace TestFx.SpecK.IntegrationTests.Simple
{
  public class HaltingTest : TestBase<HaltingTest.DomainSpec>
  {
    [Subject (typeof (HaltingTest), "Test")]
    public class DomainSpec : Spec
    {
      public DomainSpec ()
      {
        Specify (x => 1)
            .DefaultCase (_ => _
                .Given ("Throwing arrangement", x => { throw new Exception (); })
                .Given ("Halted arrangement", x => { })
                .It ("Halted assertion", x => { }))
            .Case ("Passing", _ => _);
      }
    }

    [Test]
    public override void Test ()
    {
      AssertTest (Default, State.Failed)
          .WithOperations ("Throwing arrangement")
          .WithFailures ("Throwing arrangement");

      AssertTest ("Passing", State.Passed);

      RunResult.State.Should ().Be (State.Failed);
    }
  }
}
