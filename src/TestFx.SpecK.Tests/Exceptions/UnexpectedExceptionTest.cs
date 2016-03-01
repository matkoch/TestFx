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
using UserNamespace;

namespace TestFx.SpecK.Tests.Exceptions
{
  internal class UnexpectedExceptionTest : TestBase<UnexpectedExceptionTest.DomainSpec>
  {
    [Subject (typeof (UnexpectedExceptionTest))]
    internal class DomainSpec : Spec
    {
      string Message;
      Exception InnerException;

      DomainSpec ()
      {
        Specify (x => UserClass.Throw<ArgumentException> (Message, InnerException))
            .DefaultCase (_ => _
                .Given ("a message", x => Message = "Message")
                .Given ("an inner exception with message", x => InnerException = new Exception ("InnerMessage"))
                .It ("asserts something different", x => { }));
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.GetTestResult ()
          .HasFailed ()
          .HasOperations (
              Constants.Reset_Instance_Fields,
              "a message",
              "an inner exception with message",
              Constants.Action)
          .HasFailingOperation (
              Constants.Action,
              x =>
              {
                x.Name.Should ().Be ("ArgumentException");
                x.FullName.Should ().Be ("System.ArgumentException");
                x.StackTrace.Should ().NotContain ("at TestFx");
                x.StackTrace.Should ().Contain ("at UserNamespace");
                x.Message.Should ().Be ("Message\r\n---> InnerMessage");
              });
    }
  }
}