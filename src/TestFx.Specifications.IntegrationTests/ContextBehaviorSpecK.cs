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
using FluentAssertions;

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (ContextBehaviorSpecK), "Method")]
  public class ContextBehaviorSpecK : SpecK<ContextBehaviorSpecK.DomainType>
  {
    ContextBehaviorSpecK ()
    {
      Specify (x => x.Property)
          .Elaborate ("Case 1", _ => _
              .Given (SetSubject ("ctor arg"))
              .It (AssertProperty ("ctor arg")));
    }

    Context<DomainType> SetSubject (string value)
    {
      return context => context
          .GivenSubject ("with ctor arg", x => new DomainType (value));
    }

    Behavior<DomainType, string> AssertProperty (string value)
    {
      return behavior => behavior
          .It ("has result set to " + value, x => x.Result.Should ().Be (value))
          .It ("has property set to null", x => x.Subject.Property.Should ().BeNull ());
    }

    public class DomainType
    {
      public DomainType (string property)
      {
        Property = property;
      }

      public string Property { get; set; }
    }
  }
}

#if !EXAMPLE

namespace TestFx.Specifications.IntegrationTests
{
  using Evaluation.Results;
  using Extensibility.Providers;
  using NUnit.Framework;

  public class ContextBehaviorTest : TestBase<ContextBehaviorSpecK>
  {
    [Test]
    public void Test ()
    {
      RunResult.State.Should ().Be (State.Failed);

      AssertResult (OperationResults[0], "<OPERATION>", "subject with ctor arg", State.Passed, OperationType.Action);
      AssertResult (OperationResults[2], "<OPERATION>", "has result set to ctor arg", State.Passed, OperationType.Assertion);
      AssertResult (OperationResults[3], "<OPERATION>", "has property set to null", State.Failed, OperationType.Assertion);
    }
  }
}

#endif