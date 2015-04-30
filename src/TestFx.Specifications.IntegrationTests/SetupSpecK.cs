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

namespace TestFx.Specifications.IntegrationTests
{
  public class MyAssemblySetup : IAssemblySetup
  {
    public static Action AssemblySetupAction = A.Fake<Action> ();
    public static Action AssemblyCleanupAction = A.Fake<Action> ();

    public void Setup ()
    {
      AssemblySetupAction ();
    }

    public void Cleanup ()
    {
      AssemblyCleanupAction ();
    }
  }

  [Subject (typeof (SetupSpecK), "Method")]
  public class SetupSpecK : SpecK<object>
  {
    public static readonly Action SetupOnceAction1 = A.Fake<Action> ();
    public static readonly Action SetupOnceAction2 = A.Fake<Action> ();
    public static readonly Action CleanupOnceAction1 = A.Fake<Action> ();
    public static readonly Action CleanupOnceAction2 = A.Fake<Action> ();

    public static readonly Action<ITestContext<object>> SetupAction = A.Fake<Action<ITestContext<object>>> ();
    public static readonly Action<ITestContext<object>> CleanupAction = A.Fake<Action<ITestContext<object>>> ();

    public static readonly object Subject1 = new object ();
    public static readonly object Subject2 = new object ();

    public static MyAssemblySetup MyAssemblySetup;

    public SetupSpecK ()
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

#if !EXAMPLE

namespace TestFx.Specifications.IntegrationTests
{
  using System.Linq;
  using FluentAssertions;
  using NUnit.Framework;

  public class SetupTest : TestBase<SetupSpecK>
  {
    [SetUp]
    public override void SetUp()
    {
      MyAssemblySetup.AssemblySetupAction = A.Fake<Action> ();
      MyAssemblySetup.AssemblyCleanupAction = A.Fake<Action> ();

      base.SetUp ();
    }

    [Test]
    public override void Test ()
    {
      using (Scope.OrderedAssertions ())
      {
        A.CallTo (() => MyAssemblySetup.AssemblySetupAction ()).MustHaveHappened (Repeated.Exactly.Once);

        A.CallTo (() => SetupSpecK.SetupOnceAction1 ()).MustHaveHappened (Repeated.Exactly.Once);
        A.CallTo (() => SetupSpecK.SetupOnceAction2 ()).MustHaveHappened (Repeated.Exactly.Once);

        A.CallTo (() => SetupSpecK.SetupAction (A<ITestContext<object>>.That.Matches (x => x.Subject == SetupSpecK.Subject1)))
            .MustHaveHappened (Repeated.Exactly.Once);
        A.CallTo (() => SetupSpecK.CleanupAction (A<ITestContext<object>>.That.Matches (x => x.Subject == SetupSpecK.Subject1)))
            .MustHaveHappened (Repeated.Exactly.Once);

        A.CallTo (() => SetupSpecK.SetupAction (A<ITestContext<object>>.That.Matches (x => x.Subject == SetupSpecK.Subject2)))
            .MustHaveHappened (Repeated.Exactly.Once);
        A.CallTo (() => SetupSpecK.CleanupAction (A<ITestContext<object>>.That.Matches (x => x.Subject == SetupSpecK.Subject2)))
            .MustHaveHappened (Repeated.Exactly.Once);

        A.CallTo (() => SetupSpecK.CleanupOnceAction2 ()).MustHaveHappened (Repeated.Exactly.Once);
        A.CallTo (() => SetupSpecK.CleanupOnceAction1 ()).MustHaveHappened (Repeated.Exactly.Once);

        A.CallTo (() => MyAssemblySetup.AssemblyCleanupAction ()).MustHaveHappened (Repeated.Exactly.Once);
      }

      AssemblyResults[0].SetupResults.ElementAt (0).Text.Should ().Be ("MyAssemblySetup.Setup");
      AssemblyResults[0].CleanupResults.ElementAt (0).Text.Should ().Be ("MyAssemblySetup.Cleanup");

      TypeResults[0].SetupResults.ElementAt (0).Text.Should ().Be ("SetupOnceMethod");
      TypeResults[0].SetupResults.ElementAt (1).Text.Should ().Be ("<lambda method>");
      TypeResults[0].CleanupResults.ElementAt (0).Text.Should ().Be ("<lambda method>");
      TypeResults[0].CleanupResults.ElementAt (1).Text.Should ().Be ("CleanupOnceMethod");
    }
  }
}

#endif