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
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

// ReSharper disable ArgumentsStyleLiteral

namespace TestFx.SpecK.Tests.Simple
{
  internal class InconclusiveTest : TestBase<InconclusiveTest.DomainSpec>
  {
    [Subject (typeof (InconclusiveTest))]
    internal class DomainSpec : Spec
    {
      DomainSpec ()
      {
        Specify (x => 1)
            .DefaultCase (_ => _
                .Given ("arranges something")
                .It ("does something")
                .It ("passes", x => { })
                .It ("fails", x => { throw new Exception (); }));
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.WasInconclusive ();

      var testResults = runResult.GetTestResults ();
      testResults[0]
          .WasInconclusive ()
          .HasOperations (
              "arranges something",
              Constants.Action,
              "does something",
              "passes",
              "fails")
          .HasFailingOperations (
              "fails");
    }
  }
}