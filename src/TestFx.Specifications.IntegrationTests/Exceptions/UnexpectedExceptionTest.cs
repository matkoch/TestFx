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
using TestFx.Utilities;
using UserNamespace;

namespace TestFx.Specifications.IntegrationTests.Exceptions
{
  public class UnexpectedExceptionTest : TestBase<UnexpectedExceptionTest.DomainSpecK>
  {
    [Subject (typeof (UnexpectedExceptionTest), "Test")]
    public class DomainSpecK : SpecK
    {
      string Message;
      Exception InnerException;

      DomainSpecK ()
      {
        Specify (x => UserClass.Throw<ArgumentException> (Message, InnerException))
            .DefaultCase (_ => _
                .Given ("a message", x => Message = "Message")
                .Given ("an inner exception with message", x => InnerException = new Exception ("InnerMessage"))
                .It ("asserts something different", x => { }));
      }
    }

    [Test]
    public override void Test ()
    {
      AssertDefaultTest (State.Failed)
          .WithOperations (
              "<Reset_Instance_Fields>",
              "a message",
              "an inner exception with message",
              "<Action>")
          .WithFailureDetails (
              "<Action>",
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