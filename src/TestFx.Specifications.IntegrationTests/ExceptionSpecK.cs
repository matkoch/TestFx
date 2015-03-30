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
          .DefaultCase (_ => _
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
      AssertResult (TestResults[0], "<Default>", State.Failed);
      AssertResult (TestResults[1], "Message provider", State.Failed);
      AssertResult (TestResults[2], "InnerException provider", State.Failed);
      AssertResult (TestResults[3], "Passing for message and inner exception", State.Passed);
      AssertResult (TestResults[4], "Missing exception assertion", State.Failed);

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