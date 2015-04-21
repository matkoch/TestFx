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
using System.Linq;
using TestFx.Utilities;

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (SubjectCreationWithMissingArgumentsConstructorSpecK), "Method")]
  public class SubjectCreationWithMissingArgumentsConstructorSpecK : SpecK<SubjectCreationWithMissingArgumentsConstructorSpecK.DomainType>
  {
    [Injected] static string MyString = "MyString";

    SubjectCreationWithMissingArgumentsConstructorSpecK ()
    {
      Specify (x => x.ToString ())
          .DefaultCase (_ => _
              .It ("don't care", x => { }));
    }

    public class DomainType
    {
      public DomainType (string firstMissingString, string myString, string secondMissingString)
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

  public class SubjectCreationWithMissingArgumentsConstructorTest : TestBase<SubjectCreationWithMissingArgumentsConstructorSpecK>
  {
    [Test]
    public void Test ()
    {
      AssertResult (TestResults[0], "<Default>", State.Failed);

      var exception = OperationResults.Single (x => x.Text == "<CreateSubject>").Exception.AssertNotNull ();
      exception.Message.Should ().Be ("Missing constructor arguments for subject type 'DomainType': firstMissingString, secondMissingString");
    }
  }
}

#endif