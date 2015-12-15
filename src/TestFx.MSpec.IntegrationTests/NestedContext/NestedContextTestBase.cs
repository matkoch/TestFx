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

namespace TestFx.MSpec.IntegrationTests.NestedContext
{
  public class given_context
  {
    Establish ctx = () => NestedContextTestBase.OuterSetup ();

    Cleanup stuff = () => NestedContextTestBase.OuterCleanup ();

    [Subject (typeof (int))]
    public class when_calling
    {
      Establish ctx = () => NestedContextTestBase.Setup ();

      Because of = () => NestedContextTestBase.Action ();

      It asserts = () => NestedContextTestBase.Assertion ();

      Cleanup stuff = () => NestedContextTestBase.Cleanup ();
    }
  }

  public abstract class NestedContextTestBase : TestBase<given_context.when_calling>
  {
    public static Action OuterSetup;
    public static Action Setup;
    public static Action Action;
    public static Action Assertion;
    public static Action Cleanup;
    public static Action OuterCleanup;

    public static Action ThrowingAction = () => { throw new Exception(); };

    public override void SetUp()
    {
      base.SetUp();

      OuterSetup = A.Fake<Action>();
      Setup = A.Fake<Action>();
      Action = A.Fake<Action>();
      Assertion = A.Fake<Action>();
      Cleanup = A.Fake<Action>();
      OuterCleanup = A.Fake<Action>();
    }
  }
}