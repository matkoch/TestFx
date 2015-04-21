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

namespace UserNamespace
{
  public static class UserClass
  {
    public static void Throw (string message, Exception innerException)
    {
      throw new ArgumentException (message, innerException);
    }
  }
}

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (ExceptionSpecK), "Method")]
  public class ExceptionSpecK : SpecK
  {
    string Message;
    Exception InnerException;

    ExceptionSpecK ()
    {
      Specify (x => UserClass.Throw (Message, InnerException))
          .DefaultCase (_ => _
              .Given ("a message", x => Message = "Message")
              .Given ("an inner exception", x => InnerException = new Exception ())
              .ItThrows (typeof (ArgumentException), x => Message, x => InnerException))
          .Case ("Missing exception assertion", _ => _
              .Given ("a message", x => Message = "Message")
              .Given ("an inner exception with message", x => InnerException = new Exception ("InnerMessage"))
              .It ("asserts something different", x => { }))
          .Case ("Wrong exception type", _ => _
              .ItThrows (typeof (InvalidOperationException)))
          .Case ("Wrong message", _ => _
              .ItThrows (typeof (ArgumentException), "Wrong message"))
          .Case ("Wrong message provider", _ => _
              .Given ("a message", x => Message = "Message")
              .ItThrows (typeof (ArgumentException), x => "Wrong message"))
          .Case ("Wrong inner exception provider", _ => _
              .Given ("a message", x => Message = "Message")
              .ItThrows (typeof (ArgumentException), x => "Message", x => new Exception ()));
    }
  }
}

#if !EXAMPLE

namespace TestFx.Specifications.IntegrationTests
{
  public class ExceptionTest : TestBase<ExceptionSpecK>
  {
    [Test]
    public void Test ()
    {
      AssertResult (TestResults[0], "<Default>", State.Passed);
      AssertResult (TestResults[1], "Missing exception assertion", State.Failed);
      AssertResult (TestResults[2], "Wrong exception type", State.Failed);
      AssertResult (TestResults[3], "Wrong message", State.Failed);
      AssertResult (TestResults[4], "Wrong message provider", State.Failed);
      AssertResult (TestResults[5], "Wrong inner exception provider", State.Failed);

      var actException = OperationResults.Where (x => x.Text == "<Action>").ElementAt (1).Exception.AssertNotNull ();
      actException.Name.Should ().Be ("ArgumentException");
      actException.FullName.Should ().Be ("System.ArgumentException");
      actException.StackTrace.Should ().NotContain ("at TestFx");
      actException.StackTrace.Should ().Contain ("at UserNamespace");
      actException.Message.Should ().Be ("Message\r\n---> InnerMessage");
    }
  }
}

#endif