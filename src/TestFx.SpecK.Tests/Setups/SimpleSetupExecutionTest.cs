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
using FakeItEasy.Core;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.Tests.Setups
{
  public class SimpleSetupExecutionTest : SetupTestBase
  {
    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      using (scope.OrderedAssertions ())
      {
        A.CallTo (() => AssemblySetupAction ()).MustHaveHappened ();
        A.CallTo (() => SetupOnceAction1 ()).MustHaveHappened ();
        A.CallTo (() => SetupOnceAction2 ()).MustHaveHappened ();
        A.CallTo (() => SetupAction (A<ITestContext<object>>.That.Matches (x => x.Subject.Equals (Subject1)))).MustHaveHappened ();
        A.CallTo (() => CleanupAction (A<ITestContext<object>>.That.Matches (x => x.Subject.Equals (Subject1)))).MustHaveHappened ();
        A.CallTo (() => SetupAction (A<ITestContext<object>>.That.Matches (x => x.Subject.Equals (Subject2)))).MustHaveHappened ();
        A.CallTo (() => CleanupAction (A<ITestContext<object>>.That.Matches (x => x.Subject.Equals (Subject2)))).MustHaveHappened ();
        A.CallTo (() => CleanupOnceAction2 ()).MustHaveHappened ();
        A.CallTo (() => CleanupOnceAction1 ()).MustHaveHappened ();
        A.CallTo (() => AssemblyCleanupAction ()).MustHaveHappened ();
      }

      var assemblyResult = runResult.GetAssemblySuiteResult ();
      assemblyResult.SetupResults.Single ().HasText ("MyAssemblySetup.Setup");
      assemblyResult.CleanupResults.Single ().HasText ("MyAssemblySetup.Cleanup");

      var classResult = runResult.GetClassSuiteResult ();
      classResult.SetupResults.ElementAt (0).HasText ("SetupOnceMethod");
      classResult.SetupResults.ElementAt (1).HasText ("<lambda method>");
      classResult.CleanupResults.ElementAt (0).HasText ("<lambda method>");
      classResult.CleanupResults.ElementAt (1).HasText ("CleanupOnceMethod");
    }
  }
}