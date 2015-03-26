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
          .Case ("Simple generic", _ => _
              .ItThrows<InvalidOperationException> ())
          .Case ("Message provider", _ => _
              .ItThrows<ArgumentException> (x => "Wrong message"))
          .Case ("InnerException provider", _ => _
              .Given ("a message", x => Message = "Message")
              .ItThrows<ArgumentException> (x => "Message", x => new Exception ()))
          .Case ("Passing for message and inner exception", _ => _
              .Given ("a message", x => Message = "Message")
              .Given ("an inner exception", x => InnerException = new Exception ())
              .ItThrows<ArgumentException> (x => Message, x => InnerException))
          .Case ("Missing exception assertion", _ => _
              .Given ("a message", x => Message = "Message")
              .Given ("an inner exception with message", x => InnerException = new Exception ("InnerMessage"))
              .It ("asserts something different", x => { }));
    }
  }
}

#if !EXAMPLE

namespace TestFx.Specifications.IntegrationTests
{
  using System.Linq;
  using Evaluation.Results;
  using FluentAssertions;
  using NUnit.Framework;
  using Utilities;

  public class ExceptionTest : TestBase<ExceptionSpecK>
  {
    [Test]
    public void Test ()
    {
      AssertResult (TestResults[0], "0", "Simple generic", State.Failed);
      AssertResult (TestResults[1], "1", "Message provider", State.Failed);
      AssertResult (TestResults[2], "2", "InnerException provider", State.Failed);
      AssertResult (TestResults[3], "3", "Passing for message and inner exception", State.Passed);
      AssertResult (TestResults[4], "4", "Missing exception assertion", State.Failed);

      var actException = OperationResults.Last ().Exception.AssertNotNull ();
      actException.Name.Should ().Be ("ArgumentException");
      actException.FullName.Should ().Be ("System.ArgumentException");
      actException.StackTrace.Should ().NotContain ("at TestFx");
      actException.StackTrace.Should ().Contain ("at UserNamespace");
      actException.Message.Should ().Be ("Message\r\n---> InnerMessage");
    }
  }
}

#endif