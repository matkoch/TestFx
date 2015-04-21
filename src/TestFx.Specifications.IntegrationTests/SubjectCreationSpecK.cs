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
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (SubjectCreationSpecK), "Method")]
  public class SubjectCreationSpecK : SpecK<SubjectCreationSpecK.DomainType>
  {
    [Injected] static string InjectedString = "MyString";
    string TempString;

    SubjectCreationSpecK ()
    {
      Specify (x => x.ToString ())
          .DefaultCase (_ => _
              .It ("passes InjectedString", x => x.Subject.InjectedString.Should ().Be ("MyString")))
          .Case ("Custom subject creation", _ => _
              .Given ("init TempString", x => TempString = "OtherString")
              .GivenSubject ("is created with TempString", x => new DomainType (TempString))
              .It ("passes TempString", x => x.Subject.InjectedString.Should ().Be ("OtherString")));
    }

    public class DomainType
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

#if !EXAMPLE

namespace TestFx.Specifications.IntegrationTests
{
  using Evaluation.Results;
  using NUnit.Framework;

  public class SubjectCreationTest : TestBase<SubjectCreationSpecK>
  {
    [Test]
    public void Test ()
    {
      RunResult.State.Should ().Be (State.Passed);

      SubjectCreationSpecK.DomainType.ConstructorCalls.Should ().Be (2);

      AssertResult (TestResults[0], "<Default>", State.Passed);
      AssertResult (TestResults[1], "Custom subject creation", State.Passed);
    }
  }
}

#endif