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
using FakeItEasy;
using TestFx.Specifications.InferredApi;

namespace TestFx.Specifications.IntegrationTests
{
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

    public SetupSpecK ()
    {
      SetupOnce (SetupOnceMethod, CleanupOnceMethod);
      SetupOnce (SetupOnceAction2, CleanupOnceAction2);
      Setup (SetupAction, CleanupAction);

      Specify (x => 1)
          .Elaborate ("Case 1", _ => _
              .GivenSubject ("static subject1", x => Subject1))
          .Elaborate ("Case 2", _ => _
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
    [Test]
    public void Test ()
    {
      using (Scope.OrderedAssertions ())
      {
        A.CallTo (() => SetupSpecK.SetupOnceAction1 ()).MustHaveHappened ();
        A.CallTo (() => SetupSpecK.SetupOnceAction2 ()).MustHaveHappened ();

        A.CallTo (() => SetupSpecK.SetupAction (A<ITestContext<object>>.That.Matches (x => x.Subject == SetupSpecK.Subject1))).MustHaveHappened ();
        A.CallTo (() => SetupSpecK.CleanupAction (A<ITestContext<object>>.That.Matches (x => x.Subject == SetupSpecK.Subject1))).MustHaveHappened ();

        A.CallTo (() => SetupSpecK.SetupAction (A<ITestContext<object>>.That.Matches (x => x.Subject == SetupSpecK.Subject2))).MustHaveHappened ();
        A.CallTo (() => SetupSpecK.CleanupAction (A<ITestContext<object>>.That.Matches (x => x.Subject == SetupSpecK.Subject2))).MustHaveHappened ();

        A.CallTo (() => SetupSpecK.CleanupOnceAction2 ()).MustHaveHappened ();
        A.CallTo (() => SetupSpecK.CleanupOnceAction1 ()).MustHaveHappened ();
      }

      TypeResults[0].SetupResults.ElementAt (0).Text.Should ().Be ("SetupOnceMethod");
      TypeResults[0].SetupResults.ElementAt (1).Text.Should ().Be ("<lambda method>");
      TypeResults[0].CleanupResults.ElementAt (0).Text.Should ().Be ("<lambda method>");
      TypeResults[0].CleanupResults.ElementAt (1).Text.Should ().Be ("CleanupOnceMethod");
    }
  }
}

#endif