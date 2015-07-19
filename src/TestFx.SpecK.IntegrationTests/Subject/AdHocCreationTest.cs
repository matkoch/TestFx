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

namespace TestFx.SpecK.IntegrationTests.Subject
{
  public class AdHocCreationTest : TestBase<AdHocCreationTest.DomainSpec>
  {
    [Subject (typeof (AdHocCreationTest), "Test")]
    public class DomainSpec : Spec<DomainType>
    {
      [Injected] static string InjectedString = "MyString";
      string OtherString;

      DomainSpec ()
      {
        Specify (x => 0)
            .DefaultCase (_ => _
                .Given ("init OtherString", x => OtherString = "OtherString")
                .GivenSubject ("is created with OtherString", x => new DomainType (OtherString))
                .It ("passes OtherString", x => x.Subject.InjectedString.Should ().Be ("OtherString"))
                .It ("creates subject only once", x => DomainType.ConstructorCalls.Should ().Be (1)));
      }
    }

    [Test]
    public override void Test ()
    {
      AssertTest (Default, State.Passed)
          .WithOperations (
              Reset_Instance_Fields,
              "init OtherString",
              "subject is created with OtherString",
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