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

namespace TestFx.Specifications.IntegrationTests.Subject
{
  public class CustomCreationTest : TestBase<CustomCreationTest.DomainSpecK>
  {
    [Subject (typeof (CustomCreationTest), "Test")]
    public class DomainSpecK : SpecK<DomainType>
    {
      [Injected] static string InjectedString = "MyString";

      DomainSpecK ()
      {
        Specify (x => 0)
            .DefaultCase (_ => _
                .It ("passes OtherString", x => x.Subject.InjectedString.Should ().Be ("OtherString"))
                .It ("creates subject only once", x => DomainType.ConstructorCalls.Should ().Be (1)));
      }

      public override DomainType CreateSubject ()
      {
        return new DomainType ("OtherString");
      }
    }

    [Test]
    public override void Test ()
    {
      AssertTest (Default, State.Passed)
          .WithOperations (
              Create_Subject,
              Action,
              "passes OtherString",
              "creates subject only once");
    }

    public class DomainType
    {
      public static int ConstructorCalls;

      public DomainType (string injectedString)
      {
        ConstructorCalls++;

        InjectedString = injectedString;
      }

      public string InjectedString { set; get; }
    }
  }
}