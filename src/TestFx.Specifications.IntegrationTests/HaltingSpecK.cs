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

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (HaltingSpecK), "Method")]
  public class HaltingSpecK : SpecK
  {
    public HaltingSpecK ()
    {
      Specify (x => 1)
          .DefaultCase (_ => _
              .Given ("Throwing arrangement", x => { throw new Exception (); })
              .Given ("Halted arrangement", x => { })
              .It ("Halted assertion", x => { }));
    }
  }
}

#if !EXAMPLE

namespace TestFx.Specifications.IntegrationTests
{
  using Evaluation.Results;
  using Extensibility.Providers;
  using FluentAssertions;
  using NUnit.Framework;

  public class HaltingTest : TestBase<HaltingSpecK>
  {
    [Test]
    public void Test ()
    {
      RunResult.State.Should ().Be (State.Failed);

      OperationResults.Should ().HaveCount (2);
      AssertResult (OperationResults[0], "<Reset_Instance_Fields>", State.Passed, OperationType.Action);
      AssertResult (OperationResults[1], "Throwing arrangement", State.Failed, OperationType.Action);
    }
  }
}

#endif