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
using FakeItEasy;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.IntegrationTests.Setups
{
  public abstract class SetupTestBase : TestBase<SetupTestBase.SetupSpec>
  {
    public static Action AssemblySetupAction;
    public static Action AssemblyCleanupAction;

    public static Action SetupOnceAction1;
    public static Action SetupOnceAction2;
    public static Action CleanupOnceAction1;
    public static Action CleanupOnceAction2;

    public static Action<ITestContext<object>> SetupAction;
    public static Action<ITestContext<object>> CleanupAction;

    public static object Subject1;
    public static object Subject2;

    public static Action ThrowingAction = () => { throw new Exception (); };

    public override void SetUp ()
    {
      base.SetUp ();

      AssemblySetupAction = A.Fake<Action>();
      SetupOnceAction1 = A.Fake<Action>();
      SetupOnceAction2 = A.Fake<Action>();
      SetupAction = A.Fake<Action<ITestContext<object>>>();
      CleanupAction = A.Fake<Action<ITestContext<object>>>();
      CleanupOnceAction2 = A.Fake<Action>();
      CleanupOnceAction1 = A.Fake<Action>();
      AssemblyCleanupAction = A.Fake<Action>();

      Subject1 = new object ();
      Subject2 = new object ();
    }

    [Subject (typeof (SetupSpec), "Test")]
    public class SetupSpec : Spec<object>
    {
      [AssemblySetup] public static MyAssemblySetup MyAssemblySetup;

      public SetupSpec ()
      {
        SetupOnce (SetupOnceMethod, CleanupOnceMethod);
        SetupOnce (SetupOnceAction2, CleanupOnceAction2);
        Setup (SetupAction, CleanupAction);

        Specify (x => 1)
            .DefaultCase (_ => _
                .GivenSubject ("static subject1", x => Subject1))
            .Case ("Case 2", _ => _
                .GivenSubject ("static subject2", x => Subject2));
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
  }

  public class MyAssemblySetup : IAssemblySetup
  {
    public void Setup()
    {
      SetupTestBase.AssemblySetupAction();
    }

    public void Cleanup()
    {
      SetupTestBase.AssemblyCleanupAction();
    }
  }
}