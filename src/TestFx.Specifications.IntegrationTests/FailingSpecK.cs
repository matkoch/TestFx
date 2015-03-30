// Copyright 2014, 2013 Matthias Koch
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

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (FailingSpecK), "Method")]
  public class FailingSpecK : SpecK
  {
    FailingSpecK ()
    {
      Specify (x => 1)
          .DefaultCase(_ => _
              .It ("Failing assertion", x => { throw new Exception (); })
              .It ("Passing assertion", x => x.Result.Should ().Be (1)));
    }
  }
}

#if !EXAMPLE

namespace TestFx.Specifications.IntegrationTests
{
  using Evaluation.Results;
  using Extensibility.Providers;
  using NUnit.Framework;

  public class FailingTest : TestBase<FailingSpecK>
  {
    [Test]
    public void Test ()
    {
      RunResult.State.Should ().Be (State.Failed);

      AssertResult (OperationResults[2], "Failing assertion", State.Failed, OperationType.Assertion);
      AssertResult (OperationResults[3], "Passing assertion", State.Passed, OperationType.Assertion);
    }
  }
}

#endif