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
using FluentAssertions;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using UserNamespace;

namespace TestFx.Specifications.IntegrationTests.Exceptions
{
  public class ExceptionAssertionTest : TestBase<ExceptionAssertionTest.DomainSpecK>
  {
    [Subject (typeof (ExceptionAssertionTest), "Test")]
    public class DomainSpecK : SpecK
    {
      string Message;
      Exception InnerException;

      DomainSpecK ()
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

    [Test]
    public override void Test ()
    {
      AssertTest (Default, State.Passed)
          .WithOperations (
              Reset_Instance_Fields,
              "a message",
              "an inner exception",
              Action,
              "Throws ArgumentException");

      AssertTest ("Wrong exception type", State.Failed);
      AssertTest ("Wrong message", State.Failed);
      AssertTest ("Wrong message provider", State.Failed);
      AssertTest ("Wrong inner exception provider", State.Failed);
      AssertTest ("Custom failing assertion", State.Failed)
          .WithFailures ("Throws exception with special properties");

      AssertTest ("Custom passing assertion", State.Passed);
    }
  }
}