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
using System.Collections.Generic;
using System.Linq;
using FakeItEasy.Core;
using FluentAssertions;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.Tests.Subject
{
  internal class DefaultCreationTest : TestBase<DefaultCreationTest.DomainSpec>
  {
    [Subject (typeof (NoDefaultConstructorTest))]
    internal class DomainSpec : Spec<DomainType>
    {
      [Injected] static string InjectedString = "MyString";

      DomainSpec ()
      {
        Specify (x => 0)
            .DefaultCase (_ => _
                .It ("passes InjectedString", x => x.Subject.InjectedString.Should ().Be ("MyString"))
                .It ("creates subject only once", x => DomainType.ConstructorCalls.Should ().Be (1)));
      }
    }


    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.GetTestResult ()
          .HasPassed ()
          .HasOperations (Constants.Create_Subject, Constants.Action,
              "passes InjectedString",
              "creates subject only once");
    }

    internal class DomainType
    {
      public static int ConstructorCalls;

      public DomainType (IEnumerable<char> injectedString)
      {
        ConstructorCalls++;

        InjectedString = new string (injectedString.ToArray ());
      }

      public string InjectedString { set; get; }
    }
  }
}