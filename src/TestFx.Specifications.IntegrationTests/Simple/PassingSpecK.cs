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
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.Extensibility.Providers;

namespace TestFx.Specifications.IntegrationTests.Simple
{
  public class PassingTest : TestBase<PassingTest.DomainSpecK>
  {
    [Subject (typeof (PassingTest), "Test")]
    public class DomainSpecK : SpecK
    {
      object Object;

      public DomainSpecK ()
      {
        Specify (x => Console.WriteLine (true))
            .DefaultCase (_ => _
                .Given (x => { })
                .It ("Assertion", x => { }));
      }
    }

    [Test]
    public override void Test ()
    {
      RunResult.State.Should ().Be (State.Passed);

      AssertResult (AssemblyResults.Single (),
          relativeId: typeof (DomainSpecK).Assembly.Location,
          text: typeof (DomainSpecK).Assembly.GetName ().Name,
          state: State.Passed);

      AssertResult (TypeResults.Single (),
          relativeId: typeof (DomainSpecK).FullName,
          text: "PassingTest.Test",
          state: State.Passed);

      AssertResult (TestResults.Single (),
          relativeId: "<Default>",
          text: "<Default>",
          state: State.Passed);

      AssertResult (OperationResults[0], "<Reset_Instance_Fields>", State.Passed, OperationType.Action);
      AssertResult (OperationResults[1], "<Arrangement>", State.Passed, OperationType.Action);
      AssertResult (OperationResults[2], "<Action>", State.Passed, OperationType.Action);
      AssertResult (OperationResults[3], "Assertion", State.Passed, OperationType.Assertion);

      TestResults.Single ().Identity.Absolute.Should ()
          .EndWith (
              "TestFx.Specifications.IntegrationTests.dll � " +
              "TestFx.Specifications.IntegrationTests.Simple.PassingTest+DomainSpecK � " +
              "<Default>");
    }
  }
}