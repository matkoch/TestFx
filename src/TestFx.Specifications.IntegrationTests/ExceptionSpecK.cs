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
          .Elaborate ("Case 1", _ => _
              .ItThrows<InvalidOperationException> ())
          .Elaborate ("Case 2", _ => _
              .ItThrows<ArgumentException> (x => "Wrong message"))
          .Elaborate ("Case 3", _ => _
              .Given ("a message", x => Message = "Message")
              .ItThrows<ArgumentException> (x => "Message", x => new Exception ()))
          .Elaborate ("Case 4", _ => _
              .Given ("a message", x => Message = "Message")
              .Given ("an inner exception", x => InnerException = new Exception ())
              .ItThrows<ArgumentException> (x => Message, x => InnerException))
          .Elaborate ("Case 5", _ => _
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
      AssertResult (TestResults[0], "0", "Case 1", State.Failed);
      AssertResult (TestResults[1], "1", "Case 2", State.Failed);
      AssertResult (TestResults[2], "2", "Case 3", State.Failed);
      AssertResult (TestResults[3], "3", "Case 4", State.Passed);
      AssertResult (TestResults[4], "4", "Case 5", State.Failed);

      var actException = OperationResults.Last ().Exception.AssertNotNull ();
      actException.TypeFullName.Should ().Be ("System.ArgumentException");
      actException.StackTrace.Should ().NotContain ("at TestFx");
      actException.StackTrace.Should ().Contain ("at UserNamespace");
      actException.Message.Should ().Be ("Message\r\n---> InnerMessage");
    }
  }
}

#endif