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
  public class NoDefaultConstructorTest : TestBase<NoDefaultConstructorTest.DomainSpecK>
  {
    [Subject (typeof (NoDefaultConstructorTest), "Test")]
    public class DomainSpecK : SpecK<DomainType>
    {
      [Injected] static string MyString = "MyString";

      DomainSpecK ()
      {
        Specify (x => 0)
            .DefaultCase (_ => _);
      }
    }

    [Test]
    public override void Test ()
    {
      AssertTest (Default, State.Failed)
          .WithFailureDetails (
              Create_Subject,
              message: "Missing default constructor for subject type 'DomainType'.");
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