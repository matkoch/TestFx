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

// ReSharper disable ArgumentsStyleLiteral

namespace TestFx.SpecK.Tests.Subject
{
  internal class AdHocCreationTest : TestBase<AdHocCreationTest.DomainSpec>
  {
    [Subject (typeof (AdHocCreationTest))]
    internal class DomainSpec : Spec<DomainType>
    {
      // ReSharper disable once ConvertToConstant.Local
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

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.GetTestResult ()
          .HasPassed ()
          .HasOperations (
              Constants.Reset_Instance_Fields,
              "init OtherString",
              "subject is created with OtherString",
              Constants.Action,
              "passes OtherString",
              "creates subject only once");
    }

    internal class DomainType
    {
      public static int ConstructorCalls;

      public DomainType (string injectedString)
      {
        ConstructorCalls++;

        InjectedString = injectedString;
      }

      public string InjectedString { get; }
    }
  }
}