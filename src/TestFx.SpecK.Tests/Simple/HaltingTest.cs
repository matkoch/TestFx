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
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.Tests.Simple
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

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.HasFailed ();

      var testResults = runResult.GetTestResults ();
      testResults[0]
          .HasFailed ()
          .HasOperations ("Throwing arrangement")
          .HasFailingOperation ("Throwing arrangement");
      testResults[1].HasPassed ();
    }
  }
}
