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
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace TestFx.SpecK.IntegrationTests
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

  [Subject (typeof (SetupSpec), "Method")]
  public class SetupSpec : Spec<object>
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

#if !EXAMPLE

  public class SetupTest : TestBase<SetupSpec>
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

        A.CallTo (() => SetupSpec.SetupOnceAction1 ()).MustHaveHappened (Repeated.Exactly.Once);
        A.CallTo (() => SetupSpec.SetupOnceAction2 ()).MustHaveHappened (Repeated.Exactly.Once);

        A.CallTo (() => SetupSpec.SetupAction (A<ITestContext<object>>.That.Matches (x => x.Subject == SetupSpec.Subject1)))
            .MustHaveHappened (Repeated.Exactly.Once);
        A.CallTo (() => SetupSpec.CleanupAction (A<ITestContext<object>>.That.Matches (x => x.Subject == SetupSpec.Subject1)))
            .MustHaveHappened (Repeated.Exactly.Once);

        A.CallTo (() => SetupSpec.SetupAction (A<ITestContext<object>>.That.Matches (x => x.Subject == SetupSpec.Subject2)))
            .MustHaveHappened (Repeated.Exactly.Once);
        A.CallTo (() => SetupSpec.CleanupAction (A<ITestContext<object>>.That.Matches (x => x.Subject == SetupSpec.Subject2)))
            .MustHaveHappened (Repeated.Exactly.Once);

        A.CallTo (() => SetupSpec.CleanupOnceAction2 ()).MustHaveHappened (Repeated.Exactly.Once);
        A.CallTo (() => SetupSpec.CleanupOnceAction1 ()).MustHaveHappened (Repeated.Exactly.Once);

        A.CallTo (() => MyAssemblySetup.AssemblyCleanupAction ()).MustHaveHappened (Repeated.Exactly.Once);
      }

      var assemblyResult = RunResult.SuiteResults.Single ();
      assemblyResult.SetupResults.ElementAt (0).Text.Should ().Be ("MyAssemblySetup.Setup");
      assemblyResult.CleanupResults.ElementAt (0).Text.Should ().Be ("MyAssemblySetup.Cleanup");

      var typeResult = assemblyResult.SuiteResults.Single ();
      typeResult.SetupResults.ElementAt (0).Text.Should ().Be ("SetupOnceMethod");
      typeResult.SetupResults.ElementAt (1).Text.Should ().Be ("<lambda method>");
      typeResult.CleanupResults.ElementAt (0).Text.Should ().Be ("<lambda method>");
      typeResult.CleanupResults.ElementAt (1).Text.Should ().Be ("CleanupOnceMethod");
    }
  }

#endif
}