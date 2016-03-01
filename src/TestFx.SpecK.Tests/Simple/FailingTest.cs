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

namespace TestFx.SpecK.Tests.Simple
{
  internal class FailingTest : TestBase<FailingTest.DomainSpec>
  {
    [Subject (typeof (FailingTest))]
    internal class DomainSpec : Spec
    {
      DomainSpec ()
      {
        Specify (x => 1)
            .DefaultCase (_ => _
                .It ("Failing assertion", x => { throw new Exception (); })
                .It ("Passing assertion", x => x.Result.Should ().Be (1))
                .It ("Another failing assertion", x => { throw new Exception (); }))
            .Case ("Passing", _ => _);
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.HasFailed ();

      var testResults = runResult.GetTestResults ();
      testResults[0]
          .HasFailed ()
          .HasOperations (
              Constants.Action,
              "Failing assertion",
              "Passing assertion",
              "Another failing assertion")
          .HasFailingOperations (
              "Failing assertion",
              "Another failing assertion");
      testResults[1].HasPassed ();
    }
  }
}