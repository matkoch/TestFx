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
using TestFx.Extensibility.Providers;

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (ContextBehaviorSpecK), "Method")]
  public class ContextBehaviorSpecK : SpecK<ContextBehaviorSpecK.DomainType>
  {
    ContextBehaviorSpecK ()
    {
      Specify (x => x.Property)
          .DefaultCase (_ => _
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
  public class ContextBehaviorTest : TestBase<ContextBehaviorSpecK>
  {
    [Test]
    public override void Test ()
    {
      AssertTestFailed ("<Default>",
          operationTexts: new[]
                          {
                              "subject with ctor arg",
                              "<Action>",
                              "has result set to ctor arg",
                              "has property set to null"
                          },
          failedOperationTexts: new[] { "has property set to null" });
    }
  }
}

#endif