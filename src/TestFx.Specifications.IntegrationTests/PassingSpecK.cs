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

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (PassingSpecK), "Method")]
  public class PassingSpecK : SpecK
  {
    public PassingSpecK ()
    {
      Specify (x => Console.WriteLine (true))
          .DefaultCase (_ => _
              .Given ("Arrangement", x => { })
              .It ("Assertion", x => { }));
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
  using System.Linq;

  public class PassingTest : TestBase<PassingSpecK>
  {
    [Test]
    public void Test ()
    {
      RunResult.State.Should ().Be (State.Passed);

      AssertResult (AssemblyResults.Single (), typeof (PassingSpecK).Assembly.Location, typeof (PassingSpecK).Assembly.GetName ().Name, State.Passed);
      AssertResult (TypeResults.Single (), typeof (PassingSpecK).FullName, "PassingSpecK.Method", State.Passed);
      AssertResult (ExpressionResults.Single (), "0", "Console.WriteLine(True)", State.Passed);
      AssertResult (TestResults.Single (), "0", "<Default>", State.Passed);
      AssertResult (OperationResults[0], "<OPERATION>", "<DefaultInitialization>", State.Passed, OperationType.Action);
      AssertResult (OperationResults[1], "<OPERATION>", "Arrangement", State.Passed, OperationType.Action);
      AssertResult (OperationResults[2], "<OPERATION>", "Console.WriteLine(True)", State.Passed, OperationType.Action);
      AssertResult (OperationResults[3], "<OPERATION>", "Assertion", State.Passed, OperationType.Assertion);

      TestResults.Single ().Identity.Absolute.Should ()
          .EndWith (@"TestFx.Specifications.IntegrationTests.dll » TestFx.Specifications.IntegrationTests.PassingSpecK » 0 » 0");
    }
  }
}

#endif