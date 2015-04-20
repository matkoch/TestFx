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
using System.Linq;

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (SubjectCreationWithoutDefaultConstructorSpecK), "Method")]
  public class SubjectCreationWithoutDefaultConstructorSpecK : SpecK<SubjectCreationWithoutDefaultConstructorSpecK.DomainType>
  {
    [Injected] static string MyString = "MyString";

    SubjectCreationWithoutDefaultConstructorSpecK ()
    {
      Specify (x => x.ToString ())
          .DefaultCase (_ => _
              .It ("don't care", x => { }));
    }

    public class DomainType
    {
      public DomainType ()
      {
      }

      public DomainType (string myString = "default")
      {
      }
    }
  }
}

#if !EXAMPLE

namespace TestFx.Specifications.IntegrationTests
{
  using Evaluation.Results;
  using NUnit.Framework;
  using FluentAssertions;

  public class SubjectCreationWithoutDefaultConstructorTest : TestBase<SubjectCreationWithoutDefaultConstructorSpecK>
  {
    [Test]
    public void Test ()
    {
      AssertResult (TestResults[0], "<Default>", State.Failed);

      var exception = OperationResults.Single (x => x.Text == "<CreateSubject>").Exception;
      exception.Message.Should ().Be ("Missing default constructor for subject type 'DomainType'.");
    }
  }
}

#endif