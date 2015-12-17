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
using FakeItEasy.Core;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.IntegrationTests.Subject
{
  public class NoDefaultConstructorTest : TestBase<NoDefaultConstructorTest.DomainSpec>
  {
    [Subject (typeof (NoDefaultConstructorTest), "Test")]
    public class DomainSpec : Spec<DomainType>
    {
      [Injected] static string MyString = "MyString";

      DomainSpec ()
      {
        Specify (x => 0)
            .DefaultCase (_ => _);
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.GetTestResult ()
          .HasFailed ()
          .HasFailingOperation (Constants.Create_Subject,
              "Missing default constructor for subject type 'DomainType'.");
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