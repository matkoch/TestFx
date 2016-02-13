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

namespace TestFx.SpecK.Tests.Simple
{
  internal class InheritanceTest : TestBase<InheritanceTest.SpecializedDomainSpec>
  {
    public abstract class DomainSpecBase : Spec
    {
      protected DomainSpecBase ()
      {
        Specify (x => Console.WriteLine (true))
            .Case ("Base case", _ => _);
      }
    }

    [Subject (typeof (InheritanceTest), "Test")]
    internal class DomainSpec : DomainSpecBase
    {
      public DomainSpec ()
      {
        Specify (x => Console.WriteLine (true))
            .DefaultCase (_ => _
                .It ("Assertion", x => CheckSomething ()));
      }

      protected virtual void CheckSomething ()
      {
      }
    }

    [Subject (typeof (InheritanceTest), "Test")]
    internal class SpecializedDomainSpec : DomainSpec
    {
      [UsedImplicitly] object ResetableObject;

      public SpecializedDomainSpec ()
      {
        Specify (x => Console.WriteLine (true))
            .Case ("Additional case", _ => _);
      }

      protected override void CheckSomething ()
      {
        throw new Exception ();
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      var testResults = runResult.GetTestResults ();

      testResults[0].HasPassed ().HasText ("Base case");
      testResults[1].HasFailed ().HasText (Constants.Default);
      testResults[2].HasPassed ().HasText ("Additional case");
    }
  }
}