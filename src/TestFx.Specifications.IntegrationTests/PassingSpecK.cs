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
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.Extensibility.Providers;

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (PassingSpecK), "Method")]
  public class PassingSpecK : SpecK
  {
    public PassingSpecK ()
    {
      Specify (x => Console.WriteLine (true))
          .Elaborate ("Case", _ => _
              .Given ("Arrangement", x => { })
              .It ("Assertion", x => { }));
    }
  }

#if !EXAMPLE
  public class PassingTest : TestBase<PassingSpecK>
  {
    [Test]
    public void Test ()
    {
      RunResult.State.Should ().Be (State.Passed);

      AssertResult (AssemblyResults.Single (), typeof (PassingSpecK).Assembly.Location, typeof (PassingSpecK).Assembly.GetName ().Name, State.Passed);
      AssertResult (TypeResults.Single (), typeof (PassingSpecK).FullName, "PassingSpecK.Method", State.Passed);
      AssertResult (ExpressionResults.Single (), "0", "Console.WriteLine(True)", State.Passed);
      AssertResult (TestResults.Single (), "0", "Case", State.Passed);
      AssertResult (OperationResults[0], "<OPERATION>", "Arrangement", State.Passed, OperationType.Action);
      AssertResult (OperationResults[1], "<OPERATION>", "Console.WriteLine(True)", State.Passed, OperationType.Action);
      AssertResult (OperationResults[2], "<OPERATION>", "Assertion", State.Passed, OperationType.Assertion);

      //assertion.GetAbsoluteId ().Should ()
      //    .Be (
      //        @"C:\Users\matthias.koch\Desktop\SpecK\src\SpecK.Framework.IntegrationTests\bin\Debug\SpecK.Framework.IntegrationTests.dll " +
      //        @"» Spe_cK.Framework.IntegrationTests.PassingSpecs " +
      //        @"» .ctor " +
      //        @"» 0 » 0 » 3");
    }
  }
#endif
}