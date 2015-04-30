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
      var failedTest = AssertTestFailed ("<Default>",
          operationTexts: new[]
                          {
                              "<Reset_Instance_Fields>",
                              "a message",
                              "an inner exception with message",
                              "<Action>"
                          },
          failedOperationTexts: new[] { "<Action>" });

      var exception = failedTest.OperationResults.Single (x => x.Text == "<Action>").Exception.AssertNotNull ();
      exception.Name.Should ().Be ("ArgumentException");
      exception.FullName.Should ().Be ("System.ArgumentException");
      exception.StackTrace.Should ().NotContain ("at TestFx");
      exception.StackTrace.Should ().Contain ("at UserNamespace");
      exception.Message.Should ().Be ("Message\r\n---> InnerMessage");
    }
  }
}