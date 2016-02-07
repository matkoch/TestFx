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
  public class ExceptionAssertionTest : TestBase<ExceptionAssertionTest.DomainSpec>
  {
    [Subject (typeof (ExceptionAssertionTest), "Test")]
    public class DomainSpec : Spec
    {
      string Message;
      Exception InnerException;

      DomainSpec ()
      {
        Specify (x => UserClass.Throw<ArgumentException> (Message, InnerException))
            .DefaultCase (_ => _
                .Given ("a message", x => Message = "Message")
                .Given ("an inner exception", x => InnerException = new Exception ())
                .ItThrows (typeof (ArgumentException), x => Message, x => InnerException))
            .Case ("Wrong exception type", _ => _
                .ItThrows (typeof (InvalidOperationException)))
            .Case ("Wrong message", _ => _
                .ItThrows (typeof (ArgumentException), "Wrong message"))
            .Case ("Wrong message provider", _ => _
                .Given ("a message", x => Message = "Message")
                .ItThrows (typeof (ArgumentException), x => "Wrong message"))
            .Case ("Wrong inner exception provider", _ => _
                .Given ("a message", x => Message = "Message")
                .ItThrows (typeof (ArgumentException), x => "Message", x => new Exception ()))
            .Case ("Custom failing assertion", _ => _
                .ItThrows ("exception with special properties", x => x.Exception.InnerException.Should ().NotBeNull ()))
            .Case ("Custom passing assertion", _ => _
                .ItThrows ("exception with special properties", x => x.Exception.InnerException.Should ().BeNull ()));
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      var testResults = runResult.GetTestResults ();

      testResults[0]
          .HasPassed ()
          .HasRelativeId ("<Default>")
          .HasOperations (
              Constants.Reset_Instance_Fields,
              "a message",
              "an inner exception",
              Constants.Action,
              "Throws ArgumentException");

      testResults[1].HasFailed ().HasRelativeId ("Wrong exception type");
      testResults[2].HasFailed ().HasRelativeId ("Wrong message");
      testResults[3].HasFailed ().HasRelativeId ("Wrong message provider");
      testResults[4].HasFailed ().HasRelativeId ("Wrong inner exception provider");
      testResults[5].HasFailed ().HasRelativeId ("Custom failing assertion").HasFailingOperations ("Throws exception with special properties");

      testResults[6].HasPassed ().HasRelativeId ("Custom passing assertion");
    }
  }
}