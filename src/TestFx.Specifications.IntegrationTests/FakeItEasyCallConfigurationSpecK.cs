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
using FakeItEasy;
using TestFx.FakeItEasy;

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (FakeItEasyCallConfigurationSpecK), "Method")]
  public class FakeItEasyCallConfigurationSpecK : FakeItEasySpecK
  {
    FakeItEasyCallConfigurationSpecK ()
    {
      Specify (x => x.DoSomething2 ())
          .Case ("Case 1", _ => _
              .Define (x => new { MyException = new InvalidOperationException () })
              .GivenACallTo (x => ServiceProvider.GetService (null), (c, x) => c
                  .Throws (x.MyException))
              .ItThrows (x => x.MyException))
          .Case ("Case 2", _ => _
              .GivenACallTo (x => ServiceProvider.GetService (null), (c, x) => c
                  .Returns (OtherService))
              .ItReturns (x => OtherService));
    }
  }
}

#if !EXAMPLE

namespace TestFx.Specifications.IntegrationTests
{
  using Evaluation.Results;
  using FluentAssertions;
  using NUnit.Framework;

  public class FakeItEasyCallConfigurationTest : TestBase<FakeItEasyCallConfigurationSpecK>
  {
    [Test]
    public void Test ()
    {
      RunResult.State.Should ().Be (State.Passed);
      
      AssertResult (OperationResults[1], "<OPERATION>", "<FakeCreation>", State.Passed);
      AssertResult (OperationResults[2], "<OPERATION>", "<FakeSetup>", State.Passed);
      AssertResult (OperationResults[4], "<OPERATION>", "ACallTo ServiceProvider.GetService().Throws(MyException)", State.Passed);
      AssertResult (OperationResults[6], "<OPERATION>", "Throws MyException", State.Passed);
      AssertResult (OperationResults[11], "<OPERATION>", "ACallTo ServiceProvider.GetService().Returns(OtherService)", State.Passed);
      AssertResult (OperationResults[13], "<OPERATION>", "Returns OtherService", State.Passed);
    }
  }
}

#endif