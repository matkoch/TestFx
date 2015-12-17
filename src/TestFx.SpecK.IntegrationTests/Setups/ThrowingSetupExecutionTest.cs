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
using FakeItEasy.Core;
using TestFx.Evaluation.Results;

namespace TestFx.SpecK.IntegrationTests.Setups
{
  public class ThrowingSetupExecutionTest : SetupTestBase
  {
    public override void SetUp ()
    {
      base.SetUp ();

      SetupOnceAction2 = ThrowingAction;
    }

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