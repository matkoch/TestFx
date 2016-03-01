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
using FakeItEasy;
using FakeItEasy.Core;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.Tests.Setups
{
  internal class ThrowingSetupExecutionTest : TestBase<ThrowingSetupExecutionTest.DomainSpec>
  {
    [Subject (typeof (ThrowingSetupExecutionTest))]
    internal class DomainSpec : Spec<object>
    {
      // TODO: Assert NotNull + Value Init
      [AssemblySetup] public static MyAssemblySetup MyAssemblySetup;

      public DomainSpec ()
      {
        SetupOnce (SetupOnceMethod, CleanupOnceMethod);
        SetupOnce (SetupOnceAction2, CleanupOnceAction2);
        Setup (SetupAction, CleanupAction);

        Specify (x => 1)
            .DefaultCase (_ => _)
            .Case ("Case 2", _ => _);
      }

      static void SetupOnceMethod ()
      {
        SetupOnceAction1 ();
      }

      static void CleanupOnceMethod ()
      {
        CleanupOnceAction1 ();
      }
    }

    internal class MyAssemblySetup : IAssemblySetup
    {
      public void Setup ()
      {
        AssemblySetupAction ();
      }

      public void Cleanup ()
      {
        AssemblyCleanupAction ();
      }
    }

    static readonly Action AssemblySetupAction = A.Fake<Action> ();
    static readonly Action AssemblyCleanupAction = A.Fake<Action> ();

    static readonly Action SetupOnceAction1 = A.Fake<Action> ();
    static readonly Action SetupOnceAction2 = () => { throw new Exception (); };
    static readonly Action CleanupOnceAction1 = A.Fake<Action> ();
    static readonly Action CleanupOnceAction2 = A.Fake<Action> ();

    static readonly Action<ITestContext<object>> SetupAction = A.Fake<Action<ITestContext<object>>> ();
    static readonly Action<ITestContext<object>> CleanupAction = A.Fake<Action<ITestContext<object>>> ();

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      using (scope.OrderedAssertions ())
      {
        A.CallTo (() => AssemblySetupAction ()).MustHaveHappened ();
        A.CallTo (() => SetupOnceAction1 ()).MustHaveHappened ();

        A.CallTo (() => SetupAction (A<ITestContext<object>>._)).MustNotHaveHappened ();
        A.CallTo (() => CleanupAction (A<ITestContext<object>>._)).MustNotHaveHappened ();
        A.CallTo (() => SetupAction (A<ITestContext<object>>._)).MustNotHaveHappened ();
        A.CallTo (() => CleanupAction (A<ITestContext<object>>._)).MustNotHaveHappened ();
        A.CallTo (() => CleanupOnceAction2 ()).MustNotHaveHappened ();
        A.CallTo (() => CleanupOnceAction1 ()).MustHaveHappened ();
        A.CallTo (() => AssemblyCleanupAction ()).MustHaveHappened ();
      }
    }
  }
}