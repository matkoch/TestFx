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
using JetBrains.Annotations;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

// ReSharper disable ArgumentsStyleLiteral

namespace TestFx.SpecK.Tests.Simple
{
  internal class PassingTest : TestBase<PassingTest.DomainSpec>
  {
    [Subject (typeof (PassingTest), "Test")]
    internal class DomainSpec : Spec
    {
      [UsedImplicitly] object ResetableObject;

      public DomainSpec ()
      {
        Specify (x => Console.WriteLine (true))
            .DefaultCase (_ => _
                .Given (x => { })
                .It ("Assertion", x => { }));
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.HasPassed ();

      runResult.GetAssemblySuiteResult ()
          .HasPassed ()
          .HasRelativeId (typeof (DomainSpec).Assembly.Location)
          .HasText ("TestFx.SpecK.Tests");

      runResult.GetClassSuiteResult ()
          .HasPassed ()
          .HasRelativeId ("TestFx.SpecK.Tests.Simple.PassingTest+DomainSpec")
          .HasText ("PassingTest.Test");

      runResult.GetTestResult ()
          .HasPassed ()
          .HasRelativeId (Constants.Default)
          .HasText (Constants.Default)
          .HasOperations (
              Constants.Reset_Instance_Fields,
              "<Arrangement>", Constants.Action,
              "Assertion");
    }
  }
}