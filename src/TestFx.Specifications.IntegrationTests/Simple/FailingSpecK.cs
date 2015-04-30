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

namespace TestFx.Specifications.IntegrationTests.Simple
{
  public class FailingTest : TestBase<FailingTest.DomainSpecK>
  {
    [Subject (typeof (FailingTest), "Test")]
    public class DomainSpecK : SpecK
    {
      DomainSpecK ()
      {
        Specify (x => 1)
            .DefaultCase (_ => _
                .It ("Failing assertion", x => { throw new Exception (); })
                .It ("Passing assertion", x => x.Result.Should ().Be (1)))
            .Case ("Passing", _ => _);
      }
    }

    [Test]
    public override void Test ()
    {
      AssertTestFailed ("<Default>",
          operationTexts: null /* don't care */,
          failedOperationTexts: new[] { "Failing assertion" });

      AssertTestPassed ("Passing", operationTexts: null /* don't care */);
      RunResult.State.Should ().Be (State.Failed);
    }
  }
}
